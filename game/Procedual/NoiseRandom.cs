using System;

namespace game.Procedual
{
    public class NoiseRandom
    {
        protected int[] Permutation
        {
            get;
            set;
        }

        public NoiseRandom(int seed)
        {
            Permutation = new int[256];
            
            // Initialize to identity permuation
            for (int i = 0; i < 256; ++i)
                Permutation[i] = i;
            
            // Randomize it
            var rnd = new Random(seed);
            for (int i = 0; i < 256; ++i)
            {
                int j = Permutation[i];
                int k = rnd.Next() & 255;
                Permutation[i] = Permutation[k];
                Permutation[k] = j;
            }
        }

        public int Next(int x, int y)
        {
            return Permutation[(x + Permutation[y & 255]) & 255] & 7;
        }

        public float Next(int x, int y, int z)
        {
            int r1 = Permutation[(x + Permutation[(y + Permutation[z & 255]) & 255]) & 255];

            return ((float) r1 / 256.0f);
        }
    }
}