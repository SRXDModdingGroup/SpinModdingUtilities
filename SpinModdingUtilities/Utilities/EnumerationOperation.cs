using System;
using System.Collections.Generic;

namespace SMU.Utilities {
    /// <summary>
    /// Utility class for queueing changes to be made to an enumeration
    /// </summary>
    /// <typeparam name="T">The type of the values to be inserted</typeparam>
    public class EnumerationOperation<T> {
        private readonly struct Insertion : IComparable<Insertion> {
            public int Index { get; }
            public IEnumerable<T> ToInsert { get; }

            public Insertion(int index, IEnumerable<T> toInsert) {
                Index = index;
                ToInsert = toInsert;
            }

            public int CompareTo(Insertion other) => Index.CompareTo(other.Index);
        }

        private readonly struct Removal : IComparable<Removal> {
            public int Index { get; }
            public int Count { get; }

            public Removal(int index, int count) {
                Index = index;
                Count = count;
            }

            public int CompareTo(Removal other) => Index.CompareTo(other.Index);
        }

        private List<Insertion> insertions = new List<Insertion>();
        private List<Removal> removals = new List<Removal>();

        /// <summary>
        /// Adds a set of values to be inserted
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="list">The set of values to be inserted</param>
        public void Insert(int index, IEnumerable<T> list) => InsertSorted(insertions, new Insertion(index, list));

        /// <summary>
        /// Adds a range of indices to be removed
        /// </summary>
        /// <param name="index">The starting index to remove at</param>
        /// <param name="count">The number of items to remove</param>
        public void Remove(int index, int count) => InsertSorted(removals, new Removal(index, count));

        /// <summary>
        /// Adds a range of indices to be replaced with a new set of values
        /// </summary>
        /// <param name="index">The starting index to replace at</param>
        /// <param name="count">The number of items to remove</param>
        /// <param name="list">The set of values to be inserted</param>
        public void Replace(int index, int count, IEnumerable<T> list) {
            Insert(index, list);
            Remove(index, count);
        }

        /// <summary>
        /// Returns a new sequence with the queued operations applied
        /// </summary>
        /// <param name="list">The sequence to apply the queued operations to</param>
        public IEnumerable<T> Enumerate(IEnumerable<T> list) {
            int index = 0;
            int nextInsertion = 0;
            int nextRemoval = 0;

            foreach (var item in list) {
                while (nextInsertion < insertions.Count) {
                    var insertion = insertions[nextInsertion];

                    if (insertion.Index > index)
                        break;

                    foreach (var itemToInsert in insertion.ToInsert)
                        yield return itemToInsert;

                    nextInsertion++;
                }

                bool yieldThis = true;

                while (nextRemoval < removals.Count) {
                    var removal = removals[nextRemoval];

                    if (removal.Index + removal.Count > index) {
                        if (removal.Index <= index)
                            yieldThis = false;

                        break;
                    }

                    nextRemoval++;
                }

                if (yieldThis)
                    yield return item;

                index++;
            }
        }

        private static void InsertSorted<U>(List<U> list, U item) where U : IComparable<U> {
            int index = list.BinarySearch(item);

            if (index < 0)
                index = ~index;

            list.Insert(index, item);
        }
    }
}