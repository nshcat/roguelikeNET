using System;
using System.Collections.Generic;
using System.IO;
using game.Ascii;
using game.AutoJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game
{
    /// <summary>
    /// A static class managing the various terrain types that can be used by game maps.
    /// </summary>
    public static class TerrainTypeManager
    {
        /// <summary>
        /// All known terrain type instances. The unique ID is used as a key to allow
        /// faster look-up.
        /// </summary>
        private static Dictionary<string, TerrainType> _terrainTypes
            = new Dictionary<string, TerrainType>();

        /// <summary>
        /// All known terrain type instances. The unique ID is used as a key to allow
        /// faster look-up. This is a read-only wrapper, available to users outside of this
        /// class.
        /// </summary>
        public static IReadOnlyDictionary<string, TerrainType> TerrainTypes => _terrainTypes;
        
        /// <summary>
        /// Initialize the terrain type manager.
        /// </summary>
        public static void Initialize()
        {        
            LoadTypes();
        }

        /// <summary>
        /// Retrieve the terrain type instance associated with given unique id.
        /// If no appropiate terrain type instance exists, the placeholder type is
        /// returned instead.
        /// </summary>
        /// <param name="id">Unique string identifier to use for lookup</param>
        /// <returns>Corresponding terrain type instance if exists, placeholder otherwise</returns>
        public static TerrainType GetType(string id)
        {
            if (_terrainTypes.ContainsKey(id))
                return _terrainTypes[id];
            else return TerrainType.Placeholder;
        }

        /// <summary>
        /// Check if a terrain type with given unique identifier is known.
        /// </summary>
        /// <param name="id">Unique string identifier to check for</param>
        /// <returns>Flag indicating presence of a corresponding terrain type instance for given identifier</returns>
        public static bool HasType(string id)
        {
            return _terrainTypes.ContainsKey(id);
        }

        /// <summary>
        /// Search asset subfolder for JSON documents and try to interpret
        /// them as terrain type descriptions
        /// </summary>
        private static void LoadTypes()
        {
            // Retrieve asset path and build terrain type database path
            var path = Path.Combine(Paths.DataDirectory, "json", "terrain");

            // Inspect every JSON document and parse
            foreach (var file in Directory.GetFiles(path, "*.json", SearchOption.AllDirectories))
            {
                // File is a full path to the particular document    
                try
                {
                    // Parse JSON file
                    using (var fileReader = File.OpenText(file))
                    using (var jsonReader = new JsonTextReader(fileReader))
                    {
                        // Since we want to allow multiple terrain definitions in a single file, we require
                        // the top level structure to be an array.
                        var dataSource = JArray.ReadFrom(jsonReader) as JArray;

                        // Loop through all terrain type definitions. They have to be objects.
                        foreach (var entry in dataSource)
                        {
                            // Ignore entries that are not objects
                            if (entry.Type != JTokenType.Object)
                            {
                                Logger.PostMessageTagged(SeverityLevel.Warning, "TerrainTypeManager",
                                    $"Ignoring non-object entry in terrain definition file \"{Path.GetFileName(file)}\"");
                                
                                continue;
                            }
                            
                            // Deserialize terrain type instance from the JSON entry
                            var instance = JsonLoader.Deserialize<TerrainType>(entry as JObject);
                            
                            _terrainTypes.Add(instance.Identifier, instance);
                        }
                    } 
                }
                catch (Exception e)
                {
                    Logger.PostMessageTagged(SeverityLevel.Fatal, "TerrainTypeManager",
                        String.Format("Failed to load terrain type definition file \"{0}\": {1}", Path.GetFileName(file), e.Message));

                    throw;
                }
            }
            
            Logger.PostMessageTagged(SeverityLevel.Debug, "TerrainTypeManager",
                $"Loaded {_terrainTypes.Count} terrain types");
        }
    }
}