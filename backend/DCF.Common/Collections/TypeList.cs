using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Core.Collections;

namespace DCF.Common.Collections
{
        /// <summary>
        /// A shortcut for <see cref="TypeList{TBaseType}"/> to use object as base type.
        /// </summary>
        public class TypeList : TypeList<Object>, ITypeList
        {
        }

        /// <summary>
        /// Extends <see cref="List{Type}"/> to add restriction a specific base type.
        /// </summary>
        /// <typeparam name="TBaseType">Base Type of <see cref="Type"/>s in this list</typeparam>
        public class TypeList<TBaseType> : ITypeList<TBaseType>
        {
            /// <summary>
            /// Gets the count.
            /// </summary>
            /// <value>The count.</value>
            public Int32 Count { get { return _typeList.Count; } }

            /// <summary>
            /// Gets a value indicating whether this instance is read only.
            /// </summary>
            /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
            public Boolean IsReadOnly { get { return false; } }

            /// <summary>
            /// Gets or sets the <see cref="Type"/> at the specified index.
            /// </summary>
            /// <param name="index">Index.</param>
            public Type this[Int32 index]
            {
                get { return _typeList[index]; }
                set
                {
                    CheckType(value);
                    _typeList[index] = value;
                }
            }

            private readonly List<Type> _typeList;

            /// <summary>
            /// Creates a new <see cref="TypeList{T}"/> object.
            /// </summary>
            public TypeList()
            {
                _typeList = new List<Type>();
            }

            /// <inheritdoc/>
            public void Add<T>() where T : TBaseType
            {
                _typeList.Add(typeof(T));
            }

            /// <inheritdoc/>
            public void Add(Type item)
            {
                CheckType(item);
                _typeList.Add(item);
            }

            /// <inheritdoc/>
            public void Insert(Int32 index, Type item)
            {
                _typeList.Insert(index, item);
            }

            /// <inheritdoc/>
            public Int32 IndexOf(Type item)
            {
                return _typeList.IndexOf(item);
            }

            /// <inheritdoc/>
            public Boolean Contains<T>() where T : TBaseType
            {
                return Contains(typeof(T));
            }

            /// <inheritdoc/>
            public Boolean Contains(Type item)
            {
                return _typeList.Contains(item);
            }

            /// <inheritdoc/>
            public void Remove<T>() where T : TBaseType
            {
                _typeList.Remove(typeof(T));
            }

            /// <inheritdoc/>
            public Boolean Remove(Type item)
            {
                return _typeList.Remove(item);
            }

            /// <inheritdoc/>
            public void RemoveAt(Int32 index)
            {
                _typeList.RemoveAt(index);
            }

            /// <inheritdoc/>
            public void Clear()
            {
                _typeList.Clear();
            }

            /// <inheritdoc/>
            public void CopyTo(Type[] array, Int32 arrayIndex)
            {
                _typeList.CopyTo(array, arrayIndex);
            }

            /// <inheritdoc/>
            public IEnumerator<Type> GetEnumerator()
            {
                return _typeList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _typeList.GetEnumerator();
            }

            private static void CheckType(Type item)
            {
                if (!typeof(TBaseType).IsAssignableFrom(item))
                {
                    throw new ArgumentException("Given item is not type of " + typeof(TBaseType).AssemblyQualifiedName, "item");
                }
            }
        }
    }
