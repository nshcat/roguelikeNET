using System;

namespace game.Procedual
{
    public class SimplexNoise : INoise
    {
        // TODO: config file
        protected float generalSkew = (float) (Math.Sqrt(3.0f) - 1.0f) * 0.5f;
        protected float generalUnskew = (float) (3.0f-Math.Sqrt(3.0f))/6.0f;

        protected NoiseRandom RNG
        {
            get;
            set;
        }

        public SimplexNoise(NoiseRandom r)
        {
            RNG = r;
        }
        
        public float ValueAt(float x, float y)
        {  
            float disbX, disbY, dismX, dismY, distX, distY, noiseB, noiseM, noiseT, tempDis,
                skewValue, unskewValue;

            int cornerBx, cornerBy, cornerMx, cornerMy, cornerTx, cornerTy, gradB, gradM, gradT;

            float[,] gradients = new float[8, 2];
            for (int i = 0; i < 8; ++i)
            {
                gradients[i, 0] = (float)Math.Cos(0.785398163f * (float) i);
                gradients[i, 1] = (float) Math.Sin(0.785398163f * (float) i);
            }

            skewValue = (x + y) * generalSkew;
            cornerBx = Floor(x + skewValue);
            cornerBy = Floor(y + skewValue);

            unskewValue = (float) (cornerBx + cornerBy) * generalUnskew;
            disbX = x - (float) cornerBx + unskewValue;
            disbY = y - (float) cornerBy + unskewValue;

            if (disbX > disbY)
            {
                cornerMx = 1 + cornerBx;
                cornerMy = cornerBy;
            }
            else
            {
                cornerMx = cornerBx;
                cornerMy = 1 + cornerBy;
            }

            cornerTx = 1 + cornerBx;
            cornerTy = 1 + cornerBy;

            dismX = disbX - (float) (cornerMx - cornerBx) + generalUnskew;
            dismY = disbY - (float) (cornerMy - cornerBy) + generalUnskew;

            distX = disbX - 1.0f + generalUnskew + generalUnskew;
            distY = disbY - 1.0f + generalUnskew + generalUnskew;

            gradB = RNG.Next(cornerBx, cornerBy);
            gradM = RNG.Next(cornerMx, cornerMy);
            gradT = RNG.Next(cornerTx, cornerTy);


            tempDis = 0.5f - disbX * disbX - disbY * disbY;
            if (tempDis < 0.0f)
            {
                noiseB = 0.0f;
            }
            else
            {
                tempDis *= tempDis;
                noiseB = tempDis * tempDis * DotProduct(gradients, gradB, disbX, disbY);
            }

            tempDis = 0.5f - dismX * dismX - dismY * dismY;
            if (tempDis < 0.0f)
            {
                noiseM = 0.0f;
            }
            else
            {
                tempDis *= tempDis;
                noiseM = tempDis * tempDis * DotProduct(gradients, gradM, dismX, dismY);
            }
            
            tempDis = 0.5f - distX * distX - distY * distY;
            if (tempDis < 0.0f)
            {
                noiseT = 0.0f;
            }
            else
            {
                tempDis *= tempDis;
                noiseT = tempDis * tempDis * DotProduct(gradients, gradT, distX, distY);
            }

            return (noiseB + noiseM + noiseT);
        }

        protected int Floor(float val)
        {
            return (val >= 0 ? (int) val : (int) val - 1);
        }
        
        protected float DotProduct(float[,] grad, int idx, float x, float y)
        {
            return (grad[idx, 0] * x + grad[idx, 1] * y);
        }
    }
}