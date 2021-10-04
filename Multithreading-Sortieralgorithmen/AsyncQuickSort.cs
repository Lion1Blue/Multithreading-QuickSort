using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

namespace Multithreading_Sortieralgorithmen
{
    class AsyncQuickSort
    {
        public static event EventHandler FinishedSorting;

        public static void Sort1Thread(double[] array, int left, int right)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            PictureBoxHelperClass.Pivots = new int[] { left, right };
            PictureBoxHelperClass.BigUpdate = false;

            //Wrapper Class to call a method on a different Thread with Method Parameters
            AsyncQuickSortWrapper wrapper = new AsyncQuickSortWrapper(array, left, right);
            Thread thread = new Thread(wrapper.Sort);

            thread.Start();
            //waiting for thread to join to main thread
            thread.Join();

            stopwatch.Stop();
            PictureBoxHelperClass.BigUpdate = true;
            FinishedSorting?.Invoke(null, new EventArgs());

            Console.WriteLine($"[Normal]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void Sort2Threads(double[] array, int left, int right)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int[] pivots = new int[3];
            pivots[0] = left;
            pivots[1] = QuickSort.Partition(array, left, right);
            pivots[2] = right;

            PictureBoxHelperClass.Pivots = pivots;
            PictureBoxHelperClass.BigUpdate = false;
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < 2; i++)
            {
                AsyncQuickSortWrapper wrapper = new AsyncQuickSortWrapper(array, pivots[i], pivots[i + 1]);

                Thread thread = new Thread(wrapper.Sort);
                threads.Add(thread);

                thread.Start();
            }

            //waiting for all threads to join to main thread
            foreach (Thread t in threads)
                t.Join();

            stopwatch.Stop();
            PictureBoxHelperClass.BigUpdate = true;
            FinishedSorting?.Invoke(null, new EventArgs());

            Console.WriteLine($"[Async2]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void Sort4Threads(double[] array, int left, int right)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int[] pivots = new int[5];
            pivots[0] = left;
            pivots[2] = QuickSort.Partition(array, left, right);
            if (pivots[2] >= 1)
                pivots[1] = QuickSort.Partition(array, left, pivots[2] - 1);
            else
                pivots[1] = 0;
            pivots[3] = QuickSort.Partition(array, pivots[2] + 1, right);
            pivots[4] = right;

            PictureBoxHelperClass.Pivots = pivots;
            PictureBoxHelperClass.BigUpdate = false;
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < 4; i++)
            {
                AsyncQuickSortWrapper wrapper = new AsyncQuickSortWrapper(array, pivots[i], pivots[i + 1]);

                Thread thread = new Thread(wrapper.Sort);
                threads.Add(thread);

                thread.Start();
            }

            //waiting for all threads to join to main thread
            foreach (Thread t in threads)
                t.Join();

            stopwatch.Stop();
            PictureBoxHelperClass.BigUpdate = true;
            FinishedSorting?.Invoke(null, new EventArgs());

            Console.WriteLine($"[Async4]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
