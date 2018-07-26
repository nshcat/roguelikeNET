using System;

namespace game
{
    /// <summary>
    /// A helper class that makes it possible to define events that happen every certain amount
    /// of in-game simulation ticks.
    /// </summary>
    /// <remarks>
    /// Once set, the period is immutable and therefore cannot be changed. To change the period of an event,
    /// a new TickCounter instance has to be constructed with the new period.
    /// </remarks>
    public class TickCounter
    {
        protected static Random ClassRandom = new Random();
        protected static object ClassMutex = new object();
        
        /// <summary>
        /// The period of this tick counter. It determines how often the event is fired.
        /// </summary>
        public long Period
        {
            get;
            protected set;
        }

        /// <summary>
        /// The current tick count. Will be added on next call of update.
        /// </summary>
        protected long Counter
        {
            get;
            set;
        } = 0;


        /// <summary>
        /// Construct new tick counter instance with given period
        /// </summary>
        /// <param name="period">Period of the tick counter</param>
        public TickCounter(long period)
        {
            Period = period;
        }

        /// <summary>
        /// Construct new tick counter instance with given period and initial counter value. This allows creation
        /// of multiple tick counters that are "out of sync"
        /// </summary>
        /// <param name="period">Period</param>
        /// <param name="initialValue">Initial value of the internal counter</param>
        public TickCounter(long period, long initialValue)
        {
            Period = period;
            Counter = initialValue;
        }

        /// <summary>
        /// Construct new tick counter instance with random initial counter value
        /// </summary>
        /// <param name="period">Period</param>
        /// <returns></returns>
        public static TickCounter WithRandomInitialValue(long period)
        {
            // Lock mutex to avoid multiple threads accessing the class random number generator
            lock (ClassMutex)
                return new TickCounter(period, ClassRandom.Next(0, (int)period));
        }

        /// <summary>
        /// Process given amount of elapsed ticks and return the amount of periods that elapsed this update
        /// </summary>
        /// <param name="elapsedTicks">Elapsed ticks</param>
        /// <returns>Amount of periods that elapsed</returns>
        public long Update(long elapsedTicks)
        {
            var totalCount = elapsedTicks + Counter;
            var elapsedPeriods = totalCount / Period;
            Counter = totalCount % Period;

            return elapsedPeriods;
        }

        /// <summary>
        /// Process given amount of elapsed ticks and return a flag indicating whether at least one full period
        /// elapsed
        /// </summary>
        /// <param name="elapsedTicks">Elapsed ticks</param>
        /// <returns>Flag indicating whether at least one full period elapsed</returns>
        public bool UpdateSimple(long elapsedTicks)
        {
            return Update(elapsedTicks) > 0;
        }
    }
}