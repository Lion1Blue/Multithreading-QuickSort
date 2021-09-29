using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading_Sortieralgorithmen
{
    class QuickSort
    {
        public static void Sort(double[] array, int left, int right)
        {
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
                while (array[left] < pivot)
                    left++;

                while (array[right] > pivot)
                    right--;

                if (left < right)
                {
                    double temp = array[left];
                    array[left] = array[right];
                    array[right] = temp;

                    if (array[left] == array[right])
                        left++;
                }
                else
                {
                    return right;
                }
            }
        }

        /*
        public static void ParallelQuickSort(double[] array, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(array, left, right);

                Parallel.Invoke(
                    () => ParallelQuickSort(array, left, pivot - 1),
                    () => ParallelQuickSort(array, left, pivot - 1)
                    );
            }
        }
        */
    }
}
