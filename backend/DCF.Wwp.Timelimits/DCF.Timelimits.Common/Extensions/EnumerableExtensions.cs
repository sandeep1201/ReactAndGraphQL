using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EnsureThat;

namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> set)
        {
            return set ?? Enumerable.Empty<T>();
        }

        [DebuggerStepThrough]
        public static IList<T> ForEach<T>(this IEnumerable<T> source, Action<T> valueAction)
        {
            Ensure.That(source, nameof(source)).IsNotNull();

            var list = (source as IList<T>) ?? source.ToList();
            foreach (var value in list)
            {
                valueAction(value);
            }

            return list;
        }

        [DebuggerStepThrough]
        public static T SingletonOrDefault<T>(this IEnumerable<T> source,
            T defaultValue = default(T))
        {
            return source.Count() == 1 ? source.ElementAt(0) : defaultValue;
        }

        [DebuggerStepThrough]
        public static void ForEachValue<TKey, TElement>(this ILookup<TKey, TElement> source,
            Action<TElement> action)
        {
            foreach (var value in source.Values())
            {
                action(value);
            }
        }

        [DebuggerStepThrough]
        public static void ForEachValue<TKey, TElement>(this IDictionary<TKey, TElement> source,
            Action<TElement> action)
        {
            foreach (var value in source.Values)
            {
                action(value);
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<TElement> Values<TKey, TElement>(this ILookup<TKey, TElement> source)
        {
            return source.SelectMany(v => v);
        }

        [DebuggerStepThrough]
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        [DebuggerStepThrough]
        public static Boolean IsEmpty<T>(this IEnumerable<T> source)
        {
            Ensure.That(source,nameof(source)).IsNotNull();
            return !source.Any();
        }

        [DebuggerStepThrough]
        public static Boolean IsSingletonSet<T>(this IEnumerable<T> source)
        {
            Ensure.That(source,nameof(source)).IsNotNull();
            return source.Count() == 1;
        }

        [DebuggerStepThrough]
        public static IEnumerable<TKey> WhereDuplicated<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> propertySelector)
        {
            Ensure.That(source,nameof(source)).IsNotNull();
            Ensure.That(propertySelector, nameof(propertySelector)).IsNotNull();

            return source.GroupBy(propertySelector)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);
        }

        [DebuggerStepThrough]
        public static TSource GetMin<TSource, U>(this IEnumerable<TSource> data, Func<TSource, U> f) where U : IComparable
        {
            var dataList = data as IList<TSource> ?? data.ToList();
            return dataList.Count > 1 ? dataList.Aggregate((el1, el2) => f(el1).CompareTo(f(el2)) > 0 ? el2 : el1) : dataList.FirstOrDefault();
        }

        [DebuggerStepThrough]
        public static TSource GetMax<TSource, U>(this IEnumerable<TSource> data, Func<TSource, U> f) where U : IComparable
        {
            var dataList = data as IList<TSource> ?? data.ToList();
            return dataList.Count > 1 ? data.Aggregate((el1, el2) => f(el1).CompareTo(f(el2)) > 0 ? el1 : el2) : dataList.FirstOrDefault();
        }
    }
}