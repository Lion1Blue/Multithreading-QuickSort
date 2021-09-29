using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        }

        double[] array = new double[0];

        private void QuickSort_ValuesSwitched(object sender, ValuesSwitchedEventArgs e)
        {
            Console.WriteLine($"values Changed. Index1 {e.Index1}, Index2 {e.Index2}, Value1: {e.Value1}, Value2: {e.Value2}, Value1 {array[e.Index1]}, Value2 {array[e.Index2]}");
            
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string input = ((TextBox)sender).Text.ToString();
            if (input.Trim(' ') != string.Empty)
                e.Cancel = !(int.TryParse(input, out int a));
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            array = new double[Convert.ToInt32(textBoxElements.Text)];
            Random random = new Random();
            
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(0, int.MaxValue);
            }
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            QuickSort.Stop = false;

            switch (sortingAlgorithm)
            {
                case CurrentSortingAlgorithm.QuickSort:
                    AsyncQuickSort.Sort1Thread(array, 0, array.Length - 1);
                    break;

                case CurrentSortingAlgorithm.QuickSort2Threads:
                    AsyncQuickSort.Sort2Threads(array, 0, array.Length - 1);
                    break;

                case CurrentSortingAlgorithm.QuickSort4Threads:
                    AsyncQuickSort.Sort4Threads(array, 0, array.Length - 1);
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
    }
}
