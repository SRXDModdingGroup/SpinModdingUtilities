using System;
using System.Collections.Generic;

namespace SMU.Extensions
{
    /// <summary>
    /// Extension class for IEnumerables
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Loop through every element in the sequence, applying the given action.
        /// </summary>
        /// <typeparam name="T">Type of each element.</typeparam>
        /// <param name="source">The enumerable to perform the action on.</param>
        /// <param name="act">The action to perform.</param>
        /// <returns>The modified enumerable.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> act)
        {
            foreach (T element in source) act(element);
            return source;
        }
    }
}
