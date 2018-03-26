using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Describes an enumerable type which is filtred source.
    /// </summary>
    /// <typeparam name="TSource">Tuye source type.</typeparam>
    public class WhereEnumerable<TSource> : IEnumerable<TSource>
    {
        /// <summary>
        /// The source enumerable.
        /// </summary>
        private IEnumerable<TSource> source;

        /// <summary>
        /// The predicate.
        /// </summary>
        private Func<TSource, bool> predicate;

        /// <summary>
        /// Creates a new enumerable.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate.</param>
        public WhereEnumerable(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        /// <summary>
        /// Method that returns the iterator of this class.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TSource> GetEnumerator()
        {
            return new WhereIterator<TSource>(this.source, this.predicate);
        }

        /// <summary>
        /// Method that returns the iterator of this class as object.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class WhereIterator<TSource> : IEnumerator<TSource>
        {
            /// <summary>
            /// The source enumerator.
            /// </summary>
            private IEnumerator<TSource> enumerator;

            /// <summary>
            /// The predicate.
            /// </summary>
            private Func<TSource, bool> predicate;

            /// <summary>
            /// The current element.
            /// </summary>
            public TSource Current
            {
                get
                {
                    return this.enumerator.Current;
                }
            }

            /// <summary>
            /// The current element as object.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.enumerator.Current;
                }
            }

            /// <summary>
            /// Creates a new iterator.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="predicate">The predicate.</param>
            public WhereIterator(IEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                this.enumerator = source.GetEnumerator();
                this.predicate = predicate;
            }

            /// <summary>
            /// Moves the iterator to next matching element.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if(this.predicate(this.enumerator.Current))
                {
                    return true;
                }
                while(this.enumerator.MoveNext())
                {
                    if (this.predicate(this.enumerator.Current))
                    {
                        return true;
                    }
                }
                return false;
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
            // ~WhereIterator() {
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
