using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Multithreading_Sortieralgorithmen
{
    class AsyncQuickSort
    {
        public static void Sort(double[] array)
        {
            Sort2Threads(array, 0, array.Length - 1);
        }

        public static void Sort1Thread(double[] array, int left, int right)
        {
            AsyncQuickSortWrapper wrapper = new AsyncQuickSortWrapper();
            wrapper.Array = array;
            wrapper.Left = left;
            wrapper.Right = right;

            Thread thread = new Thread(wrapper.Sort);

            thread.Start();
        }

        public static void Sort2Threads(double[] array, int left, int right)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Thread> threads = new List<Thread>();

            int[] pivots = new int[3];
            pivots[0] = left;
            pivots[1] = QuickSort.Partition(array, left, right);
            pivots[2] = right;

            for (int i = 0; i < 2; i++)
            {
                AsyncQuickSortWrapper wrapper = new AsyncQuickSortWrapper();
                wrapper.Array = array;
                wrapper.Left = pivots[i];
                wrapper.Right = pivots[i + 1];

                Thread thread = new Thread(wrapper.Sort);
                threads.Add(thread);

                thread.Start();
            }

            //foreach (Thread t in threads)
            //    t.Join();

            stopwatch.Stop();

            Console.WriteLine($"[Async2]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void Sort4Threads(double[] array, int left, int right)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<Thread> threads = new List<Thread>();

            int[] pivots = new int[5];
            pivots[0] = left;
            pivots[2] = QuickSort.Partition(array, left, right);
            pivots[1] = QuickSort.Partition(array, left, pivots[2] - 1);
            pivots[3] = QuickSort.Partition(array, pivots[2] + 1, right);
            pivots[4] = right;

            for (int i = 0; i < 4; i++)
            {
                AsyncQuickSortWrapper wrapper = new AsyncQuickSortWrapper();
                wrapper.Array = array;
                wrapper.Left = pivots[i];
                wrapper.Right = pivots[i + 1];

                Thread thread = new Thread(wrapper.Sort);
                threads.Add(thread);

                thread.Start();
            }

            foreach (Thread t in threads)
                t.Join();

            stopwatch.Stop();

            Console.WriteLine($"[Async4]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
