using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Describes an enumerable type, which elements are ordered.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    class OrderedEnumerable<TSource, TKey> : IOrderedEnumerable<TSource> where TKey: IComparable<TKey>
    {
        /// <summary>
        /// The source enumerable.
        /// </summary>
        private IEnumerable<TSource> source;

        /// <summary>
        /// The key selector.
        /// </summary>
        private Func<TSource, TKey> keySelector;

        /// <summary>
        /// Cantains the sort direction.
        /// </summary>
        private static bool descending;

        /// <summary>
        /// Creates a new Ordered Enumerable.
        /// </summary>
        /// <param name="source">The source enumerable.</param>
        /// <param name="keySelector">the key selector function.</param>
        /// <param name="desc">The sort direction</param>
        public OrderedEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool desc)
        {
            this.source = source;
            this.keySelector = keySelector;
            descending = desc;
        }

        /// <summary>
        /// Nobody needs this. :)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <param name="descending"></param>
        /// <returns></returns>
        public IOrderedEnumerable<TSource> CreateOrderedEnumerable<T>(Func<TSource, T> keySelector, IComparer<T> comparer, bool descending)
        {
            return this;
        }

        /// <summary>
        /// Return the enumerator of this.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TSource> GetEnumerator()
        {
            List<KeyValuePair<TKey, TSource>> unordered = new List<KeyValuePair<TKey, TSource>>();
            foreach(var item in source)
            {
                unordered.Add(new KeyValuePair<TKey, TSource>(this.keySelector(item), item));
            }

            return new OrderIterator<TKey, TSource>(Sort(unordered));
        }

        /// <summary>
        /// Returns the enumerator of this as object.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Sorts given List.
        /// </summary>
        /// <param name="unordered">The unordered List.</param>
        /// <returns>Returns ordered one.</returns>
        private static List<KeyValuePair<TKey, TSource>> Sort(List<KeyValuePair<TKey, TSource>> unordered)
        {
            QuickSort(unordered, 0, unordered.Count() - 1);
            return unordered;
        }
        
        /// <summary>
        /// Quick sort algorihm.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="left">The left index.</param>
        /// <param name="right">The right index.</param>
        private static void QuickSort(List<KeyValuePair<TKey, TSource>> list, int left, int right)
        {
            if(left < right)
            {
                int partition = PartitionIndex(list, left, right);
                QuickSort(list, left, partition - 1);
                QuickSort(list, partition + 1, right);
            }
        }

        /// <summary>
        /// Finds the pivot of the subList.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="left">The left index.</param>
        /// <param name="right">the right index.</param>
        /// <returns>Return the partition index.</returns>
        private static int PartitionIndex(List<KeyValuePair<TKey, TSource>> list, int left, int right)
        {
            KeyValuePair<TKey, TSource> pivot = list[right];
            int index = left;
            KeyValuePair<TKey, TSource> temporary;

            if (!descending)
            {
                for (int i = left; i < right; i++)
                {
                    if (list[i].Key.CompareTo(pivot.Key) <= 0)
                    {
                        temporary = list[i];
                        list[i] = list[index];
                        list[index] = temporary;
                        index++;
                    }
                }
            }
            else
            {
                for (int i = left; i < right; i++)
                {
                    if (list[i].Key.CompareTo(pivot.Key) >= 0)
                    {
                        temporary = list[i];
                        list[i] = list[index];
                        list[index] = temporary;
                        index++;
                    }
                }
            }
            

            temporary = list[right];
            list[right] = list[index];
            list[index] = temporary;

            return index;
        }

        /// <summary>
        /// Describes an iterator of enumerable above.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TSource">The source type.</typeparam>
        private class OrderIterator<TKey, TSource> : IEnumerator<TSource>
        {
            /// <summary>
            /// The enumerator of source enumerable.
            /// </summary>
            private IEnumerator<KeyValuePair<TKey, TSource>> enumerator;

            /// <summary>
            /// The current element of the enumerable.
            /// </summary>
            public TSource Current
            {
                get
                {
                    return this.enumerator.Current.Value;
                }
            }

            /// <summary>
            /// The current element of the enumerable as object.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.enumerator.Current.Value;
                }
            }

            /// <summary>
            /// Creates a new Iterator.
            /// </summary>
            /// <param name="source">he source enumerable.</param>
            public OrderIterator(IEnumerable<KeyValuePair<TKey, TSource>> source)
            {
                this.enumerator = source.GetEnumerator();
            }

            /// <summary>
            /// Moves the iterator to next position. 
            /// </summary>
            /// <returns>returns true if the process of moving has done successful, else returns false.</returns>
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
            // ~OrderIterator() {
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

    
}
