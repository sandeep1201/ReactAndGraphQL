using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Common.Extensions
{
    public static class LookupExtensions
    {
        public static ILookup<TKey, TValue> ConcatLookups<TKey, TValue>(
           this ILookup<TKey, TValue> first, ILookup<TKey, TValue> second, IEqualityComparer<TKey> keyComparer = null)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));

            //return ConcatImpl(first, second) .ToLookup(keyComparer);
            return ConcatImpl(first, second).Where(x => !Equals(x.Value, default(IEnumerable<TValue>)))
                .SelectMany(kv => kv.Value, (kv, v) => new { kv.Key, Value = v })
                .ToLookup(x => x.Key, x => x.Value, keyComparer);
        }

        private static IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> ConcatImpl<TKey, TValue>(
            IEnumerable<IGrouping<TKey, TValue>> first, ILookup<TKey, TValue> second)
        {
            var secondKeys = second.Select(x=>x.Key).ToList();
            foreach (var grouping in first)
            {
                secondKeys.Remove(grouping.Key);
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(grouping.Key, grouping.Concat(second[grouping.Key]));
            }

            foreach (var newKey in secondKeys)
            {
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(newKey, second[newKey]);
            }
        }
    }
}
