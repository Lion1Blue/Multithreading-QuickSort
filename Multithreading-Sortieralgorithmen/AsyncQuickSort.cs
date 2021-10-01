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
        static List<Thread> threads = new List<Thread>();
        static Color[] colors = new Color[4] { Color.Orange, Color.Red, Color.Navy, Color.DarkOliveGreen };
        public static event EventHandler FinishedSorting;

        public static void Sort1Thread(double[] array, int left, int right)
        {
            threads.Clear();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Wrapper Class to call a method on a different Thread with Method Parameters
            AsyncQuickSortWrapper wrapper = new AsyncQuickSortWrapper(array, left, right);

            Thread thread = new Thread(wrapper.Sort);
            threads.Add(thread);

            thread.Start();
            thread.Join();

            stopwatch.Stop();
            PictureBoxHelperClass.BigUpdate = true;
            FinishedSorting?.Invoke(null, new EventArgs());

            Console.WriteLine($"[Normal]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void Sort2Threads(double[] array, int left, int right)
        {
            threads.Clear();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int[] pivots = new int[3];
            pivots[0] = left;
            pivots[1] = QuickSort.Partition(array, left, right);
            pivots[2] = right;

            for (int i = 0; i < 2; i++)
            {
                AsyncQuickSortWrapper wrapper = new AsyncQuickSortWrapper(array, pivots[i], pivots[i + 1]);

                Thread thread = new Thread(wrapper.Sort);
                threads.Add(thread);

                thread.Start();
            }

            foreach (Thread t in threads)
                t.Join();

            stopwatch.Stop();
            PictureBoxHelperClass.BigUpdate = true;
            FinishedSorting?.Invoke(null, new EventArgs());

            Console.WriteLine($"[Async2]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void Sort4Threads(double[] array, int left, int right)
        {
            threads.Clear();
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

            for (int i = 0; i < 4; i++)
            {
                AsyncQuickSortWrapper wrapper = new AsyncQuickSortWrapper(array, pivots[i], pivots[i + 1]);

                Thread thread = new Thread(wrapper.Sort);
                threads.Add(thread);

                thread.Start();
            }

            foreach (Thread t in threads)
                t.Join();

            stopwatch.Stop();
            PictureBoxHelperClass.BigUpdate = true;
            FinishedSorting?.Invoke(null, new EventArgs());

            Console.WriteLine($"[Async4]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }

        public static Color ThreadToColor(Thread thread)
        {
            for (int i = 0; i < threads.Count; i++)
                if (thread == threads[i])
                    return colors[i];

            return Color.Black;

        }
    }
}
