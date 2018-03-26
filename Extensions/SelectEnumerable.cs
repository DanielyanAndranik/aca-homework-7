using System;
using System.Collections;
using System.Collections.Generic;

namespace Extensions
{
    /// <summary>
    /// Describes an enumerable type which elements are elements of the source performing the change on them according the selector.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TResult">The result type.</typeparam>
    internal class SelectEnumerable<TSource, TResult> : IEnumerable<TResult>
    {
        /// <summary>
        /// The source enumerable.
        /// </summary>
        private IEnumerable<TSource> source;

        /// <summary>
        /// The selector function.
        /// </summary>
        private Func<TSource, TResult> selector;
        
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector</param>
        public SelectEnumerable(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        /// <summary>
        /// Method that returns the iterator of this class.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TResult> GetEnumerator()
        {
            return new SelectIterator<TSource, TResult>(this.source, this.selector);
        }

        /// <summary>
        /// Method that returns the iterator of this class as object.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Describes an iterator of class above.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        private class SelectIterator<TSource, TResult> : IEnumerator<TResult>
        {
            /// <summary>
            /// The iterator of source enumerable.
            /// </summary>
            private IEnumerator<TSource> enumerator;

            /// <summary>
            /// The selector function.
            /// </summary>
            private Func<TSource, TResult> selector;

            /// <summary>
            /// The current element of the enumerable.
            /// </summary>
            public TResult Current
            {
                // Returns the current element of the source which has change its value according the selector function.
                get
                {
                    return this.selector(this.enumerator.Current);
                }
            }

            /// <summary>
            /// The current element of the numerable as object.
            /// </summary>
            object IEnumerator.Current
            {
                // Returns the current element as object of the source which has change its value according the selector function.
                get
                {
                    return this.selector(this.enumerator.Current);
                }
            }

            /// <summary>
            /// Creates a new iterator.
            /// </summary>
            /// <param name="source">The source enumerable.</param>
            /// <param name="selector">The selector function.</param>
            public SelectIterator(IEnumerable<TSource> source, Func<TSource, TResult> selector)
            {
                this.enumerator = source.GetEnumerator();
                this.selector = selector;
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
            /// Reset the iterator.
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
            // ~SelectIterator() {
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
