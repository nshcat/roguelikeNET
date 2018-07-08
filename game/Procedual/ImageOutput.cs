using System;
using game.Ascii;
using System.Drawing;
using System.IO;
using Color = System.Drawing.Color;

namespace game.Procedual
{
    /// <summary>
    /// Output map as grayscale heightmap image. The colors are automatically scaled in such
    /// manner that 0 represents the lowest value found in the data set, and 255 the highest
    /// </summary>
    public class ImageOutput : IMapGeneratorOutput
    {
        protected float floodLevel = 0.4f; //0.3f;
        protected float mountainLevel = 0.85f;

        protected Color landlow = Color.FromArgb(0, 64, 0);
        protected Color landhigh = Color.FromArgb(116, 182, 133);
        protected Color waterlow = Color.FromArgb(0, 0, 55);
        protected Color waterhigh = Color.FromArgb(0, 53, 106);
        protected Color mountlow = Color.FromArgb(147, 157, 167);
        protected Color mounthigh = Color.FromArgb(226, 223, 216);
        protected Color black = Color.Black;
        protected Color white = Color.White;


        protected string OutputPath { get; set; }
        protected string RandomName { get; set; }

        public ImageOutput(string path)
        {
            OutputPath = path;
            RandomName = Path.GetRandomFileName();
        }

        public void Apply(MapGeneratorState state)
        {
            // Empty map is not okay
            if (state.Dimensions.Equals(Dimensions.Empty))
                throw new ArgumentException("Cannot output empty map");

            try
            {
                GenerateHeightmap(state);
                GenerateColoredMap(state);
                GenerateTemperatureMap(state);
                //GenerateBiggerImages(state, 4);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected void GenerateTemperatureMap(MapGeneratorState state)
        {
            GenerateTemperatureMapFor(state.Dimensions, state.Temperature, "temperature");
        }

        protected void GenerateHeightmap(MapGeneratorState state)
        {
            GenerateHeightmapFor(state.Dimensions, state.Heightmap, "heightmap");
        }

        protected void GenerateColoredMap(MapGeneratorState state)
        {
            GenerateColoredMapFor(state.Dimensions, state.Heightmap, "colored");
        }

        protected void GenerateBiggerImages(MapGeneratorState state, int factor)
        {
            var newDims = new Dimensions((uint) (state.Dimensions.X * factor), (uint) (state.Dimensions.Y * factor));

            float[,] data = new float[newDims.X, newDims.Y];

            for (int ix = 0; ix < newDims.X; ++ix)
            {
                for (int iy = 0; iy < newDims.Y; ++iy)
                {
                    // Retrieve coordinates in relation to the original dimensions
                    float x = CommonMath.LMap(ix, 0, newDims.X - 1, 0, state.Dimensions.X - 1);
                    float y = CommonMath.LMap(iy, 0, newDims.Y - 1, 0, state.Dimensions.Y - 1);

                    // Fractional parts of the coordinates
                    float xfrac = (float) (x - Math.Floor(x));
                    float yfrac = (float) (y - Math.Floor(y));

                    // Integral parts of the coordinates
                    int xint = (int) Math.Floor(x);
                    int yint = (int) Math.Floor(y);

                    //Console.WriteLine($"{xint}, {yint}");

                    data[ix, iy] = (1 - xfrac) * ((1 - yfrac) * state.Heightmap[xint, yint] +
                                                  yfrac * state.Heightmap[xint,
                                                      Math.Min(yint + 1, state.Dimensions.Y - 1)]) +
                                   xfrac * ((1 - yfrac) *
                                            state.Heightmap[Math.Min(xint + 1, state.Dimensions.X - 1), yint] +
                                            yfrac * state.Heightmap[Math.Min(xint + 1, state.Dimensions.X - 1),
                                                Math.Min(yint + 1, state.Dimensions.Y - 1)]);

                }
            }

            GenerateHeightmapFor(newDims, data, "big_heightmap");
            GenerateColoredMapFor(newDims, data, "big_colored");
        }

        protected void GenerateHeightmapFor(Dimensions d, float[,] data, string postfix)
        {
            // Create new output image
            var img = new Bitmap((int) d.X, (int) d.Y);

            for (int ix = 0; ix < d.X; ++ix)
            {
                for (int iy = 0; iy < d.Y; ++iy)
                {
                    // Retrieve value at given position. Since the heightmap is normalized,
                    // we can just multiply with 255 (white) to generate a gray scale image.
                    var val = data[ix, iy];
                    var color = val * 255;

                    img.SetPixel(ix, iy, System.Drawing.Color.FromArgb(255, (int) color, (int) color, (int) color));
                }
            }

            // Save to disk
            img.Save(Path.Combine(OutputPath, "output", RandomName + "_" + postfix + ".png"));
        }

        protected void GenerateColoredMapFor(Dimensions d, float[,] data, string postfix)
        {
            // Find minimum and maximum
            float max = float.MinValue, min = float.MaxValue;

            for (int ix = 0; ix < d.X; ++ix)
            {
                for (int iy = 0; iy < d.Y; ++iy)
                {
                    float val = data[ix, iy];

                    if (val > max)
                        max = val;
                    if (val < min)
                        min = val;
                }
            }

            float diff = max - min;
            floodLevel *= diff;
            mountainLevel *= diff;


            // Create new output image
            var img = new Bitmap((int) d.X, (int) d.Y);

            for (int ix = 0; ix < d.X; ++ix)
            {
                for (int iy = 0; iy < d.Y; ++iy)
                {
                    // Retrieve value at given position. Since the heightmap is normalized,
                    // we can just multiply with 255 (white) to generate a gray scale image.
                    var val = data[ix, iy];

                    Color c;
                    
                    // Check if elevation is below flood line
                    if (val < floodLevel)
                        c = LerpColor(waterlow, waterhigh, val / floodLevel);
                    else if (val > mountainLevel) // Check if it is above mountain level
                        c = LerpColor(mountlow, mounthigh, (val - mountainLevel) / (diff - mountainLevel));
                    else
                        c = LerpColor(landlow, landhigh, (val - floodLevel) / (mountainLevel - floodLevel));
                    
                    img.SetPixel(ix, iy, c);
                }
            }

            // Save to disk
            img.Save(Path.Combine(OutputPath, "output", RandomName + "_" + postfix + ".png"));
        }
        
        protected void GenerateTemperatureMapFor(Dimensions d, float[,] data, string postfix)
        {
            // Create new output image
            var img = new Bitmap((int) d.X, (int) d.Y);

            for (int ix = 0; ix < d.X; ++ix)
            {
                for (int iy = 0; iy < d.Y; ++iy)
                {
                    // Retrieve value at given position.
                    var val = data[ix, iy];

                    Color c = LerpColor(Color.Blue, Color.Red, val);
                    
                    img.SetPixel(ix, iy, c);
                }
            }

            // Save to disk
            img.Save(Path.Combine(OutputPath, "output", RandomName + "_" + postfix + ".png"));
        }

        protected Color LerpColor(Color a, Color b, float x)
        {
            return Color.FromArgb(
                (int)(a.A * (1.0f - x) + b.A * x),
                (int)(a.R * (1.0f - x) + b.R * x),
                (int)(a.G * (1.0f - x) + b.G * x),
                (int)(a.B * (1.0f - x) + b.B * x)
            );
        }
    }
}