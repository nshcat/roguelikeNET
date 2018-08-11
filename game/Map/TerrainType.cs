using System;
using game.Ascii;
using game.AutoJson;
using Newtonsoft.Json.Linq;

namespace game
{
    [Deserializable]
    public class TerrainType
    {
        /// <summary>
        /// The unique string identifier of this terrain type.
        /// </summary>
        [AutoJson.Key("id")]
        [Required]
        public string Identifier
        {
            get;
            protected set;
        }

        /// <summary>
        /// The rough category this terrain type falls into. See <see cref="TerrainCategory"/> for more
        /// information.
        /// </summary>
        [AutoJson.Key("category")]
        [DefaultValue(TerrainCategory.Passable)]
        public TerrainCategory Category
        {
            get;
            protected set;
        }

        /// <summary>
        /// The tile to be drawn to represent this type.
        /// </summary>
        /// <remarks>
        /// This is not used if <see cref="IsVaried"/> is true.
        /// </remarks>
        [AutoJson.Key("tile")]
        public Tile Tile
        {
            get;
            protected set;
        }

        /// <summary>
        /// A short text describing this terrain type.
        /// </summary>
        [AutoJson.Key("description")]
        [DefaultValue("")]
        public string Description
        {
            get;
            protected set;
        }

        /// <summary>
        /// A pretty-printed name of the entity type. In contrast to <see cref="Identifier"/>, this does not
        /// have to be unique.
        /// </summary>
        [AutoJson.Key("name")]
        [Required]
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Whether this terrain type uses a palette of different tiles in order to
        /// appear varied.
        /// </summary>
        /// <remarks>
        /// If this is true, the JSON document has to supply a palette of tile information.
        /// </remarks>
        [AutoJson.Key("is_varied")]
        [DefaultValue(false)]
        public bool IsVaried
        {
            get;
            protected set;
        }

        /// <summary>
        /// The palette to use if <see cref="IsVaried"/> is set to true. Otherwise
        /// this will be null.
        /// </summary>
        public WeightedDistribution<Tile> Palette
        {
            get;
            protected set;
        }

        /// <summary>
        /// A special terrain type used as a placeholder if a referenced type does not exist
        /// </summary>
        /// <remarks>
        /// This could be used when a map is loaded from a save file that contains terrain types
        /// that have since been removed from the asset files.
        /// </remarks>
        public static TerrainType Placeholder =>
            new TerrainType
            {
                Identifier = "unknown_type",
                Name = "Unknown Terrain Type",
                Category = TerrainCategory.Wall,
                Description = "The requested terrain type for this grid cell could not be loaded",
                IsVaried = false,
                Tile = new Tile(Color.Black, Color.Red, 63)
            };


        /// <summary>
        /// Do additional work after bulk of deserialization is done. If the type is marked
        /// as using varied tiles, the palette has to be deserialized to a weighted distribution.
        /// </summary>
        /// <param name="obj">Source JSON document</param>
        /// <exception cref="ArgumentException">If JSON document format is invalid</exception>
        [AfterDeserialization]
        private void Deserialize(JObject obj)
        {
            // Populate palette if needed
            if (IsVaried)
            {
                try
                {
                    Palette = new WeightedDistribution<Tile>();
                    
                    foreach (var token in obj["palette"] as JArray)
                    {
                        if(token.Type != JTokenType.Object)
                            throw new ArgumentException("Expected JSON object in palette array");

                        var entry = token as JObject;

                        var probability = entry["probability"].Value<double>();
                        var tile = JsonLoader.Deserialize<Tile>(entry["tile"] as JObject);
                        
                        Palette.Add(probability, tile);
                    }
                }
                catch (Exception e)
                {
                    Logger.PostMessageTagged(SeverityLevel.Fatal, "TerrainType", $"Failed to load terrain type \"{Identifier}\": {e.Message}");
                    throw;
                }
            }
            else if(Tile == null) // If IsVaried is false, the document has to supply tile information.
            {               
                Logger.PostMessageTagged(SeverityLevel.Fatal, "TerrainType", $"Terrain type \"{Identifier}\" is not set to use varied tiles, but has no tile defined");
                throw new ArgumentException($"Terrain type \"{Identifier}\" is not set to use varied tiles, but has no tile defined");
            }
        }
    }
}