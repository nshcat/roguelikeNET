using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace game
{
    /// <summary>
    /// An entry in a weighted distribution. Contains a value and its probability.
    /// </summary>
    /// <typeparam name="T">Type of the stored value</typeparam>
    public class WeightedEntry<T> : ICloneable
    {
        /// <summary>
        /// The raw, non-normalized probability that this entry will be chosen.
        /// Note that this does not have to be a percentage: since the values
        /// will get normalized by <see cref="WeightedDistribution{T}"/>, any value
        /// can be used here.
        /// </summary>
        public double Probability
        {
            get;
            set;
        }

        /// <summary>
        /// The actual value this entry is associated with.
        /// </summary>
        public T Value
        {
            get;
            set;
        }

        /// <summary>
        /// Construct new entry from given value and probability.
        /// </summary>
        /// <param name="probability">Probability</param>
        /// <param name="value">Value</param>
        internal WeightedEntry(double probability, T value)
        {
            Probability = probability;
            Value = value;
        }

        protected bool Equals(WeightedEntry<T> other)
        {
            return Probability.Equals(other.Probability) && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WeightedEntry<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Probability.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
            }
        }

        public object Clone()
        {
            return new WeightedEntry<T>(Probability, Value);
        }

        public static bool operator ==(WeightedEntry<T> left, WeightedEntry<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WeightedEntry<T> left, WeightedEntry<T> right)
        {
            return !Equals(left, right);
        }
    }
    
    /// <summary>
    /// A distribution of values, where each value is assigned a probabilty. Allows the user to
    /// pick random values, with a random distribution according to the assigned probability values.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    public class WeightedDistribution<T> : IEnumerable<WeightedEntry<T>>
    {
        /// <summary>
        /// Whether re-normalization is needed before the next value can be picked. This is caused
        /// by adding new elements after normalization.
        /// </summary>
        protected bool NeedsNormalization
        {
            get;
            set;
        }
        
        /// <summary>
        /// All entries registered with this distribution, in their non-normalized state. Note that this collection
        /// is not used for the actual algorithm, only for re-normalization purposes when new entries
        /// are added after-the-fact.
        /// </summary>
        protected List<WeightedEntry<T>> Entries
        {
            get;
        } = new List<WeightedEntry<T>>();

        /// <summary>
        /// Collection of all entries, normalized according to the total probability. This is used as part
        /// of the random selection algorithm.
        /// </summary>
        protected List<WeightedEntry<T>> NormalizedEntries
        {
            get;
            set;
        } = new List<WeightedEntry<T>>();

        /// <summary>
        /// Add new entry. This is supplied to allow simpler collection initialization.
        /// </summary>
        /// <param name="probability">Probability</param>
        /// <param name="value">Value</param>
        public void Add(double probability, T value)
        {
            Add(new WeightedEntry<T>(probability, value));
        }

        /// <summary>
        /// Add new entry. This will cause a re-normalization to happen when the next
        /// value is requested by the user.
        /// </summary>
        /// <param name="entry">Entry to add</param>
        public void Add(WeightedEntry<T> entry)
        {
            Entries.Add(entry);
            
            // We added a new element, so re-normalization needs to happen.
            NeedsNormalization = true;
        }

        /// <summary>
        /// Retrieve enumerator to the stored entries. Note that this references the non-normalized
        /// entries.
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<WeightedEntry<T>> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        /// <summary>
        /// Retrieve enumerator to the stored entries. Note that this references the non-normalized
        /// entries.
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Normalize the registered entries to make the probabilties adhere to the rules of
        /// a proper random distribution. This means they will all be inside the interval [0, 1]
        /// and sum up to 1.
        /// </summary>
        protected void Normalize()
        {
            // Clear old normalized entry list
            NormalizedEntries.Clear();
            
            // Find sum of all probabilities. This is used as a base for normalization.
            var total = Entries.Select(x => x.Probability).Sum();

            // Create new collection of normalized entries
            NormalizedEntries = Entries.Select(x => new WeightedEntry<T>(x.Probability / total, x.Value)).ToList();

            // No more normalization is needed until entries are added/removed.
            NeedsNormalization = false;
        }

        /// <summary>
        /// Pick next random value.
        /// </summary>
        /// <param name="rng">Random number generator instance to use</param>
        /// <returns>A new value, picked at random from the distribution</returns>
        public T PickValue(Random rng)
        {
            // Pick random value in [0, 1)
            var randomValue = rng.NextDouble();

            return PickValue(randomValue);
        }
        
        
        /// <summary>
        /// Pick next value, based deterministically on given seed number.
        /// </summary>
        /// <param name="val">Seed value in [0, 1).</param>
        /// <returns>Value corresponding to given input value</returns>
        /// <remarks>
        /// For every value x, this function will always return the same output value, given that
        /// the internal collection is not modified.
        /// </remarks>
        public T PickValue(double val)
        {
            // Normalize first if needed
            if (NeedsNormalization)
                Normalize();
            
            // Implement russian roulette method of selecting the proper value.
            var currentSum = 0.0;
            foreach (var entry in NormalizedEntries)
            {
                currentSum += entry.Probability;

                if (val <= currentSum)
                    return entry.Value;
            }
            
            // This should not happen, just return last value.
            return NormalizedEntries.Last().Value;
        }
    }
}