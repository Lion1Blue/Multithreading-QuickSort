using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multithreading_Sortieralgorithmen
{
    class AsyncQuickSortWrapper
    {
        public double[] Array { private get; set; }
        public int Left { private get; set; }
        public int Right { private get; set; }

        public void Sort()
        {
            QuickSort.Sort(Array, Left, Right);
        }
    }
}
