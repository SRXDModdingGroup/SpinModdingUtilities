using System;
using System.Collections.Generic;

namespace SMU.Utilities {
    /// <summary>
    /// Utility class for queueing changes to be made to a list
    /// </summary>
    /// <typeparam name="T">The type of the values to be inserted</typeparam>
    public class DeferredListOperation<T> {
        private abstract class Operation : IComparable<Operation> {
            protected int Index { get; }

            protected Operation(int index) {
                Index = index;
            }

            public abstract void Execute(List<T> list);

            public int CompareTo(Operation other) => -Index.CompareTo(other.Index);
        }

        private class Insertion : Operation {
            private IList<T> toInsert;

            public Insertion(int index, IList<T> toInsert) : base(index) => this.toInsert = toInsert;

            public override void Execute(List<T> list) => list.InsertRange(Index, toInsert);
        }

        private class Removal : Operation {
            private int count;

            public Removal(int index, int count) : base(index) => this.count = count;
            
            public override void Execute(List<T> list) => list.RemoveRange(Index, count);
        }
        
        private class Replacement : Operation {
            private int count;
            private IList<T> toInsert;

            public Replacement(int index, int count, IList<T> toInsert) : base(index) {
                this.count = count;
                this.toInsert = toInsert;
            }

            public override void Execute(List<T> list) {
                list.RemoveRange(Index, count);
                list.InsertRange(Index, toInsert);
            }
        }

        private List<Operation> operations = new List<Operation>();

        /// <summary>
        /// Adds a set of values to be inserted
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="list">The set of values to be inserted</param>
        public void Insert(int index, IList<T> list) => operations.Add(new Insertion(index, list));

        /// <summary>
        /// Adds a range of indices to be removed
        /// </summary>
        /// <param name="index">The starting index to remove at</param>
        /// <param name="count">The number of items to remove</param>
        /// <remarks>Avoid queueing multiple removals with overlapping ranges</remarks>
        public void Remove(int index, int count) => operations.Add(new Removal(index, count));

        /// <summary>
        /// Adds a range of indices to be replaced with a new set of values
        /// </summary>
        /// <param name="index">The starting index to replace at</param>
        /// <param name="count">The number of items to remove</param>
        /// <param name="list">The set of values to be inserted</param>
        /// <remarks>Avoid queueing multiple replacements with overlapping ranges</remarks>
        public void Replace(int index, int count, IList<T> list) => operations.Add(new Replacement(index, count, list));

        /// <summary>
        /// Performs the queued operations on a list and clears the queue
        /// </summary>
        /// <param name="list">The list to insert into</param>
        public void Execute(List<T> list) {
            operations.Sort();

            foreach (var operation in operations)
                operation.Execute(list);
                
            operations.Clear();
        }
    }
}