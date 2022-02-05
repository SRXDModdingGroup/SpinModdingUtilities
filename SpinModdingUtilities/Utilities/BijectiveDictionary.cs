using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMU.Utilities
{    /// <summary>
     /// Class that allows you to access values within a dictionary in both directions
     /// </summary>
     /// <typeparam name="T1">The first type within the BijectiveDictionary</typeparam>
     /// <typeparam name="T2">The second type within the BijectiveDictionary</typeparam>
    public class BijectiveDictionary<T1, T2>
    {
        private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        /// <summary>
        /// Constructor. Assigns the Indexers
        /// </summary>
        public BijectiveDictionary()
        {
            this.Forward = new Indexer<T1, T2>(_forward);
            this.Reverse = new Indexer<T2, T1>(_reverse);
        }

        /// <summary>
        /// Constructor. Assigns the Indexers
        /// </summary>
        public class Indexer<T3, T4>
        {
            private Dictionary<T3, T4> _dictionary;

            /// <summary>
            /// Constructor. Sets the Dictionary
            /// </summary>
            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }
            /// <summary>
            /// Declares T4
            /// </summary>
            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }
        }

        /// <summary>
        /// Adds element to the BijectiveDictionary
        /// </summary>
        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        /// <summary>
        /// Removes element out of the BijectiveDictionary
        /// </summary>
        public void Remove(T1 t1)
        {
            _reverse.Remove(_forward[t1]);
            _forward.Remove(t1);
        }

        /// <summary>
        /// Removes element out of the BijectiveDictionary
        /// </summary>
        public void Remove(T2 t2)
        {
            _forward.Remove(_reverse[t2]);
            _reverse.Remove(t2);
        }

        /// <summary>
        /// Accessor for T1 -> T2
        /// </summary>
        public Indexer<T1, T2> Forward { get; private set; }

        /// <summary>
        /// Accessor for T2 -> T1
        /// </summary>
        public Indexer<T2, T1> Reverse { get; private set; }
    }
}
