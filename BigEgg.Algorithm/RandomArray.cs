using System;
using System.Collections.Generic;

namespace BigEgg.Algorithm
{
    /// <summary>
    /// Use a pseudo-random number generator to generate non-duplicate 
    /// 32-bit signed integer array.
    /// </summary>
    public class RandomArray
    {
        #region Fields
        private List<int> array;
        private int index;
        private int? seed;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomArray" /> class with the specified initial capacity.
        /// Using a time-dependent default seed value.
        /// </summary>
        /// <param name="capacity">The number of elements that the list can initially store the random numbers.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">capacity is less than or equal to 0.</exception>
        public RandomArray(int capacity)
            : this(null, capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomArray" /> class with the specified initial capacity.
        /// Using the specified seed value.
        /// </summary>
        /// <param name="seed">A number used to calculate a starting value for the pseudo-random number
        /// sequence. If a negative number is specified, the absolute value of the number
        /// is used.</param>
        /// <param name="capacity">The number of elements that the list can initially store the random numbers.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">capacity is less than or equal to 0.</exception>
        public RandomArray(int? seed, int capacity)
        {
            if (capacity <= 0) { throw new ArgumentOutOfRangeException("capacity is less than or equal to 0."); }

            this.array = new List<int>();
            this.index = 0;
            this.seed = seed;

            if (seed.HasValue)
            {
                Initialize(new Random(seed.Value), capacity);
            }
            else
            {
                Initialize(new Random(), capacity);
            }
        }


        #region Methods
        #region Public Methods
        /// <summary>
        /// Returns the next nonnegative random number.
        /// </summary>
        /// <returns>
        /// The next 32-bit signed integer greater than or equal to zero and less than the list's capacity.
        /// </returns>
        public int Next()
        {
            if (this.index >= this.array.Count)
            {
                this.index = 0;

                //  Re-generate the random number array.
                if (this.seed.HasValue)
                {
                    Initialize(new Random(this.seed.Value), this.array.Count);
                }
                else
                {
                    Initialize(new Random(), this.array.Count);
                }
            }

            return this.array[this.index];
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initializes the random array.
        /// </summary>
        /// <param name="generator">The random number generator.</param>
        /// <param name="capacity">The number of elements that the list can initially store the random numbers.</param>
        /// <exception cref="System.ArgumentException">generator cannot be null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">capacity is less than or equal to 0.</exception>
        private void Initialize(Random generator, int capacity)
        {
            if (generator == null) { throw new ArgumentException("generator cannot be null."); }
            if (capacity <= 0) { throw new ArgumentOutOfRangeException("capacity is less than or equal to 0."); }

            this.array.Clear();
            while (this.array.Count < capacity)
            {
                var newNumber = generator.Next(capacity + 1);
                if (newNumber == 0 || this.array.Contains(newNumber))
                {
                    continue;
                }

                this.array.Add(newNumber);
            }
        }
        #endregion
        #endregion
    }
}
