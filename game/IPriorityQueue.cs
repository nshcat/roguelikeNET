using System;

namespace game
{
    public interface IPriorityQueueItem<TValue, TPriority>
        where TPriority : IComparable<TPriority>
    {
        TValue Item
        {
            get;
            set;
        }

        TPriority Priority
        {
            get;
            set;
        }
    }
    
    public interface IPriorityQueue<TValue, TPriority>
        where TPriority : IComparable<TPriority>
    {
        /// <summary>
        /// Apply given operation to the priority of every item stored in the queue.
        /// </summary>
        /// <remarks>
        /// The behaviour is undefined if the operation does not do exactly the
        /// same thing to every priority, and does not invalidate the heap ordering.
        /// </remarks>-
        /// <param name="operation">Operation applied to the priority of every item</param>
        void AdjustPriorities(Func<TPriority, TPriority> operation);

        /// <summary>
        /// Enqueue item with given priority.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="priority"></param>
        void Enqueue(TValue item, TPriority priority);

        /// <summary>
        /// Remove given item, no matter where it is currently located in the queue.
        /// </summary>
        /// <param name="item"></param>
        void Remove(TValue item);
        
        /// <summary>
        /// Dequeue item with the lowest priority
        /// </summary>
        /// <returns></returns>
        IPriorityQueueItem<TValue, TPriority> Dequeue();

        /// <summary>
        /// Removes all elements from the queue.
        /// </summary>
        void Clear();
    }
}