using System;
using System.Collections.Generic;

namespace SMU.Utilities {
    /// <summary>
    /// Utility class for queueing insertions to be made to a list
    /// </summary>
    /// <typeparam name="T">The type of the values to be inserted</typeparam>
    public class DeferredInsertion<T> {
        private readonly struct Insertion : IComparable<Insertion> {
            public int Index { get; }
                
            public IList<T> List { get; }

            public Insertion(int index, IList<T> list) {
                Index = index;
                List = list;
            }

            public int CompareTo(Insertion other) => -Index.CompareTo(other.Index);
        }

        private List<Insertion> insertions = new List<Insertion>();

        /// <summary>
        /// Adds a set of values to be inserted
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="list">The set of values to be inserted</param>
        public void Add(int index, IList<T> list) => insertions.Add(new Insertion(index, list));

        /// <summary>
        /// Inserts the queued values into a list and clears the queue
        /// </summary>
        /// <param name="list">The list to insert into</param>
        public void Insert(List<T> list) {
            insertions.Sort();

            foreach (var insertion in insertions)
                list.InsertRange(insertion.Index, insertion.List);
                
            insertions.Clear();
        }
    }
}