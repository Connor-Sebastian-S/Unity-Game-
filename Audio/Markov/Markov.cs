using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class Markov<T> where T : IComparable
    {
        private readonly List<Sequence<T>> _trainedSequence = new List<Sequence<T>>();
        private readonly Random _random = new Random();
        private readonly T _stopKey;

        private class Sequence<T> where T : IComparable
        {
            public T Entry { get; set; }
            public List<T> EntryChain { get; set; }
        }

        public Markov(T stopKey)
        {
            _stopKey = stopKey;
        }

        /// <summary>
        /// Train a piece
        /// </summary>
        /// <param name="trainingEntries"></param>
        /// <param name="order"></param>
        public void Train(List<T> trainingEntries, int order)
        {
            for (var idx = 0; idx < trainingEntries.Count; idx++)
            {
                var key = trainingEntries[idx];
                if (key.CompareTo(_stopKey) == 0) continue;
                var entryChain = new List<T>();
                for (var i = 0; i < order; i++)
                {
                    var e = trainingEntries[(idx + i + 1) % trainingEntries.Count];
                    entryChain.Add(e);
                    if (e.CompareTo(_stopKey) == 0) break;
                }

                if (entryChain.Count <= 0) continue;
                var entry = new Sequence<T>
                {
                    Entry = key,
                    EntryChain = entryChain
                };
                _trainedSequence.Add(entry);
            }
        }

        /// <summary>
        /// Create a piece of length idealLength
        /// </summary>
        /// <param name="idealLength"></param>
        /// <param name="includeStopKey"></param>
        /// <returns></returns>
        public List<T> GeneratePiece(int idealLength, bool includeStopKey)
        {
            var result = new List<T>();

            // Get starting point
            var key = _trainedSequence[_random.Next(0, _trainedSequence.Count)].Entry;
            result.Add(key);

            while (result.Count < idealLength)
            {
                // Count the sequences that start at that point, and randomly choose one
                var sequences = _trainedSequence.Count(t => t.Entry.CompareTo(key) == 0);

                // Loop random sequences that start with the key
                if (sequences > 0)
                {
                    var neededSequence = _random.Next(0, sequences);
                    sequences = 0;
                    foreach (var t in _trainedSequence)
                        if (t.Entry.CompareTo(key) == 0)
                        {
                            // Is this sequence iteration is what we want
                            if (sequences == neededSequence)
                            {
                                // Add the contents of the sequence
                                foreach (var t1 in t.EntryChain)
                                {
                                    var newBit = t1;
                                    // If stop key, only include it if requested 
                                    if (newBit.CompareTo(_stopKey) == 0 && !includeStopKey) continue;
                                    result.Add(newBit);
                                    key = t1;
                                }

                                break;
                            }

                            sequences++;
                        }
                }
                else
                {
                    // No sequences so start somewhere else
                    key = _trainedSequence[_random.Next(0, _trainedSequence.Count)].Entry;
                    result.Add(key);
                }
            }

            // The length has been reached (or exceeded)
            return result;
        }

        public string GeneratePiece(int idealLength, string separator, bool includeStopKey)
        {
            var result = new StringBuilder();
            var intermediate = GeneratePiece(idealLength, includeStopKey);
            for (var idx = 0; idx < intermediate.Count; idx++)
                result.Append((idx > 0 ? separator : "") + intermediate[idx]);
            return result.ToString();
        }
    }