using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DCF.Timelimits.Rules.Domain
{
    /// <summary>
    ///     Dictonary that will add 
    ///     Internally we use a concurent dictonory so the methods are thread safe
    /// </summary>
    public class TimelineDictionary<T> : IDictionary<DateTime, T>, IReadOnlyDictionary<DateTime, T>
    {
        private readonly ConcurrentDictionary<String, T> _internalMap;
        private readonly String _dateKeyFormat;

        public TimelineDictionary(String dateKeyFormat = "MM-yyyy")
        {
            this._dateKeyFormat = dateKeyFormat;
            this._internalMap = new ConcurrentDictionary<String, T>();
        }

        #region Public Methods

        public Boolean TryAdd(DateTime key, T value)
        {
            var returnVal = this._internalMap.TryAdd(this.GetObjectAsTimelineKey(key), value);
            if (returnVal)
            {
                this.ItemChanged(new DictionaryItemChangedEventArgs<DateTime, T>(key, value, DictionaryOpertation.Added));
            }
            return returnVal;
        }

        public Boolean ContainsKey(DateTime key)
        {
            return this._internalMap.ContainsKey(this.GetObjectAsTimelineKey(key));

        }

        public Boolean TryRemove(DateTime key, out T value)
        {
            var returnVal = this._internalMap.TryRemove(this.GetObjectAsTimelineKey(key), out value);
            if (returnVal)
            {
                this.ItemChanged(new DictionaryItemChangedEventArgs<DateTime, T>(key, value, DictionaryOpertation.Removed));
            }
            return returnVal;
        }

        public Boolean TryGetValue(DateTime key, out T value)
        {
            return this._internalMap.TryGetValue(this.GetObjectAsTimelineKey(key), out value);
        }

        public Boolean TryUpdate(DateTime key, T newValue, T comparisonValue)
        {
            var returnVal = this._internalMap.TryUpdate(this.GetObjectAsTimelineKey(key), newValue, comparisonValue);
            if (returnVal)
            {
                this.ItemChanged(new DictionaryItemChangedEventArgs<DateTime, T>(key, newValue, DictionaryOpertation.Updated));
            }
            return returnVal;
        }

        public void Clear()
        {
            this._internalMap.Clear();
            this.ItemChanged(new DictionaryItemChangedEventArgs<DateTime, T>(DictionaryOpertation.Removed));

        }

        public T GetOrAdd(DateTime key, Func<String, T> valueFactory)
        {
            return this._internalMap.GetOrAdd(this.GetObjectAsTimelineKey(key), (arg) =>
            {
                var returnVal = valueFactory(arg);
                this.ItemChanged(new DictionaryItemChangedEventArgs<DateTime, T>(key, returnVal, DictionaryOpertation.Added));
                return returnVal;
            });
        }

        public T GetOrAdd(DateTime key, T value)
        {
            var returnVal = this._internalMap.GetOrAdd(this.GetObjectAsTimelineKey(key), value);
            // always assume it changed since we don't know..., maybe figure out a better way to do this with a real cache
            this.ItemChanged(new DictionaryItemChangedEventArgs<DateTime, T>(key, returnVal, DictionaryOpertation.Added));
            return returnVal;
        }

        public T AddOrUpdate(DateTime key, Func<String, T> addValueFactory, Func<String, T, T> updateValueFactory)
        {
            DictionaryOpertation? operation = null;
            var returnVal = this._internalMap.AddOrUpdate(this.GetObjectAsTimelineKey(key),
                (arg) => { operation = DictionaryOpertation.Added; return addValueFactory(arg); },
                (arg, val) => { operation = DictionaryOpertation.Updated; return updateValueFactory(arg, val); });
            if (operation.HasValue)
            {
                this.ItemChanged(new DictionaryItemChangedEventArgs<DateTime, T>(key, returnVal, operation.Value));
            }

            return returnVal;
        }

        public T AddOrUpdate(DateTime key, T addValue, Func<String, T, T> updateValueFactory)
        {
            DictionaryOpertation operation = DictionaryOpertation.Added;
            var returnVal = this._internalMap.AddOrUpdate(this.GetObjectAsTimelineKey(key), addValue, (arg, val) => { operation = DictionaryOpertation.Updated; return updateValueFactory(arg, val); });
            this.ItemChanged(new DictionaryItemChangedEventArgs<DateTime, T>(key, returnVal, operation));
            return returnVal;
        }

        public KeyValuePair<DateTime, T>[] ToArray()
        {
            return this.ToExternalDictionary().ToArray();
        }

        #endregion


        IEnumerator<KeyValuePair<DateTime, T>> IEnumerable<KeyValuePair<DateTime, T>>.GetEnumerator()
        {
            var newMap = this.ToExternalDictionary();
            //_internalMap.ToDictionary((kvp) => DateTime.ParseExact(kvp.Key, Timeline.DateFormat, CultureInfo.InvariantCulture), kvp => kvp.Value);
            return newMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this._internalMap).GetEnumerator();
        }

        void ICollection<KeyValuePair<DateTime, T>>.Add(KeyValuePair<DateTime, T> item)
        {
            this.TryAdd(item.Key, item.Value);
        }

        void ICollection<KeyValuePair<DateTime, T>>.Clear()
        {
            this._internalMap.Clear();
        }

        Boolean ICollection<KeyValuePair<DateTime, T>>.Contains(KeyValuePair<DateTime, T> item)
        {
            return
                this._internalMap.Contains(new KeyValuePair<String, T>(item.Key.ToString(this._dateKeyFormat),
                    item.Value));
        }

        void ICollection<KeyValuePair<DateTime, T>>.CopyTo(KeyValuePair<DateTime, T>[] array, Int32 arrayIndex)
        {
            this.ToExternalDictionary().CopyTo(array, arrayIndex);
        }

        public Boolean Remove(KeyValuePair<DateTime, T> item)
        {
            return
                ((ICollection<KeyValuePair<String, T>>)this._internalMap).Remove(
                    new KeyValuePair<String, T>(this.GetObjectAsTimelineKey(item.Key), item.Value));
        }

        public Boolean Remove(DateTime key)
        {
            T outVal = default(T);
            return this.TryRemove(key, out outVal);
        }

        public void CopyTo(Array array, Int32 index)
        {
            ((ICollection)_internalMap).CopyTo(array, index);
        }

        public Int32 Count => _internalMap.Count;

        public Object SyncRoot => ((ICollection)_internalMap).SyncRoot;

        public Boolean IsSynchronized => ((ICollection)_internalMap).IsSynchronized;

        Int32 ICollection<KeyValuePair<DateTime, T>>.Count => this._internalMap.Count;

        //public ICollection Values => ((IDictionary)_internalMap).Values;

        public Boolean IsReadOnly => ((IDictionary)_internalMap).IsReadOnly;

        public Boolean IsFixedSize => ((IDictionary)_internalMap).IsFixedSize;

        Boolean ICollection<KeyValuePair<DateTime, T>>.IsReadOnly => ((IDictionary)this._internalMap).IsReadOnly;

        void IDictionary<DateTime, T>.Add(DateTime key, T value)
        {
            ((IDictionary<String, T>)this._internalMap).Add(this.GetObjectAsTimelineKey(key), value);
        }

        Boolean IDictionary<DateTime, T>.Remove(DateTime key)
        {
            return ((IDictionary<String, T>)this._internalMap).Remove(this.GetObjectAsTimelineKey(key));
        }

        T IDictionary<DateTime, T>.this[DateTime key]
        {
            get { return this._internalMap[this.GetObjectAsTimelineKey(key)]; }
            set { this._internalMap[this.GetObjectAsTimelineKey(key)] = value; }
        }

        public ICollection<DateTime> Keys
        {
            get
            {
                return
                    this._internalMap.Keys.Select(
                        c => DateTime.ParseExact(c, this._dateKeyFormat, CultureInfo.InvariantCulture)).ToList();
            }
        }


        ICollection<T> IDictionary<DateTime, T>.Values => this._internalMap.Values;

        Boolean IReadOnlyDictionary<DateTime, T>.ContainsKey(DateTime key)
        {
            return this._internalMap.ContainsKey(this.GetObjectAsTimelineKey(key));
        }

        Boolean IReadOnlyDictionary<DateTime, T>.TryGetValue(DateTime key, out T value)
        {
            return this._internalMap.TryGetValue(this.GetObjectAsTimelineKey(key), out value);
        }

        IEnumerable<DateTime> IReadOnlyDictionary<DateTime, T>.Keys
        {
            get
            {
                return
                    this._internalMap.Keys.Select(
                        c => DateTime.ParseExact(c, this._dateKeyFormat, CultureInfo.InvariantCulture)).ToList();
            }
        }

        IEnumerable<T> IReadOnlyDictionary<DateTime, T>.Values => ((IDictionary<DateTime, T>)this).Values;

        Int32 IReadOnlyCollection<KeyValuePair<DateTime, T>>.Count => this._internalMap.Count;

        public event Action<DictionaryItemChangedEventArgs<DateTime, T>> ItemChanged = (args) => { };

        public T this[DateTime key]
        {
            get
            {
                if (this.ContainsKey(key))
                {
                    return this._internalMap[this.GetObjectAsTimelineKey(key)];
                }
                return default(T);
            }

            set
            {
                this.TryRemove(key, out value);
                this.TryAdd(key, value);
            }
        }

        private String GetObjectAsTimelineKey(Object obj)
        {
            var dateTimeObj = new DateTime();
            if (obj is DateTime)
            {
                dateTimeObj = (DateTime)obj;
            }
            else
            {
                if (obj != null)
                {
                    DateTime.TryParse(obj.ToString(), out dateTimeObj);
                }
            }
            return dateTimeObj.ToString(this._dateKeyFormat);
        }

        private IDictionary<DateTime, T> ToExternalDictionary()
        {
            return
                this._internalMap.ToDictionary(
                    kvp => DateTime.ParseExact(kvp.Key, this._dateKeyFormat, CultureInfo.InvariantCulture),
                    kvp => kvp.Value);
        }
    }

    public enum DictionaryOpertation
    {
        Added,
        Removed,
        Updated
    }

    public class DictionaryItemChangedEventArgs<T, K> : EventArgs
    {
        internal DictionaryItemChangedEventArgs(T key, K val, DictionaryOpertation operation)
        {
            this.Key = key;
            this.Val = val;
            this.Operation = operation;
        }

        public DictionaryItemChangedEventArgs(DictionaryOpertation operation)
        {
            this.Operation = operation;
        }


        public T Key { get; }
        public K Val { get; }
        public DictionaryOpertation Operation { get; }
    }
}