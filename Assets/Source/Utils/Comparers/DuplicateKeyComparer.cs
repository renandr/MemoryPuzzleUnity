using System;
using System.Collections.Generic;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// Comparer for comparing two keys, handling equality as beeing greater
    /// Use this Comparer e.g. with SortedLists or SortedDictionaries if you want to allow duplicate values
    /// 
    /// Source: http://stackoverflow.com/questions/5716423/c-sharp-sortable-collection-which-allows-duplicate-keys
    /// </summary>
    /// <typeparam name="TKey">The type to be compared</typeparam>
    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable<TKey>
    {
        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
            {
                // Handle equality as beeing greater
                return 1;
            }
            return result;
        }
    }
}