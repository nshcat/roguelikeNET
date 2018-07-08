using System;

namespace game.Procedual
{
    // TODO: Add bias (on the poles, there is a bias torwards cold climate,
    // and on the equator there is a bias torwards warm climate)
    // TODO: Tweak frequency a bit
    public class TemperaturePhase
        : IMapGeneratorPhase
    {
        private static readonly int SeedOffset = 33;
        
        private static float gain = 0.65f;
        private static float lacunarity = 2.0f;
        
        public void Apply(MapGeneratorState state)
        {
            // === Generate temperature data

            // RNG used by the noise generator
            var rng = new NoiseRandom(state.Seed*2 + SeedOffset);

            // Initialize extrema
            float max = float.MinValue;
            float min = float.MaxValue;

            // Create noise object
            var noise = new SimplexNoise(rng);

            for (int ix = 0; ix < state.Dimensions.X; ++ix)
            {
                for (int iy = 0; iy < state.Dimensions.Y; ++iy)
                {
                    float frequency = 1.0f / (float) state.Dimensions.X;
                    float amplitude = 90.0f;

                    // The total value of noise at this position
                    float total = 0.0f;

                    // Big cell noise for fundamental features
                    for (int i = 0; i < 2; ++i)
                    {
                        // Create variation between different octaves
                        float offset = (float) i * 7.0f;

                        total += noise.ValueAt((float) ix * frequency + offset, (float) iy * frequency + offset) *
                                 amplitude;

                        // Adjust frequency and amplitude for later runs
                        frequency *= lacunarity;
                        amplitude *= gain;
                    }

                    // Save value
                    state.Temperature[ix, iy] = total;

                    // Check if new value is in fact a new extreme value
                    if (total > max)
                        max = total;
                    if (total < min)
                        min = total;
                }
            }


            // === Normalize data
            float dif = max - min;
            for (int ix = 0; ix < state.Dimensions.X; ++ix)
            {
                for (int iy = 0; iy < state.Dimensions.Y; ++iy)
                {
                    state.Temperature[ix, iy] = (state.Temperature[ix, iy] - min) / dif;
                }
            }
        }

        public string Label()
        {
            return "Generating temperature map";
        }
    }
}