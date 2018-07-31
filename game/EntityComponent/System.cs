using System.Collections.Generic;
using System.Linq;

namespace game.EntityComponent
{
    /// <summary>
    /// A class representing a system in the context of an ECS, which is a module
    /// that only consumes entities that match a certain set of filters and has a very
    /// specific task to fulfil.
    /// </summary>
    /// <remarks>
    /// After defining the appropiate set of filters (which is usually done in the constructor), a system
    /// deriving from this class will only receive entities that match the filters, so no additional
    /// filtering is needed.
    /// </remarks>
    /// <example>
    /// For example, one could implement a system that constantly watches over all entities that
    /// have the component "Particle" and removes entities from the global registry iff their
    /// life time has exceeded the maximum duration. This could be implemented by an additional
    /// predicate filter thats checks for just that expiration.
    /// </example>
    public abstract class System
    {
        /// <summary>
        /// The collection of filters used to determine the set of entities to supply to
        /// this system.
        /// </summary>
        public IEnumerable<IEntityFilter> Filters
        {
            get;
            protected set;
        } = new List<IEntityFilter>();

        /// <summary>
        /// Update system state. This causes the implementation of <see cref="Update(long, EntityQueryResult)"/> to
        /// be called with a set of entities chosen based on the currently set filters.
        /// </summary>
        /// <param name="elapsedTicks">The amount of ticks that elapsed since the last update</param>
        public void Update(long elapsedTicks)
        {
            Update(elapsedTicks, EntityManager.AllEntities.GetEntities(Filters.ToArray()));
        }

        /// <summary>
        /// Method that implements the actual work the system does.
        /// </summary>
        /// <param name="elapsedTicks">The amount of ticks that elapsed since the last update</param>
        /// <param name="entities">Set of pre-filtered entities that the system can work on</param>
        protected abstract void Update(long elapsedTicks, EntityQueryResult entities);
    }
}