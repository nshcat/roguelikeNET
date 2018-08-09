using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using game.Ascii;

namespace game
{
    public class PriorityQueueItem<TValue, TPriority>
        : IPriorityQueueItem<TValue, TPriority>
        where TPriority : IComparable<TPriority>
    {
        public TValue Item
        {
            get;
            set;
        }

        public TPriority Priority
        {
            get;
            set;
        }

        public PriorityQueueItem(TValue item, TPriority priority)
        {
            Item = item;
            Priority = priority;
        }
    }

    public class PriorityQueue<TValue, TPriority>
        : IPriorityQueue<TValue, TPriority>, IEnumerable<IPriorityQueueItem<TValue, TPriority>>
        where TPriority : IComparable<TPriority>
    {
        /// <summary>
        /// All items currently stored in the queue. The priority queue is implemented
        /// by an implicit min-heap.
        /// </summary>
        private List<PriorityQueueItem<TValue, TPriority>> Items
        {
            get;
            set;
        } = new List<PriorityQueueItem<TValue, TPriority>>();

        /// <summary>
        /// The amount of items that are currently stored in the queue.
        /// </summary>
        public int Count => Items.Count;
        
        public void AdjustPriorities(Func<TPriority, TPriority> operation)
        {
            Items = Items.Select(x => new PriorityQueueItem<TValue, TPriority>(x.Item, operation(x.Priority))).ToList();
        }

        public void Enqueue(TValue item, TPriority priority)
        {
            // Insert at end of list
            Items.Add(new PriorityQueueItem<TValue, TPriority>(item, priority));
            
            // Index of the new element
            var current = Count - 1;
            
            // Does the element need to be ascended further?
            while (!IsRoot(current) && Items[Parent(current)].Priority.CompareTo(priority) > 0)
            {
                Swap(current, Parent(current));
                current = Parent(current);
            }
        }

        public void Remove(TValue item)
        {
            if(Count <= 0)
                throw new InvalidOperationException("Queue is empty");
            
            // Find position of item
            var index = Items.FindIndex(x => x.Item.Equals(item));
            
            if(index == -1)
                throw new ArgumentException("Could not find item in queue");
            
            // Special case: Queue with only one element
            if (Count == 1)
            {
                Clear();
            }
            else if(index == Count - 1) // Special case: last element
            {
                Items.RemoveAt(Count - 1);
            }
            else
            {
                // Plug hole with last element
                Items[index] = Items[Count - 1];
                Items.RemoveAt(Count - 1);
                
                // Try to move new item up
                var i = index;
                while (!IsRoot(i) && Items[i].Priority.CompareTo(Items[Parent(i)].Priority) < 0)
                {
                    Swap(i, Parent(i));
                    i = Parent(i);
                }
                
                // If i didnt change, we didnt made the new item ascend.
                // We might need to descend it.
                if(i == index)
                    Descend(i);
            }
        }

        public IPriorityQueueItem<TValue, TPriority> Dequeue()
        {
            if (Count <= 0)
                throw new InvalidOperationException("Priority queue was empty");

            // Retrieve element with smallest priority (backing array implements a
            // min heap)
            var item = Items[0];
            Items.RemoveAt(0);
            
            // Check if we have any elements left to use as new root element
            if (Count > 0)
            {
                Items.Insert(0, Items[Count - 1]);
                Items.RemoveAt(Count - 1);
                Descend(0);
            }

            return item;
        }

        public void Clear()
        {
            Items.Clear();
        }

        public IEnumerator<IPriorityQueueItem<TValue, TPriority>> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Swap the two elements with given indices in the backing list.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private void Swap(int first, int second)
        {
            var tmp = Items[first];
            Items[first] = Items[second];
            Items[second] = tmp;
        }

        private int LeftChild(int index)
        {
            return (2 * index) + 1;
        }
        
        private int RightChild(int index)
        {
            return (2 * index) + 2;
        }

        private int Parent(int index)
        {
            if (index == 0)
                throw new ArgumentException("Root node does not have a parent node");

            return (index - 1) / 2;
        }

        private bool IsLeaf(int index)
        {
            return (2 * index + 1) >= Count;
        }

        private bool IsRoot(int index)
        {
            return index == 0;
        }
        
        private void Descend(int index)
        {
            if (IsLeaf(index))
                return;
            
            var left = LeftChild(index);
            var right = RightChild(index);
            int smallest = index;

            if ((left < Count) && Items[left].Priority.CompareTo(Items[index].Priority) < 0)
            {
                smallest = left;
            }
            
            if ((right < Count) && Items[right].Priority.CompareTo(Items[smallest].Priority) < 0)
            {
                smallest = right;
            }

            if (smallest != index)
            {
                Swap(index, smallest);
                Descend(smallest);
            }
        }
    }
}