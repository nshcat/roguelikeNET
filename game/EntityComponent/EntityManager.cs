using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Query result containing all entities. This can be used as a starting point to build chained queries.
        /// </summary>
        public static EntityQueryResult AllEntities => new EntityQueryResult(Entities.Values);     

        /// <summary>
        /// Initialize the entity manager. This has to be called before any other method can be used.
        /// </summary>
        public static void Initialize()
        {
            ComponentManager.Initialize();
            LoadEntities();
            CheckDependencies();
        }

        /// <summary>
        /// Get all entities that contain a component of given type that satisfies the given predicate.
        /// Abstract component types are allowed here and result in all entities with derived component types
        /// to get chosen.
        /// </summary>
        /// <param name="predicate">Predicate operating on component instance</param>
        /// <typeparam name="T">Type of the component to filter by</typeparam>
        /// <returns>Collection of entities as result of filter operation</returns>
        [Obsolete("Please perform queries on AllEntities")]
        public static IEnumerable<Entity> GetEntities<T>(Func<T, bool> predicate) where T : class, IComponent
        {
            return Entities
                .Select(x => x.Value)
                .Where(x => x.HasComponent<T>())
                .Where(x => predicate(x.GetComponent<T>()))
                .ToList();
        }
        
        /// <summary>
        /// Get all entities that contain a component of given type
        /// </summary>
        /// <typeparam name="T">Type of the component to filter by</typeparam>
        /// <returns>Collection of entities as result of filter operation</returns>
        [Obsolete("Please perform queries on AllEntities")]
        public static IEnumerable<Entity> GetEntities<T>() where T : class, IComponent
        {
            return GetEntities<T>(_ => true);
        }

        /// <summary>
        /// Determine all entity types that contain a component with given type
        /// </summary>
        /// <typeparam name="T">Type of component to filter by</typeparam>
        /// <returns>Collection of entity type infos</returns>
        public static IEnumerable<EntityTypeInfo> GetTypes<T>() where T : class, IComponent
        {
            // Retrieve component type names. Since T could be an abstract type or a type
            // which has derived types, we need to collect all component type names that
            // are derived from this one.
            var names = ComponentManager.GetDerived(typeof(T));
            
            // Filter all known entity types. Disregard templates.
            return TypeInfos
                .Values
                .Where(x => x.Components.Intersect(names.Select(ComponentManager.GetComponentId)).Count() != 0)
                .Where(x => !x.IsTemplate);
        }

        /// <summary>
        /// Construct a temporary instance for all entity types that contain a component of given type
        /// </summary>
        /// <typeparam name="T">Type of component to filter by</typeparam>
        /// <returns>Collection of temporary entity instances</returns>
        public static IEnumerable<Entity> ConstructTemporaries<T>() where T : class, IComponent
        {
            return GetTypes<T>().Select(x => Construct(x.Name));
        }
     
        /// <summary>
        /// Construct an entity of given type
        /// </summary>
        /// <param name="type">Name of the type of the new entity</param>
        /// <returns>Constructed entity</returns>
        /// <exception cref="UnknownEntityTypeException">If the given entity type is not valid</exception>
        public static Entity Construct(string type)
        {
            // Construct entity
            var entity = ConstructInternal(type);
            
            // This entity is requested to be globally managed, so
            // register it with this manager
            Entities.Add(entity.UniqueID, entity);

            return entity;
        }
        
        /// <summary>
        /// Construct a temporary entity of given type. Temporary entities are not registered
        /// with the global entity manager can thus not get queried. Their use is for data retrieval
        /// only.
        /// </summary>
        /// <param name="type">Name of the type of the new, temporary entity</param>
        /// <returns>Constructed entity</returns>
        /// <exception cref="UnknownEntityTypeException">If the given entity type is not valid</exception>
        public static Entity ConstructTemporary(string type)
        {
            // The internal implementation method does everything we need
            return ConstructInternal(type);
        }
        
        
        /// <summary>
        /// Internal implementation of entity construction. Construct entity with given type name.
        /// This will do recursive calls to process the potentially multi-leveled inheritance tree.
        /// </summary>
        /// <param name="type">Type of the entity to construct</param>
        /// <returns></returns>
        /// <exception cref="UnknownEntityTypeException">If the given entity type is not valid</exception>
        private static Entity ConstructInternal(string type)
        {
            // Check if the type actually exists
            if (!HasEntityType(type))
            {
                Logger.postMessage(SeverityLevel.Fatal, "EntityManager", String.Format("Unknown entity type \"{0}\"", type));
                throw new UnknownEntityTypeException();
            }
            
            // Retrieve entity type information object
            var info = TypeInfos[type];
            
            // Check if constructing entities of this type is actually allowed
            if (info.IsTemplate)
            {
                Logger.postMessage(SeverityLevel.Fatal, "EntityManager",
                    String.Format("Entities of type \"{0}\" cannot be constructed, since type was defined to be a template", type));
                throw new UnknownEntityTypeException();
            }

            // Construct empty Entity
            var entity = new Entity();
            
            // Set type name
            entity.TypeName = type;

            // Populate with components from type info and those inherited from base types
            ConstructInternal(entity, info); 
            
            return entity;
        }

        /// <summary>
        /// Internal implementation of entity construction. Populates entity components with those
        /// associated with given entity type info object, and then continues recursively for any
        /// base types present in the type.
        /// </summary>
        /// <param name="e">Entity to populate</param>
        /// <param name="type">Entity type info object to retrieve component types from</param>
        /// <exception cref="EntityDependencyException">If a component of a given type already exists in the entity.</exception>
        /// <exception cref="Exception">If component construction fails</exception>
        private static void ConstructInternal(Entity e, EntityTypeInfo type)
        {
            // Retrieve entity type JSON node. This is safe since we checked that
            // the type is actually known to us beforehand.
            var obj = JsonObjectCache[type.Name];
            
            // Initialize all components
            foreach (var currentComponent in type.Components)
            {
                // Retrieve component type object
                var componentType = ComponentManager.GetComponentType(currentComponent);
                
                // Check if an instance of that component type already exists in the currently constructed
                // entity. This could indicate a circular dependency
                if (e.HasComponent(componentType))
                {
                    Logger.postMessage(SeverityLevel.Fatal, "EntityManager",
                        String.Format("Found duplicate component type while constructing entity of type \"{0}\": \"{1}\"", e.TypeName, currentComponent));
                    
                    Logger.postMessage(SeverityLevel.Info, "EntityManager", "This could indicate a circular dependency in the entity definition");
                    
                    throw new EntityDependencyException("Multiple components of same type detected");
                }
                
                // Retrieve JSON subobject corresponding to current component
                var subObject = obj[currentComponent] as JObject;                        
                
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
                e.AddComponent(currentComponent, component);
            }
            
            // Do the same with all registered base entities/templates
            foreach (var baseType in type.Bases)
            {
                // We know that this type exists, since we called CheckDependencies earlier.
                ConstructInternal(e, TypeInfos[baseType]);
            }
        }

        /// <summary>
        /// Check if an entity type with given string identifier is known.
        /// </summary>
        /// <param name="type">String identifier to check for</param>
        /// <returns>Flag indicating presence of an entity type with given identifier</returns>
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

        /// <summary>
        /// Check that all template dependencies are actually valid
        /// </summary>
        private static void CheckDependencies()
        {
            foreach (var type in TypeInfos.Values)
            {
                foreach (var dependency in type.Bases)
                {
                    if (!HasEntityType(dependency))
                    {
                        Logger.postMessage(SeverityLevel.Fatal, "EntityManager",
                            String.Format("Entity type \"{0}\" depends on invalid entity/template type \"{1}\"", type.Name, dependency));
                        
                        throw new EntityDependencyException();
                    }
                }       
            }
        }
    }
}