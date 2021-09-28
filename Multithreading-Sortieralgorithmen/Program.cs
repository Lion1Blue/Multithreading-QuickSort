using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Multithreading_Sortieralgorithmen
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            double[] array1 = new double[10000000];
            double maxValue = 10d;

            Random random = new Random();

            for (int i = 0; i < array1.Length; i++)
            {
                array1[i] = (double)random.Next(0, int.MaxValue);
            }

            double[] array2 = new double[array1.Length];
            Array.Copy(array1, array2, array1.Length);

            double[] array3 = new double[array1.Length];
            Array.Copy(array1, array3, array1.Length);

            double[] array4 = new double[array1.Length];
            Array.Copy(array1, array4, array1.Length);


            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            Array.Sort(array1);
            stopwatch1.Stop();

            Console.WriteLine($"[ArraySort]Sorted in {stopwatch1.ElapsedMilliseconds} ms");


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            QuickSort.Sort(array2, 0, array2.Length - 1);
            stopwatch.Stop();

            Console.WriteLine($"[Normal]Sorted in {stopwatch.ElapsedMilliseconds} ms");


            AsyncQuickSort.Sort2Threads(array3, 0, array1.Length - 1);
            AsyncQuickSort.Sort4Threads(array4, 0, array3.Length - 1);
        }
    }
}
