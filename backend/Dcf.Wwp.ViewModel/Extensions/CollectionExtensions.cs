using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Delegates;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Evaluates the given enumerable and returns back a list of objects that are non-
        /// empty based upon the IIsEmpty interface.  Note if the collection is null it will
        /// still return a valid list, albeit empty.
        /// </summary>
        /// <typeparam name="T">Type must implmement IIsEmtpty interface</typeparam>
        /// <param name="items">Collection of items</param>
        /// <returns>List</returns>
        public static List<T> WithoutEmpties<T>(this IEnumerable<T> items) where T : IIsEmpty
        {
            //var noEmpties = new List<T>();

            //if (items != null)
            //{
            //    foreach (var item in items)
            //    {
            //        if (!item.IsEmpty())
            //            noEmpties.Add(item);
            //    }
            //}

            // return (noEmpties);

            return (items?.Where(i => !i.IsEmpty()).Select(i => i).ToList() ?? new List<T>()); // - scott v.
        }

        /// <summary>
        /// Updates an incoming collection of items by looking at the new items and checking if each of the new
        /// items are similar to an existing item.  If so, it will re-attach to the existing item by taking
        /// back over (adopting) its Id.
        /// </summary>
        /// <typeparam name="TIncoming"></typeparam>
        /// <typeparam name="TExisting"></typeparam>
        /// <param name="incoming">The collection of posted/incoming items</param>
        /// <param name="existing">The collection of existing (database) items INCLUDING soft deleted objects.</param>
        /// <param name="adoptIfSimilar">The delegate that does the similarity checking between the two objects/types and adopts the existing identity if it matches.</param>
        /// <remarks>This is meant to be used in postbacks of Contract data that needs to be matched up against
        /// Database model objects.</remarks>
        public static void UpdateNewItemsIfSimilarToExisting<TIncoming, TExisting>(this IEnumerable<TIncoming> incoming, IList<TExisting> existing, AdoptIfSimilar<TIncoming, TExisting> adoptIfSimilar)
            where TIncoming : class, IHasId, IIsNew
            where TExisting : class, IHasId
        {
            if (incoming != null)
            {
                foreach (var inc in incoming)
                {
                    if (inc.IsNew())
                    {
                        // Look if the new item is similar to an existing item and if so we need to
                        // update the new item with the same Id.
                        foreach (var exist in existing)
                        {
                            // If they are similar and have been adopted, then exit the loop.
                            if (adoptIfSimilar(inc, exist))
                            {
                                // Exit the foreach existing and go to the next incoming item.
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates the source collection and marks the items as IsDeleted if they don't exist
        /// in the supplied list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbItems">The list of items that need to be updated</param>
        /// <param name="modedIdsInUse">A list of ID's that are in use and shouldn't be marked as IsDeleted</param>
        public static void MarkUnusedItemsAsDeleted<T>(this IList<T> dbItems, List<int> modedIdsInUse) where T : class, IHasId, IIsDeleted
        {
            var dbIds       = from x in dbItems select x.Id;
            var deletedList = dbIds.Except(modedIdsInUse).ToArray();
            var t           = typeof(T);

            foreach (int n in deletedList)
            {
                var c = dbItems.First(x => x.Id == n);
                c.IsDeleted = true;

                // Now look at properties.
                var props = t.GetProperties();
                foreach (var propertyInfo in props)
                {
                    Console.WriteLine(propertyInfo.ToString());
                    // TODO: Handle nested collections (e.g. Caretakers with Children)
                }
            }
        }

        public static void MarkUnusedItemsAsDeleted2<T>(this IList<T> items, IList<int> idsInUse) where T : class, IHasId, IIsDeleted
        {
            //var allIdsInDb = dbItems.Select(i => i.Id).ToList();
            //var idsNotInDb = allIdsInDb.Except(idsInUse);

            //dbItems.Where(i => !idsInUse.Contains(i.Id)).ForEach(i => i.IsDeleted = true);

            var l = items.Where(i => !idsInUse.Contains(i.Id)).ToList();
            l.ForEach(i => i.IsDeleted = true);
        }

        // Returns empty if collection is null.
        public static IEnumerable<T> AsNotNull<T>(this IEnumerable<T> original)
        {
            return original ?? Enumerable.Empty<T>();
        }

        public static List<int> GetMinMax(this List<int> nums)
        {
            var minMaxList = new List<int>();

            if (nums == null || !nums.Any()) return minMaxList;

            var minMax = nums.Aggregate(
                                        new
                                        {
                                            min = int.MaxValue,
                                            max = int.MinValue
                                        },
                                        (accumulator, current) => new
                                                                  {
                                                                      min = Math.Min(current, accumulator.min),
                                                                      max = current > accumulator.max ? current : accumulator.max
                                                                  });

            minMaxList.Add(minMax.min);
            minMaxList.Add(minMax.max);

            return minMaxList;
        }

        public static bool IsBetween(this List<int> nums, int num, bool inclusive = false)
        {
            var numList = nums;

            if (numList == null || !numList.Any()) return false;

            numList.Sort();

            var maxIndex = numList.Count - 1;

            return inclusive ? num >= numList[1] && num <= numList[maxIndex] : num > numList[1] && num < numList[maxIndex];
        }
    }
}
