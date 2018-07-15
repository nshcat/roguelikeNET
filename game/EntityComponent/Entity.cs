using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent
{
    /// <summary>
    /// A class implementing a generic entity as part of an ECS-design.
    /// Each entity contains a collection of components which actually implement
    /// the behaviour and data stored in that particular type of entity.
    /// </summary>
    public sealed class Entity
    {
        /// <summary>
        /// A collection of all component instances associated with this entity.
        /// A dictionary is used to speed up access via identifier.
        /// </summary>
        public Dictionary<string, IComponent> Components
        {
            get;
        } = new Dictionary<string, IComponent>();

        /// <summary>
        /// A GUID uniquely identifying this particular entity instance during runtime
        /// </summary>
        public Guid UniqueID
        {
            get;
        } = Guid.NewGuid();     

        /// <summary>
        /// A string uniquely identifying the type this entity currently implements
        /// </summary>
        public string TypeName
        {
            get;
            set;
        } 

        /// <summary>
        /// Check if an entity contains the given component.
        /// </summary>
        /// <typeparam name="T">Type of component to check for</typeparam>
        /// <returns>Flag indicating the presence of the particular component</returns>
        public bool HasComponent<T>() where T : class, IComponent
        {
            return HasComponent(typeof(T));
        }
        
        /// <summary>
        /// Check if an entity contains the given component.
        /// </summary>
        /// <param name="ty">Type of component to check for</param>
        /// <returns>Flag indicating the presence of the particular component</returns>
        /// <exception cref="ArgumentException">If given type is not a component type</exception>
        public bool HasComponent(Type ty)
        {
            if(!ComponentManager.IsComponent(ty))
                throw new ArgumentException("Given type is not a component");
 
            // Retrieve all component types that derive from this type
            var types = ComponentManager.GetDerived(ty);

            // Check if any of the derived component types is registered with this entity type
            return Components.Keys.Intersect(types.Select(ComponentManager.GetComponentId)).Count() != 0;
        }

        /// <summary>
        /// Type of component to check for
        /// </summary>
        /// <param name="id">String uniquely identifying the requested component</param>
        /// <returns>Flag indicating the presence of the particular component</returns>
        public bool HasComponent(string id)
        {
            if(!ComponentManager.IsComponentKnown(id))
                throw new UnknownComponentException(String.Format("Id \"{0}\" does not refer to a valid component type", id));

            // We cant just check if the key exists in the dictionary, since there might be
            // derived component types that would not be found that way
            return HasComponent(ComponentManager.GetComponentType(id));
        }

        /// <summary>
        /// Retrieve component by generic type
        /// </summary>
        /// <typeparam name="T">Type of the component to retrieve</typeparam>
        /// <returns>Reference to the stored component</returns>
        public T GetComponent<T>() where T : class, IComponent
        {
            return GetComponent(typeof(T)) as T;
        }

        /// <summary>
        /// Retrieve component by type object
        /// </summary>
        /// <param name="ty">Type of the component to return</param>
        /// <returns>Reference to the stored component</returns>
        /// <exception cref="ArgumentException">If no component with given type is currently part of this entity</exception>
        public IComponent GetComponent(Type ty)
        {       
            if(!ComponentManager.IsComponent(ty))
                throw new ArgumentException("Given type is not a component");      

            // Even though we search for given type and all derived types, there
            // can always just be one result here, since HasComponent is checked
            // when a new entity is added.
            var types = ComponentManager.GetDerived(ty);
            var key = Components.Keys.Intersect(types.Select(ComponentManager.GetComponentId)).First();
            return Components[key];
        }

        /// <summary>
        /// Retrieve component by id
        /// </summary>
        /// <param name="id">Id of the component to return</param>
        /// <returns>Reference to the stored component</returns>
        /// <exception cref="ArgumentException">If no component with given ID is currently part of this entity</exception>
        public IComponent GetComponent(string id)
        {
            if(!HasComponent(id))
                throw new ArgumentException("Entity does not contain a component of given type");

            return GetComponent(ComponentManager.GetComponentType(id));
        }
    }
}