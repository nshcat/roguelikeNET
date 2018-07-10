using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using game.Ascii;

namespace game.EntityComponent
{
    /// <summary>
    /// Helper class that provides utility methods based on reflection that work
    /// on component types
    /// </summary>
    public static class ComponentManager
    {
        /// <summary>
        /// Cache storing all types that derive from IComponent.
        /// </summary>
        private static Dictionary<string, Type> ComponentTypes
        {
            get;
        } = new Dictionary<string, Type>();

        /// <summary>
        /// Initialize component manager.
        /// </summary>
        public static void Initialize()
        {
            // Retrieve a list of all types that inherit from IComponent
            var components = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                from assemblyType in domainAssembly.GetTypes()
                where typeof(IComponent).IsAssignableFrom(assemblyType) && typeof(IComponent) != assemblyType
                select assemblyType).ToArray();

            foreach (var t in components)
            {  
                // Retrieve identifier string. It is a static property.
                var id = t.GetProperty("Identifier",  BindingFlags.Static | BindingFlags.Public).GetValue(null) as string;
                
                // Insert type into cache
                ComponentTypes.Add(id, t);
                
                Logger.postMessage(SeverityLevel.Debug, "ComponentManager", String.Format("Found component \"{0}\"", id));
            }
        }
        
        /// <summary>
        /// Check if a component with given id exists
        /// </summary>
        /// <param name="id">String ID to check for</param>
        /// <returns>Flag indicating if a component type with given id exists</returns>
        public static bool IsComponentKnown(string id)
        {
            return ComponentTypes.ContainsKey(id);
        }

        /// <summary>
        /// Retrieve runtime type object of component with given id
        /// </summary>
        /// <param name="id">Id of component type to retrieve</param>
        /// <returns>Component type object</returns>
        /// <exception cref="UnknownComponentException">If no component with given id exists</exception>
        public static Type GetComponentType(string id)
        {
            if(!IsComponentKnown(id))
                throw new UnknownComponentException("No component known with id \"" + id + "\"");

            return ComponentTypes[id];
        }

        /// <summary>
        /// Checks whether a given type is a component
        /// </summary>
        /// <param name="t">Type to check</param>
        /// <returns>Flag indicating if given type is a component</returns>
        public static bool IsComponent(Type t)
        {
            return typeof(IComponent).IsAssignableFrom(t) && typeof(IComponent) != t;
        }

        
        /// <summary>
        /// Retrieves unique id from given component type
        /// </summary>
        /// <param name="t">Type to extract ID from</param>
        /// <returns>Unique id for given component type</returns>
        public static string GetComponentId(Type t)
        {
            if(!IsComponent(t))
                throw new ArgumentException("Given type is not a component");
            
            return t.GetProperty("Identifier",  BindingFlags.Static | BindingFlags.Public).GetValue(null) as string;
        }
    }
}