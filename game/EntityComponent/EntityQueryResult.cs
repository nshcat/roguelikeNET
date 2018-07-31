using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace game.EntityComponent
{
    /// <summary>
    /// A class representing the result of a entity query, for example a filter operation
    /// for a specific component type. It allows chained queries.
    /// </summary>
    public class EntityQueryResult : IEnumerable<Entity>
    {
        /// <summary>
        /// An enumerable storing the set of entities that resulted from the query
        /// that created this instance
        /// </summary>
        protected IEnumerable<Entity> Result
        {
            get;
            set;
        }

        /// <summary>
        /// Internal constructor to create a new instance based on a LINQ-query result
        /// </summary>
        /// <param name="result"></param>
        internal EntityQueryResult(IEnumerable<Entity> result)
        {
            Result = result;
        }
   
        /// <summary>
        /// Get all entities that satisfy the given entity filter.
        /// </summary>
        /// <param name="filter">Entity filter to use</param>
        /// <returns>Collection of entities as result of filter operation</returns>
        public EntityQueryResult GetEntities(IEntityFilter filter)
        {
            var result = Result
                .Where(x => filter.Apply(x));
            
            return new EntityQueryResult(result);
        }

        /// <summary>
        /// Get all entities that satisfy all given filters.
        /// </summary>
        /// <param name="filters">Collection of filters that entities have to satisfy</param>
        /// <returns>Collection of entities as result of filter operation</returns>
        public EntityQueryResult GetEntities(params IEntityFilter[] filters)
        {
            var result = Result
                .Where(x => filters
                    .All(y => y.Apply(x)));
            
            return new EntityQueryResult(result);
        }
        
        /// <summary>
        /// Get all entities that contain a component of given type that satisfies the given predicate
        /// </summary>
        /// <param name="predicate">Predicate operating on component instance</param>
        /// <typeparam name="T">Type of the component to filter by</typeparam>
        /// <returns>Collection of entities as result of filter operation</returns>
        public EntityQueryResult GetEntities<T>(Func<T, bool> predicate) where T : class, IComponent
        {
            var result = Result
                .Where(x => x.HasComponent<T>())
                .Where(x => predicate(x.GetComponent<T>()))
                .ToList();
            
            return new EntityQueryResult(result);
        }

        /// <summary>
        /// Get all entities that contain a component of given type
        /// </summary>
        /// <typeparam name="T">Type of the component to filter by</typeparam>
        /// <returns>Collection of entities as result of filter operation</returns>
        public EntityQueryResult GetEntities<T>() where T : class, IComponent
        {
            return GetEntities<T>(_ => true);
        }

        /// <summary>
        /// Extract component of given type from every entity in this result. This will fail if
        /// there are entities that do not have a matching component.
        /// </summary>
        /// <typeparam name="T">Type of component to extract</typeparam>
        /// <returns>Collection of the extracted components</returns>
        public IEnumerable<T> GetComponents<T>() where T : class, IComponent
        {
            return Result.Select(x => x.GetComponent<T>());
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return Result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}