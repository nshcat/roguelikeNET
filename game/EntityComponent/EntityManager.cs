using System;
using System.Collections.Generic;
using System.IO;
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
        /// Initialize the entity manager. This has to be called before any other method can be used.
        /// </summary>
        public static void Initialize()
        {
            ComponentManager.Initialize();
            LoadEntities();
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