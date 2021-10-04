using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Multithreading_Sortieralgorithmen
{
    static class QuickSort
    {
        public static event EventHandler<ValuesSwitchedEventArgs> ValuesSwitched;

        public static bool Stop { get; set; } = false;
        public static void Sort(double[] array, int left, int right)
        {
            if (Stop)
                return;

            if (left < right)
            {
                int pivot = Partition(array, left, right);

                if (pivot > 1)
                {
                    Sort(array, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    Sort(array, pivot + 1, right);
                }
            }
        }

        public static int Partition(double[] array, int left, int right)
        {
            double pivot = array[left];

            while (true)
            {
                if (Stop)
                    return 0;

                while (array[left] < pivot)
                    left++;

                while (array[right] > pivot)
                    right--;

                if (left < right)
                {
                    double temp = array[left];
                    array[left] = array[right];
                    array[right] = temp;

                    ValuesSwitched?.Invoke(null, new ValuesSwitchedEventArgs(left, right, array[left], array[right]));


                    if (array[left] == array[right])
                        left++;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}
