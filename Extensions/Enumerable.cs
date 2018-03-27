using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Class which provide IEnumerable inctances with several methods.
    /// </summary>
    public static class Enumerable
    {
        /// <summary>
        /// The selector method.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector function.</param>
        /// <returns>returns the changed enumerable.</returns>
        public static IEnumerable<TResult> ExtensionSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if(source == null)
            {
                throw new NullReferenceException("Source is null");
            }
            if(selector == null)
            {
                throw new NullReferenceException("Selector is null");
            }
            return new SelectEnumerable<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// The filter method.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="predicate">The predicate function.</param>
        /// <returns>Returns the filtred enumerable.</returns>
        public static IEnumerable<TSource> ExtensionWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new NullReferenceException("Source is null");
            }
            if (selector == null)
            {
                throw new NullReferenceException("Selector is null");
            }
            return new WhereEnumerable<TSource>(source, predicate);
        }

        /// <summary>
        /// The grouper method.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <param name="keySelector">The key selector function.</param>
        /// <returns>Returns an enumerable pf groups.</returns>
        public static IEnumerable<IGrouping<TKey, TSource>> ExtensionGroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
            {
                throw new NullReferenceException("Source is null");
            }
            if (selector == null)
            {
                throw new NullReferenceException("Selector is null");
            }
            return new GroupedEnumerable<TKey, TSource>(source, keySelector);
        }

        /// <summary>
        /// Creates a List from enumerable.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <param name="source">the source enumerable.</param>
        /// <returns>Returns the list.</returns>
        public static List<TSource> ExtensionToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new NullReferenceException("Source is null");
            }
            List<TSource> list = new List<TSource>();
            foreach (var item in source)
            {
                list.Add(item);
            }
            return list;
        }

        public static IOrderedEnumerable<TSource> ExtensionOrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending) where TKey : IComparable<TKey>
        {
            if (source == null)
            {
                throw new NullReferenceException("Source is null");
            }
            if (selector == null)
            {
                throw new NullReferenceException("Selector is null");
            }
            return new OrderedEnumerable<TSource, TKey>(source, keySelector, descending);
        }

        /// <summary>
        /// Creates a dictionary from enumerable.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TKey">the key type.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <param name="keySelector">The key selector funtion.</param>
        /// <returns>Returns the dictionary.</returns>
        public static Dictionary<TKey, TSource> ExtensionToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
            {
                throw new NullReferenceException("Source is null");
            }
            Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>();
            foreach (var item in source)
            {
                dictionary.Add(keySelector(item), item);
            }
            return dictionary;
        }
    }


}