namespace game.Procedual
{
    public class ThermalErosionPhase : IMapGeneratorPhase
    {
        protected int Iterations
        {
            get;
            set;
        }
        
        public ThermalErosionPhase(int iterations)
        {
            Iterations = iterations;
        }
        
        public void Apply(MapGeneratorState state)
        {
            int lowestX = 0, lowestY = 0;

            float talus = 4.0f / (float) state.Dimensions.X;
            float currentDifference, currentHeight, maxDif, newHeight;


            for (int it = 0; it < Iterations; ++it)
            {
                // For all pixels, except the border
                for (int ix = 1; ix < (state.Dimensions.X - 1); ++ix)
                {
                    for (int iy = 1; iy < (state.Dimensions.Y - 1); ++iy)
                    {
                        currentHeight = state.Heightmap[ix, iy];
                        maxDif = float.MinValue;

                        for (int i = -1; i < 2; i += 2)
                        {
                            for (int j = -1; j < 2; j += 2)
                            {
                                currentDifference = currentHeight - state.Heightmap[ix + i, iy + j];

                                if (currentDifference > maxDif)
                                {
                                    maxDif = currentDifference;

                                    lowestX = i;
                                    lowestY = j;
                                }
                            }
                        }

                        if (maxDif > talus)
                        {
                            newHeight = currentHeight - maxDif / 2.0f;

                            state.Heightmap[ix, iy] = newHeight;
                            state.Heightmap[ix + lowestX, iy + lowestY] = newHeight;
                        }
                    }
                }
            }
        }

        public string Label()
        {
            return "Applying thermal erosion";
        }
    }
}