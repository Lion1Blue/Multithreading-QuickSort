using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multithreading_Sortieralgorithmen
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            QuickSort.ValuesSwitched += QuickSort_ValuesSwitched;
            AsyncQuickSort.FinishedSorting += AsyncQuickSort_FinishedSorting;

            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            PictureBoxHelperClass.PictureBox = pictureBox;
            PictureBoxHelperClass.Array = array;
        }

        double[] array = new double[0];

        private void QuickSort_ValuesSwitched(object sender, ValuesSwitchedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                PictureBoxHelperClass.E = e;
                pictureBox.Refresh();
            });
        }

        private void AsyncQuickSort_FinishedSorting(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate {
                buttonGenerate.Enabled = true;
                buttonSort.Enabled = true;
                PictureBoxHelperClass.BigUpdate = true;
            });
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input.Trim(' ') != string.Empty)
            {
                e.Cancel = !(uint.TryParse(input, out uint a));
                if (a > 1000)
                {
                    MessageBox.Show("Enter only numbers smaller than 1000", "Restriction");
                    e.Cancel = true;
                }
            }
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            string input = textBoxElements.Text;
            if (input.Trim(' ') == string.Empty)
                return;

            array = new double[Convert.ToInt32(input)];
            Random random = new Random();
            
            for (int i = 0; i < array.Length; i++)
                array[i] = random.Next(0, int.MaxValue);

            PictureBoxHelperClass.Array = array;
            PictureBoxHelperClass.Pivots = new int[0];
            pictureBox.Refresh();
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            if (array.Length == 0)
                return;

            buttonGenerate.Enabled = false;
            buttonSort.Enabled = false;
            PictureBoxHelperClass.BigUpdate = false;

            switch (sortingAlgorithm)
            {
                case CurrentSortingAlgorithm.QuickSort:
                     Task.Run(() => AsyncQuickSort.Sort1Thread(array, 0, array.Length - 1));
                    break;

                case CurrentSortingAlgorithm.QuickSort2Threads:
                     Task.Run(() => AsyncQuickSort.Sort2Threads(array, 0, array.Length - 1));
                    break;

                case CurrentSortingAlgorithm.QuickSort4Threads:
                     Task.Run(() => AsyncQuickSort.Sort4Threads(array, 0, array.Length - 1));
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
            buttonGenerate.Enabled = true;
            buttonSort.Enabled = true;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBoxHelperClass.DecidePictureBoxUpdate()();
        }
    }
}
