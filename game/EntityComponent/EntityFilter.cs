using System;

namespace game.EntityComponent
{
    /// <summary>
    /// Base interface for all entity filter types. This exists in order to allow filter
    /// that match different types of components to be stored in a single data structure without
    /// the loss of type safety.
    /// </summary>
    public interface IEntityFilter
    {
        /// <summary>
        /// Apply filter to given entity.
        /// </summary>
        /// <param name="e">Entity to apply filter to</param>
        /// <returns>Flag indicating whether filter excludes or includes given entity</returns>
        bool Apply(Entity e);
    }

    /// <summary>
    /// A class representing a filter that can be used to filter sets of entities
    /// based on whether they contain a component of a certain type and, optionally,
    /// fulfil arbitrary conditions imposed on their current data.
    /// </summary>
    /// <typeparam name="T">The type of component to match for. Has to be derived from IComponent</typeparam>
    public class EntityFilter<T> : IEntityFilter
        where T : class, IComponent
    {
        /// <summary>
        /// The predicate used to further filter entities that contain a component of
        /// type T
        /// </summary>
        /// <remarks>
        /// The default value is the trivial predicate that returns true for every possible
        /// input.
        /// </remarks>
        protected Func<T, bool> Predicate
        {
            get;
            set;
        } = (_ => true);

        /// <summary>
        /// Create a filter that matches for existance of a component with type T.
        /// Do not define any predicate to be applied to the data.
        /// </summary>
        public EntityFilter()
        {
            
        }

        /// <summary>
        /// Create a filter that both matches for existance of a component with type T,
        /// aswell as certain conditions imposed on the data managed by that component instance.
        /// These conditions are implemented as a predicate.
        /// </summary>
        /// <param name="predicate">The predicate used to filter the entities further</param>
        public EntityFilter(Func<T, bool> predicate)
        {
            Predicate = predicate;
        }

        /// <summary>
        /// Apply filter to given entity.
        /// </summary>
        /// <param name="e">Entity to apply filter to</param>
        /// <returns>Flag indicating whether filter excludes or includes given entity</returns>
        public bool Apply(Entity e)
        {
            return e.HasComponent<T>() && Predicate(e.GetComponent<T>());
        }

        /// <summary>
        /// Convenience static method that creates an entity filter that matches entities
        /// that contain a component of given type.
        /// </summary>
        /// <returns>Created entity filter</returns>
        public static EntityFilter<T> ByComponent()
        {
            return new EntityFilter<T>();
        }
        
        /// <summary>
        /// Convenience static method that creates an entity filter that matches entities
        /// that both contain a component of given type and satisfy the given predicate.
        /// </summary>
        /// <param name="predicate">Predicate that is defined for given component type</param>
        /// <returns>Created entity filter</returns>
        public static EntityFilter<T> ByPredicate(Func<T, bool> predicate)
        {
            return new EntityFilter<T>(predicate);
        }
    }
}