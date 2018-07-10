using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using game.Ascii;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game.EntityComponent
{
    public static class EntityManager
    {
        /// <summary>
        /// A dictionary used to cache JSON documents that describe entity types
        /// </summary>
        private static Dictionary<string, JObject> JsonObjectCache
        {
            get;
        } = new Dictionary<string, JObject>();

        /// <summary>
        /// A dictionary used to store all component names associated with given entity type name.
        /// This can be used to retrieve all entity types that have certain components.
        /// </summary>
        private static Dictionary<string, EntityTypeInfo> TypeInfos
        {
            get;
        } = new Dictionary<string, EntityTypeInfo>();

        /// <summary>
        /// Collection of all known entities.
        /// </summary>
        private static Dictionary<Guid, Entity> Entities
        {
            get;
        } = new Dictionary<Guid, Entity>();
        

        /// <summary>
        /// Initialize the entity manager. This has to be called before any other method can be used.
        /// </summary>
        public static void Initialize()
        {
            ComponentManager.Initialize();
            LoadEntities();
        }

        /// <summary>
        /// Construct an entity of given type
        /// </summary>
        /// <param name="type">Name of the type of the new entity</param>
        /// <returns>Constructed entity</returns>
        public static Entity Construct(string type)
        {
            // Check if the type actually exists
            if (!HasEntityType(type))
            {
                Logger.postMessage(SeverityLevel.Fatal, "EntityManager", String.Format("Unknown entity type \"{0}\"", type));
                throw new UnknownEntityTypeException();
            }
            
            // Construct empty Entity
            var entity = new Entity();

            // Retrieve entity type information object
            var info = TypeInfos[type];

            // Retrieve entity type JSON node
            var obj = JsonObjectCache[type];
            
            // Initialize all components
            foreach (var currentComponent in info.Components)
            {
                // Retrieve JSON subobject corresponding to current component
                var subObject = obj[currentComponent] as JObject;
                
                // Retrieve component type object
                var componentType = ComponentManager.GetComponentType(currentComponent);
                
                // Construct empty component instance
                var component = Activator.CreateInstance(componentType) as IComponent;
                
                // Check for possible failure
                if (component == null)
                {
                    Logger.postMessage(SeverityLevel.Fatal, "EntityManager", String.Format("Could not create instance of component \"{0}\"", currentComponent));
                    throw new Exception(String.Format("Could not create instance of component \"{0}\"", currentComponent));
                }

                // Parse JSON subobject
                component.Construct(subObject);
             
                // Add it to entity
                entity.Components.Add(currentComponent, component);
            }
            
            return entity;
        }

        public static bool HasEntityType(string type)
        {
            return TypeInfos.ContainsKey(type);
        }

        
        /// <summary>
        /// Destroy entity with given ID. This will cause the entity to get removed from the global
        /// entity collection, which means that it will be garbage collected eventually, given that
        /// no other references to it exist.
        /// </summary>
        /// <param name="id">Id of the entity to destroy</param>
        public static void Destroy(Guid id)
        {
            if(!HasEntity(id))
                Logger.postMessage(SeverityLevel.Warning, "EntityManager", String.Format("Tried to destroy non-existing entity with id \"{0}\"", id));

            Entities.Remove(id);
        }

        /// <summary>
        /// Check if there is a known entity with given ID.
        /// </summary>
        /// <param name="id">ID to check for</param>
        /// <returns>Flag indicating if entity exists with given ID</returns>
        public static bool HasEntity(Guid id)
        {
            return Entities.ContainsKey(id);
        }
        
        /// <summary>
        /// Load all entity definitions
        /// </summary>
        private static void LoadEntities()
        {
            // Retrieve asset path and build entity database path
            var path = Path.Combine(Paths.DataDirectory, "json", "entities");

            // Inspect every JSON document and save in cache
            foreach (var file in Directory.GetFiles(path, "*.json", SearchOption.AllDirectories))
            {
                // File is a full path to the particular document    
                try
                {
                    // Parse JSON file
                    using(var fileReader = File.OpenText(file))
                    using (var jsonReader = new JsonTextReader(fileReader))
                    {
                        // Since we want to allow multiple entity definitions in a single file, we require
                        // the top level structure to be an array.
                        var dataSource = JArray.ReadFrom(jsonReader) as JArray;
                        
                        // Loop through all entity definitions. They have to be objects.
                        foreach (var entry in dataSource)
                        {
                            if(entry.Type != JTokenType.Object)
                                continue;
                            
                            // Found an object.
                            var currentObject = entry as JObject;
                            
                            // Try to create entity type info object and store in cache
                            var info = new EntityTypeInfo(currentObject);
                            TypeInfos.Add(info.Name, info);
                            
                            // Store JSON object in cache for later use
                            JsonObjectCache.Add(info.Name, currentObject);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.postMessage(SeverityLevel.Fatal, "EntityManager",
                        String.Format("Failed to load entity definition file \"{0}\": {1}", Path.GetFileName(file), e.Message));
                    
                    throw;
                }
            }
        }
    }
}