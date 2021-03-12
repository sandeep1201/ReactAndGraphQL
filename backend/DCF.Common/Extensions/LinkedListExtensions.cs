using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Common.Extensions
{
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Should peform better for mostly sorted lists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        

        public static void InsertionSort<T>(this LinkedList<T> list) where T : IComparable<T>
        {
            var tempList = list.ToList();
            tempList.InsertionSort();
            list.Clear();
            var currentNode = list.AddFirst(tempList.FirstOrDefault());
            foreach (var sortedNode in tempList.Skip(1))
            {
                currentNode = list.AddAfter(currentNode, sortedNode);
            }
        }
    }

}