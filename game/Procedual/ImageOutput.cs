using System;
using game.Ascii;
using System.Drawing;
using System.IO;

namespace game.Procedual
{
    /// <summary>
    /// Output map as grayscale heightmap image. The colors are automatically scaled in such
    /// manner that 0 represents the lowest value found in the data set, and 255 the highest
    /// </summary>
    public class ImageOutput : IMapGeneratorOutput
    {
        protected string OutputPath { get; set; }

        public ImageOutput(string path)
        {
            OutputPath = path;
        }

        public void Apply(MapGeneratorState state)
        {
            // Empty map is not okay
            if (state.Dimensions.Equals(Dimensions.Empty))
                throw new ArgumentException("Cannot output empty map");


            // Create new output image
            var img = new Bitmap((int) state.Dimensions.X, (int) state.Dimensions.Y);

            for (int ix = 0; ix < state.Dimensions.X; ++ix)
            {
                for (int iy = 0; iy < state.Dimensions.Y; ++iy)
                {
                    // Retrieve value at given position. Since the heightmap is normalized,
                    // we can just multiply with 255 (white) to generate a gray scale image.
                    var val = state.Heightmap[ix, iy];
                    var color = val* 255;

                    img.SetPixel(ix, iy, System.Drawing.Color.FromArgb(255, (int)color, (int)color, (int)color));
                }
            }

            // Save to disk
            img.Save(Path.Combine(OutputPath, "heightmap.png"));
        }
    }
}