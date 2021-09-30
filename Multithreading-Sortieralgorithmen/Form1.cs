using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multithreading_Sortieralgorithmen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            QuickSort.ValuesSwitched += QuickSort_ValuesSwitched;
            Bitmap bm = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics gr = Graphics.FromImage(bm);
            gr.Clear(Color.White);
            pictureBox.Image = bm;
        }

        double[] array = new double[0];

        private void QuickSort_ValuesSwitched(object sender, ValuesSwitchedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                //richTextBox1.AppendText($"values Changed. Index1: {e.Index1}, Index2: {e.Index2}, Value1: {e.Value1}, Value2: {e.Value2}\n");
                richTextBox1.AppendText($"ThreadID: {e.CurrentThread.ManagedThreadId}, Time: {(float)(DateTime.Now.Second + (float)DateTime.Now.Millisecond / 1000f)}\n");
                //UpdateSamples(e.Index1, e.Index2, e.Value1, e.Value2);
                pictureBox.Refresh();
            });
        }

        private void DrawSamples()
        {
            Bitmap bm = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(bm);

            g.Clear(Color.White);

            for (int i = 0; i < array.Length; i++)
            {
                int x = (int)(((double)pictureBox.Width / array.Length) * i);

                Pen pen = new Pen(Color.Black);
                Point one = new Point(x, pictureBox.Height);
                Point two = new Point(x, (int)(pictureBox.Height - (int)((array[i] / (double)int.MaxValue) * pictureBox.Height)));
                g.DrawLine(pen, one, two);
            }

            pictureBox.Image = bm;
        }

        private void UpdateSamples(int index1, int index2, double value1, double value2)
        {
            Bitmap bm = new Bitmap(pictureBox.Image);
            Graphics g = Graphics.FromImage(bm);

            //Clear the Bitmap at index1 and index2
            using (Pen penWhite = new Pen(Color.White))
            {
                int x = (int)(((double)pictureBox.Width / array.Length) * index1);
                g.DrawLine(penWhite, new Point(x, pictureBox.Height), new Point(x, 0));
                x = (int)(((double)pictureBox.Width / array.Length) * index2);
                g.DrawLine(penWhite, new Point(x, pictureBox.Height), new Point(x, 0));
            }

            //Draw new Lines at index1 and index2
            using (Pen penBlack = new Pen(Color.Black))
            {
                int x = (int)(((double)pictureBox.Width / array.Length) * index1);
                g.DrawLine(
                       penBlack,
                       new Point(x, pictureBox.Height),
                       new Point(x, (int)(pictureBox.Height - (int)((value1 / (double)int.MaxValue) * pictureBox.Height)))
                       );

                x = (int)(((double)pictureBox.Width / array.Length) * index2);
                g.DrawLine(
                        penBlack,
                        new Point(x, pictureBox.Height),
                        new Point(x, (int)(pictureBox.Height - (int)((value2 / (double)int.MaxValue) * pictureBox.Height)))
                        );
            }

            pictureBox.Image = bm;
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input.Trim(' ') != string.Empty)
                e.Cancel = !(int.TryParse(input, out int a));
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            string input = textBoxElements.Text;
            if (input.Trim(' ') == string.Empty)
                return;

            array = new double[Convert.ToInt32(input)];
            Random random = new Random();
            
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(0, int.MaxValue);
            }

            DrawSamples();
        }

        private async void buttonSort_Click(object sender, EventArgs e)
        {
            QuickSort.Stop = false;
            richTextBox1.Clear();

            switch (sortingAlgorithm)
            {
                case CurrentSortingAlgorithm.QuickSort:
                    await Task.Run(() => QuickSort.Sort(array, 0, array.Length - 1));
                    break;

                case CurrentSortingAlgorithm.QuickSort2Threads:
                    await Task.Run(() => AsyncQuickSort.Sort2Threads(array, 0, array.Length - 1));
                    break;

                case CurrentSortingAlgorithm.QuickSort4Threads:
                    await Task.Run(() => AsyncQuickSort.Sort4Threads(array, 0, array.Length - 1));
                    break;
            }
        }

        enum CurrentSortingAlgorithm { QuickSort, QuickSort2Threads, QuickSort4Threads };
        CurrentSortingAlgorithm sortingAlgorithm = CurrentSortingAlgorithm.QuickSort;

        private void rbQuickSort_CheckedChanged(object sender, EventArgs e)
        {
            sortingAlgorithm = CurrentSortingAlgorithm.QuickSort;
        }

        private void rbQuickSort2Threads_CheckedChanged(object sender, EventArgs e)
        {
            sortingAlgorithm = CurrentSortingAlgorithm.QuickSort2Threads;
        }

        private void rbQuickSort4Threads_CheckedChanged(object sender, EventArgs e)
        {
            sortingAlgorithm = CurrentSortingAlgorithm.QuickSort4Threads;
        }

        private void buttonBreak_Click(object sender, EventArgs e)
        {
            QuickSort.Stop = true;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            DrawSamples();
        }
    }
}
