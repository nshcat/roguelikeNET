using System.Collections.Generic;
using System.Linq;

namespace game.EntityComponent
{
    /// <summary>
    /// A class representing a system in the context of an ECS that automatically gets
    /// notified when elements get removed / added to the set of entities that satisfy
    /// the current filter
    /// </summary>
    public abstract class ObserverSystem : System
    {
        /// <summary>
        /// The last known set of entities that satisfy the filter of this system.
        /// This is used to determine the entities that got removed or added in
        /// the following update.
        /// </summary>
        protected EntityQueryResult LastResult
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public ObserverSystem(IEnumerable<IEntityFilter> filters)
            : base(filters)
        {
            
        }

        /// <summary>
        /// Update system state. This causes the implementation of <see cref="Update(long, EntityQueryResult)"/> to
        /// be called with a set of entities chosen based on the currently set filters, and the implementations
        /// of <see cref="OnEntityAdded"/> and <see cref="OnEntityRemoved"/> to be called accordingly.
        /// </summary>
        /// <param name="elapsedTicks">The amount of ticks that elapsed since the last update</param>
        public override void Update(long elapsedTicks)
        {
            // Retrieve new query result containing all entities that satisfy the current filter
            var result = EntityManager.AllEntities.GetEntities(Filters.ToArray());
            
            // If the currently stored last result is null, this is the first update.
            // This means all entities are to be handled as new.
            if(LastResult == null)
            {
                foreach (var e in result)
                    OnEntityAdded(e);      
            }
            else
            {
                // Determine all entities that got removed
                foreach(var e in LastResult.Where(x => !result.Contains(x)))
                    OnEntityRemoved(e);
                
                // Determine all entites that got added
                foreach (var e in result.Where(x => !LastResult.Contains(x)))
                    OnEntityAdded(e);
            }
            
            
            // Save current result for next update
            LastResult = result;
            
            // Call the internal update implementation
            base.Update(elapsedTicks);
        }
        
        /// <summary>
        /// On update, this method is called for every entity that got removed from the set of entities
        /// that satisfy the given filter.
        /// </summary>
        /// <param name="e">Entity that got removed</param>
        protected abstract void OnEntityRemoved(Entity e);
        
        /// <summary>
        /// On update, this method is called for every entity that got added to the set of entities
        /// that satisfy the given filter.
        /// </summary>
        /// <param name="e">Entity that got added</param>
        protected abstract void OnEntityAdded(Entity e);
    }
}