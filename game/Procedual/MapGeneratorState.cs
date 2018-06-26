using game.Ascii;

namespace game.Procedual
{
    public class MapGeneratorState
    {
        public int Seed
        {
            get;
            protected set;
        }
        
        public Dimensions Dimensions
        {
            get;
            protected set;
        }

        /// <summary>
        /// The maximal absolute elevation value found in the non-normalized map.
        /// </summary>
        public float Maximum
        {
            get;
            set;
        }

        
        /// <summary>
        /// The minimal absolute elevation value found in the non-normalized map.
        /// </summary>
        public float Minimum
        {
            get;
            set;
        }

        /// <summary>
        /// A two dimensional map containing height information for each map entry.
        /// All values are normalized, which means they are in the interval [0, 1].
        /// </summary>
        public float[,] Heightmap
        {
            get;
            protected set;
        }

        public MapGeneratorState(Dimensions d, int seed)
        {
            Seed = seed;
            Dimensions = d;
            Heightmap = new float[d.X, d.Y];
        }
    }
}