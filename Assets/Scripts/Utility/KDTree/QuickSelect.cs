using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;

namespace KDTree
{
    // Implementation of QuickSelect algorithm
    // See https://en.wikipedia.org/wiki/Quickselect
    public static class QuickSelect
    {
        static readonly Random random = new Random();

        /// <summary>
        /// Find the value of k-th smallest element in list
        /// </summary>
        public static T Select<T>(List<T> list, int k, IComparer<T> cmp)
        {
            return Select(list, 0, list.Count - 1, k, cmp);
        }    

        /// <summary>
        /// Find the value of k-th smallest element in list within the range [begin, end] inclusively
        /// </summary>
        public static T Select<T>(List<T> list, int begin, int end, int k, IComparer<T> cmp)
        {
            if (begin > end)
            {
                int tmp = begin;
                begin = end;
                end = tmp;
            }
            while (true)
            {
                if (begin == end)
                    return list[begin];
                int pivot = Choose(begin, end);
                pivot = Partition(list, begin, end, pivot, cmp);
                if (k == pivot)
                    return list[pivot];
                else if (k < pivot)
                    end = pivot - 1;
                else
                    begin = pivot + 1;
            }    
        }    

        static int Partition<T>(List<T> list, int begin, int end, int pivot, IComparer<T> cmp)
        {
            T pivotValue = list[pivot];
            Swap(list, pivot, end);
            int res = begin;
            for (int i = begin; i < end; ++ i)
                if (cmp.Compare(list[i], pivotValue) < 0)
                {
                    Swap(list, res, i);
                    res++;
                }
            Swap(list, res, end);
            return res;
        }   
        
        static void Swap<T>(List<T> list, int i, int j)
        {
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }    

        static int Choose(int begin, int end)
        {
            return begin + random.Next(end - begin + 1);
        }
    }
}
