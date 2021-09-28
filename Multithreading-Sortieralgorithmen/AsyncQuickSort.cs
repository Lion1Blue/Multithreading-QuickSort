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

        public static void Sort2Threads(double[] array, int left, int right)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int pivot = QuickSort.Partition(array, left, right);

            AsyncQuickSortWrapper wrapper1 = new AsyncQuickSortWrapper();
            wrapper1.Array = array;
            wrapper1.Left = left;
            wrapper1.Right = pivot - 1;

            AsyncQuickSortWrapper wrapper2 = new AsyncQuickSortWrapper();
            wrapper2.Array = array;
            wrapper2.Left = pivot + 1;
            wrapper2.Right = right;

            Thread thread1 = new Thread(wrapper1.Sort);
            Thread thread2 = new Thread(wrapper2.Sort);

            if (pivot > 1)
                thread1.Start();
            
            if (pivot + 1 < right)
                thread2.Start();

            while (thread1.ThreadState != System.Threading.ThreadState.Stopped && thread2.ThreadState != System.Threading.ThreadState.Stopped)
            {
                
            }

            stopwatch.Stop();

            Console.WriteLine($"[Async2]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void Sort4Threads(double[] array, int left, int right)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int pivot1 = QuickSort.Partition(array, left, right);

            int pivot2 = QuickSort.Partition(array, left, pivot1 - 1);

            int pivot3 = QuickSort.Partition(array, pivot1 + 1, right);

            AsyncQuickSortWrapper wrapper1 = new AsyncQuickSortWrapper();
            wrapper1.Array = array;
            wrapper1.Left = left;
            wrapper1.Right = pivot2 - 1;

            AsyncQuickSortWrapper wrapper2 = new AsyncQuickSortWrapper();
            wrapper2.Array = array;
            wrapper2.Left = pivot2 + 1;
            wrapper2.Right = pivot1 - 1;

            AsyncQuickSortWrapper wrapper3 = new AsyncQuickSortWrapper();
            wrapper3.Array = array;
            wrapper3.Left = pivot1 + 1;
            wrapper3.Right = pivot3 - 1;

            AsyncQuickSortWrapper wrapper4 = new AsyncQuickSortWrapper();
            wrapper4.Array = array;
            wrapper4.Left = pivot3 + 1;
            wrapper4.Right = right;

            Thread thread1 = new Thread(wrapper1.Sort);
            Thread thread2 = new Thread(wrapper2.Sort);
            Thread thread3 = new Thread(wrapper3.Sort);
            Thread thread4 = new Thread(wrapper4.Sort);

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();

            while (thread1.ThreadState != System.Threading.ThreadState.Stopped &&
                   thread2.ThreadState != System.Threading.ThreadState.Stopped &&
                   thread3.ThreadState != System.Threading.ThreadState.Stopped &&
                   thread4.ThreadState != System.Threading.ThreadState.Stopped)
            {

            }

            stopwatch.Stop();

            Console.WriteLine($"[Async4]Sorted in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
