using System;
using System.Collections;
using System.Collections.Generic;

namespace SMU.Utilities {
    /// <summary>
    /// Utility class for finding patterns in a collection
    /// </summary>
    public static class PatternMatching {
        /// <summary>
        /// Stores the start (inclusive) and end (exclusive) indices of a found pattern
        /// </summary>
        public readonly struct Result {
            /// <summary>
            /// The start index of the pattern (inclusive)
            /// </summary>
            public int Start { get; }
            
            /// <summary>
            /// The start index of the pattern (exclusive)
            /// </summary>
            public int End { get; }
            
            /// <summary>
            /// The length of the pattern
            /// </summary>
            public int Length => End - Start;

            internal Result(int start, int end) {
                Start = start;
                End = end;
            }
        }
        
        /// <summary>
        /// Finds all instances of a given pattern in a collection
        /// </summary>
        /// <param name="list">The collection to search</param>
        /// <param name="pattern">The pattern to find. Consists of a series of lambda expressions that specify a condition that each item in the pattern must meet</param>
        /// <param name="startIndex">The first index to search. 0 by default</param>
        /// <param name="endIndex">The last index to search. The end of the collection by default</param>
        /// <typeparam name="T">The type of the collection's contents</typeparam>
        /// <returns>An enumerable sequence of matches. Each match is a list consisting of a single result</returns>
        public static MatchSequence<T> Match<T>(IList<T> list, Func<T, bool>[] pattern, int startIndex = 0, int endIndex = -1) {
            if (endIndex < 0)
                endIndex = list.Count;

            return new NormalPatternMatcher<T>(list, startIndex, endIndex, pattern);
        }

        /// <summary>
        /// Finds the first occurrence of a pattern after each occurrence of a previously found pattern
        /// </summary>
        /// <param name="matcher">The previously found sequence of matches</param>
        /// <param name="pattern">The new pattern to find. Consists of a series of lambda expressions that specify a condition that each item in the pattern must meet</param>
        /// <typeparam name="T">The type of the collection's contents</typeparam>
        /// <returns>A sequence of matches consisting of any previous matches in which the new pattern could be found after. The result for the new pattern is appended to the list from the previous match</returns>
        public static MatchSequence<T> Then<T>(this MatchSequence<T> matcher, Func<T, bool>[] pattern) => new ThenPatternMatcher<T>(matcher, pattern);
        
        private static bool IsMatch<T>(IList<T> list, Func<T, bool>[] pattern, int index, out Result result) {
            for (int i = 0, j = index; i < pattern.Length; i++, j++) {
                if (pattern[i](list[j]))
                    continue;

                result = new Result();
                
                return false;
            }

            result = new Result(index, index + pattern.Length);

            return true;
        }

        /// <summary>
        /// A sequence of successful matches
        /// </summary>
        /// <typeparam name="T">The type of the collection's contents</typeparam>
        public abstract class MatchSequence<T> : IEnumerable<List<Result>> {
            /// <summary>
            /// The collection that was searched
            /// </summary>
            public IList<T> List { get; }
            
            /// <summary>
            /// The pattern that was matched
            /// </summary>
            public Func<T, bool>[] Pattern { get; }
            
            /// <summary>
            /// The first index that was searched
            /// </summary>
            public int StartIndex { get; }
            
            /// <summary>
            /// The last index that was searched
            /// </summary>
            public int EndIndex { get; }

            private protected MatchSequence(IList<T> list, int startIndex, int endIndex, Func<T, bool>[] pattern) {
                List = list;
                StartIndex = startIndex;
                EndIndex = endIndex;
                Pattern = pattern;
            }

            /// <summary>
            /// Gets an enumerator for the sequence
            /// </summary>
            public abstract IEnumerator<List<Result>> GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        private class NormalPatternMatcher<T> : MatchSequence<T> {
            public NormalPatternMatcher(IList<T> list, int startIndex, int endIndex, Func<T, bool>[] pattern) : base(list, startIndex, endIndex, pattern) { }
            
            public override IEnumerator<List<Result>> GetEnumerator() {
                for (int i = StartIndex; i <= EndIndex - Pattern.Length; i++) {
                    if (IsMatch(List, Pattern, i, out var result))
                        yield return new List<Result> { result };
                }
            }
        }

        private class ThenPatternMatcher<T> : MatchSequence<T> {
            private MatchSequence<T> matcher;

            public ThenPatternMatcher(MatchSequence<T> matcher, Func<T, bool>[] pattern) : base(matcher.List, matcher.StartIndex, matcher.EndIndex, pattern) {
                this.matcher = matcher;
            }
            
            public override IEnumerator<List<Result>> GetEnumerator() {
                using var enumerator = matcher.GetEnumerator();
                
                if (!enumerator.MoveNext())
                    yield break;

                var current = enumerator.Current;
                Result findResult;

                while (enumerator.MoveNext()) {
                    var next = enumerator.Current;

                    if (Find(current[current.Count - 1].End, next[0].Start, out findResult)) {
                        current.Add(findResult);
                        
                        yield return current;
                    }

                    current = next;
                }
                
                if (Find(current[current.Count - 1].End, EndIndex, out findResult)) {
                    current.Add(findResult);
                        
                    yield return current;
                }

                bool Find(int startIndex, int endIndex, out Result result) {
                    for (int i = startIndex; i <= endIndex - Pattern.Length; i++) {
                        if (IsMatch(List, Pattern, i, out result))
                            return true;
                    }

                    result = new Result();

                    return false;
                }
            }
        }
    }
}
