using System;
using game.Ascii;

namespace game
{
    public class TestMap : IMap
    {
        public TestMap()
        {
            var rnd = new Random();

            for (int ix = 0; ix < 20; ++ix)
            {
                for (int iy = 0; iy < 20; ++iy)
                {
                    _randomMap[ix, iy] = rnd.NextDouble();
                }
            }
        }
        
        private double[,] _randomMap = new double[20, 20];
        
        public double GetPositionHash(Position p)
        {
            return _randomMap[p.X, p.Y];
        }

        public string CurrentLevel { get; set; } = "l1";

        public Dimensions Dimensions
        {
            get;
            set;
        } = new Dimensions(20, 20);

        public TerrainType this[string level, int x, int y]
        {
            get
            {
                string s = "";

                if (x < 2 || x >= 18)
                    s = "test_wall";
                else if (y < 2 || y >= 18)
                    s = "test_wall";
                else s = "test_floor";

                return TerrainTypeManager.GetType(s);
            }
            set
            {
                
            }
        }

        public TerrainType this[int x, int y]
        {
            get => this[CurrentLevel, x, y];
            set => this[CurrentLevel, x, y] = value;
        }

        public TerrainType this[Position p]
        {
            get => this[CurrentLevel, (int)p.X, (int)p.Y];
            set => this[CurrentLevel, (int)p.X, (int)p.Y] = value;
        }

        public TerrainType this[string level, Position p]
        {
            get => this[level, (int)p.X, (int)p.Y];
            set => this[level, (int)p.X, (int)p.Y] = value;
        }

        public string GetPrettyName(string level)
        {
            return "TestLevel";
        }
    }
}