using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    /// <summary>
    /// Describes an enumerable which elements are grouped source elements.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TSource">The source type.</typeparam>
    internal class GroupedEnumerable<TKey, TSource> : IEnumerable<IGrouping<TKey, TSource>>
    {
        /// <summary>
        /// The source enumerable.
        /// </summary>
        private IEnumerable<TSource> source;

        /// <summary>
        /// The key selecto function.
        /// </summary>
        private Func<TSource, TKey> keySelector;

        /// <summary>
        /// Creates a new enumerable.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        public GroupedEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            this.source = source;
            this.keySelector = keySelector;
        }

        /// <summary>
        /// Returns the iterator of the enumerable.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IGrouping<TKey, TSource>> GetEnumerator()
        {
            return new GroupedIterator<TKey, TSource>(source, keySelector);
        }

        /// <summary>
        /// Returns the iterator of the enumerable as object.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Describes an iterator of the class above.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TSource">The source type.</typeparam>
        private class GroupedIterator<TKey, TSource> : IEnumerator<IGrouping<TKey, TSource>>
        {
            /// <summary>
            /// Dictionary to contain the key-group pairs.
            /// </summary>
            private Dictionary<TKey, GroupEnumerable<TKey, TSource>> dictionary;

            /// <summary>
            /// The enumerator of the dictionary.
            /// </summary>
            private IEnumerator<KeyValuePair<TKey, GroupEnumerable<TKey, TSource>>> enumerator;

            /// <summary>
            /// The current group of the enuemrable.
            /// </summary>
            public IGrouping<TKey, TSource> Current
            {
                get
                {
                    return this.enumerator.Current.Value;
                }
            }

            /// <summary>
            /// The current group of the enuemrable as object.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.enumerator.Current.Value;
                }
            }

            /// <summary>
            /// Creates a new iterator.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="keySelector">the key selector function.</param>
            public GroupedIterator(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            {
                this.dictionary = new Dictionary<TKey, GroupEnumerable<TKey, TSource>>();
                foreach (var item in source)
                {
                    var key = keySelector(item);
                    if (this.dictionary.ContainsKey(key))
                    {
                        this.dictionary[key].Add(item);
                    }
                    else
                    {
                        var temporary = new GroupEnumerable<TKey, TSource>(key);
                        temporary.Add(item);
                        this.dictionary.Add(key, temporary);
                    }
                }
                this.enumerator = this.dictionary.GetEnumerator();
            }

            /// <summary>
            /// Moves the iterator to next position.
            /// </summary>
            /// <returns>return true if the iterator was moved successful.</returns>
            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            /// <summary>
            /// Resets the iterator.
            /// </summary>
            public void Reset()
            {
                this.enumerator.Reset();
            }

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            // ~GroupedIterator() {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            //   Dispose(false);
            // }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }
            #endregion

        }
    }

    /// <summary>
    /// Describes a group enumerable.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    internal class GroupEnumerable<TKey, TValue> : IGrouping<TKey, TValue>
    {
        /// <summary>
        /// The key of the group.
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// The list of values.
        /// </summary>
        private List<TValue> values;

        /// <summary>
        /// Creates an enumerable.
        /// </summary>
        /// <param name="key">The key.</param>
        public GroupEnumerable(TKey key)
        {
            this.Key = key;
            this.values = new List<TValue>();
        }

        /// <summary>
        /// Adds a new element into the list.
        /// </summary>
        /// <param name="value"></param>
        public void Add(TValue value)
        {
            this.values.Add(value);
        }

        /// <summary>
        /// Returns the iterator of the enumerable.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        /// <summary>
        /// Returns the iterator of the enumerable as object.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.values.GetEnumerator();
        }
    }

}
