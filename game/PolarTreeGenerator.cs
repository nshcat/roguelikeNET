using System;
using System.Drawing;
using System.Linq;
using game.Ascii;
using Color = game.Ascii.Color;

namespace game
{
    public class PolarTreeGenerator
    {
        private Random rnd;
        private double maxRadius;
        
        private double minRadius => maxRadius / 1.5;

        public PolarTreeGenerator(int seed, double r)
        {
            this.rnd = new Random(seed);
            this.maxRadius = r;
        }
        
        public void generateTree(Position center)
        {
            var tile = new Tile(Color.Green, Color.Black, 0);
            var centerTile = new Tile(Color.Blue, Color.Black, 0);
            
            // Generate radii for the four angles 0, pi/2, pi and 3pi/2
            double[] angles = { 0, Math.PI/2.0, Math.PI, (3.0*Math.PI)/2.0 };
            double[] radii = new double[4];

            do
            {
                for (int i = 0; i < radii.Length; ++i)
                {
                    //radii[i] = rnd.NextDouble() * (maxRadius - minRadius) + minRadius;
                    radii[i] = rnd.NextDouble() * ((maxRadius + 0.5) - (maxRadius / 1.5)) + maxRadius / 1.5;

                }
            //} while (radii.Zip(radii.Skip(1), (x, y) => y - x).Min() < 0.3);
            //} while (radii.Zip(radii.Skip(1), (x, y) => Math.Abs(y - x)).Min() < 0.3);
            } while ((Math.Abs(radii[0] - radii[2]) < 0.2) || (Math.Abs(radii[1] - radii[3]) < 0.2));
                
            Console.WriteLine("\n---Generated radii:");
            for (int i = 0; i < radii.Length; ++i)
                Console.WriteLine(radii[i]);   
            Console.WriteLine("---");

            // Check all 
            for (int iy = -(int)maxRadius; iy <= (int)maxRadius; ++iy)
            {
                for (int ix = -(int) maxRadius; ix <= (int) maxRadius; ++ix)
                {
                    var p = Polar.FromVector(new Vector(ix, iy));

                    int ia = 0, ib = 0;
                    var found = false;

                    for (int i = 1; i < radii.Length; ++i)
                    {
                        if (p.Angle <= angles[i])
                        {
                            ia = i - 1;
                            ib = i;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ia = 3;
                        ib = 0;
                    }

                    var r = CommonMath.LMap(p.Angle, angles[ia], (ib == 0 ? 2.0 * Math.PI : angles[ib]), radii[ia],
                        radii[ib]);

                    r = Math.Abs(r);

                    if (new Vector(2, 0) == new Vector(ix, iy))
                    {
                        Console.WriteLine(new Vector(ix, iy) + ":");
                        Console.WriteLine("Calculated radius: " + r + ", Radius of point: " + p.Radius + ", Angle of point: " + p.Angle);
                    }
                    
                    if (new Vector(0, -2) == new Vector(ix, iy))
                    {
                        Console.WriteLine(new Vector(ix, iy) + ":");
                        Console.WriteLine("Calculated radius: " + r + ", Radius of point: " + p.Radius + ", Angle of point: " + p.Angle);
                    }
                    
                    if (new Vector(-2, 0) == new Vector(ix, iy))
                    {
                        Console.WriteLine(new Vector(ix, iy) + ":");
                        Console.WriteLine("Calculated radius: " + r + ", Radius of point: " + p.Radius + ", Angle of point: " + p.Angle);
                    }
                    
                    if (new Vector(0, 2) == new Vector(ix, iy))
                    {
                        Console.WriteLine(new Vector(ix, iy) + ":");
                        Console.WriteLine("Calculated radius: " + r + ", Radius of point: " + p.Radius + ", Angle of point: " + p.Angle);
                    }

                    if (p.Radius <= r)
                    {
                        var pos = Position.FromVector(new Vector(ix, iy) + Vector.FromPosition(center));
                        
                        Screen.setTile(pos, tile);
                    }

                }
            }
            
            Screen.setTile(center, centerTile);
        }
    }
}