using System;
using System.Collections.Generic;
using System.IO;
using SadRex;

namespace game.Ascii
{
    public static class TileImageLoader
    {
        /// <summary>
        /// Cache used to avoid repeated construction of tile image instances from rex paint file.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, TileImage> cache = new Dictionary<string, TileImage>();

        /// <summary>
        /// Load tile image from given asset file.
        /// </summary>
        /// <param name="name">The name of the source assert file, located in the folder "images"</param>
        /// <returns>TileImage instance with data corresponding the the asset files contents</returns>
        public static TileImage LoadImage(string name)
        {
            // If image was not already loaded, we need to load it from the asset file.
            if (!cache.ContainsKey(name))
            {
                // Build path to resource
                var path = Path.Combine(Paths.DataDirectory, "images", Path.ChangeExtension(name, ".xp"));

                FileStream file;
                try
                {
                    // Open file stream. We only need read access.
                    file = File.Open(path, FileMode.Open, FileAccess.Read);
                }
                catch (Exception e)
                {
                    Logger.PostMessageTagged(SeverityLevel.Fatal, "TileImageLoader", String.Format("Could not load asset file \"{0}\": {1}", name, e.Message));
                    throw;
                }

                // Parse RexPaint image using SadRex.
                var rawImage = Image.Load(file);
                
                // Create data buffer for our image
                var buffer = new Tile[rawImage.Height, rawImage.Width];
                
                // At least one layer needs to be present.
                if (rawImage.LayerCount <= 0)
                {
                    Logger.PostMessageTagged(SeverityLevel.Fatal, "TileImageLoader", String.Format("Requested TileImage asset \"{0}\" does not contain any layers", name));
                    throw new ArgumentException();
                }
                
                if (rawImage.LayerCount > 1) // Warn about ignored layers
                {
                    Logger.PostMessageTagged(SeverityLevel.Warning, "TileImageLoader", String.Format("Requested TileImage asset \"{0}\" contains extra layers which are ignored", name));
                }

                // This is safe to do now, because we checked the amount of layers.
                var layer = rawImage.Layers[0];
                
                // Copy data to buffer
                for (int iy = 0; iy < rawImage.Height; ++iy)
                {
                    for (int ix = 0; ix < rawImage.Width; ++ix)
                    {
                        // Retrieve cell at this position in the raw image
                        var cell = layer[ix, iy];
                        
                        // Create tile of our own type
                        var t = new Tile(
                                new Color(cell.Background.R, cell.Background.G, cell.Background.B),
                                new Color(cell.Foreground.R, cell.Foreground.G, cell.Foreground.B),
                                (byte)cell.Character
                            );
                        
                        // Store it in the image buffer
                        buffer[iy, ix] = t;
                    }
                }
                
                // Store image in cache for later retrieval
                cache.Add(name, new TileImage(buffer));
            }
            
            // The image is stored in the cache and can be cloned and returned.
            // We clone the image in order to allow the user to modify the image at will.
            return (TileImage) cache[name].Clone();
        }
    }
}