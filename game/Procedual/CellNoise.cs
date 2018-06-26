namespace game.Procedual
{
    public class CellNoise : INoise
    {
        protected NoiseRandom RNG { get; set; }

        public CellNoise(NoiseRandom r)
        {
            RNG = r;
        }

        public float ValueAt(float x, float y)
        {
            int intX = (int) x;
            int intY = (int) y;

            float fracX = x - intX;
            float fracY = y - intY;

            float[,,] points = new float[3, 3, 2];

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    int tempX = intX + i - 1;
                    int tempY = intY + j - 1;

                    points[i, j, 0] = tempX + RNG.Next(tempX, tempY, 1);
                    points[i, j, 1] = tempY + RNG.Next(tempX, tempY, 2);
                }
            }

            float distance1 = float.MaxValue;
            float distance2 = float.MaxValue;

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    float tempDis = euclideanSquared(x, y, points[i, j, 0], points[i, j, 1]);

                    if (tempDis < distance1)
                    {
                        if (distance1 < distance2)
                            distance2 = distance1;

                        distance1 = tempDis;
                    }
                    else if (tempDis < distance2)
                        distance2 = tempDis;
                }
            }

            return distance2 - distance1;
        }

        protected float euclideanSquared(float x1, float y1, float x2, float y2)
        {
            float difX = x1 - x2;
            float difY = y1 - y2;

            return (difX * difX + difY * difY);
        }
    }
}