using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
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
            AsyncQuickSort.FinishedSorting += AsyncQuickSort_FinishedSorting;
        }

        private void AsyncQuickSort_FinishedSorting(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                buttonGenerate.Enabled = true;
                buttonSort.Enabled = true;
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics gr = Graphics.FromImage(bm);
            gr.Clear(Color.White);
            pictureBox.Image = bm;
            PictureBoxHelperClass.PictureBox = pictureBox;
            PictureBoxHelperClass.Array = array;
        }

        double[] array = new double[0];

        private void QuickSort_ValuesSwitched(object sender, ValuesSwitchedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                //richTextBox1.AppendText($"values Changed. Index1: {e.Index1}, Index2: {e.Index2}, Value1: {e.Value1}, Value2: {e.Value2}\n");
                //richTextBox1.AppendText($"ThreadID: {e.CurrentThread.ManagedThreadId}, Time: {(float)(DateTime.Now.Second + (float)DateTime.Now.Millisecond / 1000f)}, Color: {e.Color.ToString()}\n");
                //UpdateSamples(e.Index1, e.Index2, e.Value1, e.Value2);
                //PictureBoxHelperClass.Array = array;
                PictureBoxHelperClass.E = e;
                PictureBoxHelperClass.BigUpdate = false;

                pictureBox.Refresh();
                //pictureBox.Invalidate();
            });
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

            PictureBoxHelperClass.Array = array;
            PictureBoxHelperClass.BigUpdate = true;
            PictureBoxHelperClass.PictureBoxRePaint();
            
        }

        private async void buttonSort_Click(object sender, EventArgs e)
        {
            QuickSort.Stop = false;

            buttonGenerate.Enabled = false;
            buttonSort.Enabled = false;

            switch (sortingAlgorithm)
            {
                case CurrentSortingAlgorithm.QuickSort:
                    await Task.Run(() => AsyncQuickSort.Sort1Thread(array, 0, array.Length - 1));
                    break;

                case CurrentSortingAlgorithm.QuickSort2Threads:
                    await Task.Run(() => AsyncQuickSort.Sort2Threads(array, 0, array.Length - 1));
                    //await Task.Factory.StartNew(() => AsyncQuickSort.Sort2Threads(array, 0, array.Length - 1));
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
            buttonGenerate.Enabled = true;
            buttonSort.Enabled = true;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            //PictureBoxHelperClass.Array = array;
            (PictureBoxHelperClass.DecidePictureBoxUpdate())();
        }
    }
}
