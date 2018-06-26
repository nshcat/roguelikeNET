namespace game.Procedual
{
    public class ElevationPhase : IMapGeneratorPhase
    {
        // TODO move to config file, section "Mapgen"
        protected int octaves = 5;
        protected float gain = 0.65f;
        protected float lacunarity = 2.0f;

        public void Apply(MapGeneratorState state)
        {
            // Create special random number generator that will be used
            // in noise calculations
            var rng = new NoiseRandom(state.Seed);

            // Setup extrema to inital values
            state.Maximum = float.MinValue;
            state.Minimum = float.MaxValue;

            // Create Noise objects
            var simplex = new SimplexNoise(rng);
            var cell = new CellNoise(rng);

            for (int ix = 0; ix < state.Dimensions.X; ++ix)
            {
                for (int iy = 0; iy < state.Dimensions.Y; ++iy)
                {
                    float totalCell = 0.0f;
                    float totalSimp = 0.0f;

                    float frequency = 4.0f / (float) state.Dimensions.X;
                    float amplitude = 1.0f;

                    // Big cell noise for fundamental features
                    for (int i = 0; i < 2; ++i)
                    {
                        // Create variation between different octaves
                        float offset = (float) i * 7.0f;

                        totalCell += cell.ValueAt((float) ix * frequency + offset, (float) iy * frequency + offset) *
                                     amplitude;

                        // Adjust frequency and amplitude for later runs
                        frequency *= lacunarity;
                        amplitude *= gain;
                    }

                    // Simplex noise for smaller features
                    amplitude *= 30.0f;

                    for (int i = 2; i < octaves; ++i)
                    {
                        float offset = (float) i * 7.0f;

                        totalSimp += simplex.ValueAt((float) ix * frequency + offset, (float) iy * frequency + offset) *
                                     amplitude;

                        frequency *= lacunarity;
                        amplitude *= gain;
                    }

                    // Save value
                    float total = totalCell + totalSimp;
                    state.Heightmap[ix, iy] = total;

                    if (total > state.Maximum)
                        state.Maximum = total;
                    if (total < state.Minimum)
                        state.Minimum = total;
                }
            }

            // Normalize all values in height map to [0, 1]
            float dif = state.Maximum - state.Minimum;
            for (int ix = 0; ix < state.Dimensions.X; ++ix)
            {
                for (int iy = 0; iy < state.Dimensions.Y; ++iy)
                {
                    state.Heightmap[ix, iy] = (state.Heightmap[ix, iy] - state.Minimum) / dif;
                }
            }
        }

        public string Label()
        {
            return "Generating terrain";
        }
    }
}