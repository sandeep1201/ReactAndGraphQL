using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DCF.Common.Extensions
{
    public static class ListExtensions
    {
        public static void InsertionSort<T>(this IList<T> list) where T : IComparable<T>
        {
            Int32 i;

            for (i = 1; i < list.Count; i++)
            {
                T value = list[i];
                var j = i - 1;
                while ((j >= 0) && (list[j].CompareTo(value) > 0))
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = value;
            }
        }

        public static void InsertionSort<T, TK>(this IList<T> list, Expression<Func<T, TK>> keySelector) where TK:IComparable<TK>
        {
            Int32 i;
            var keySelectorFunc = keySelector.Compile();
            for (i = 1; i < list.Count; i++)
            {
                T value = list[i];
                var j = i - 1;
                while ((j >= 0) && (keySelectorFunc(list[j]).CompareTo(keySelectorFunc(value)) > 0))
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = value;
            }
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (items == null) throw new ArgumentNullException("items");

            if (list is List<T>)
            {
                ((List<T>) list).AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }
    }
}