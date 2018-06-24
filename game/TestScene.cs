using System;
using game.Ascii;

namespace game
{
    // TODO: Create class that can use N points.
    // Provide method "SampleIncrement" that gives the increment that will lead
    // to good result when drawing. Could add up distance between the points (p0->p1, p1-> ... -> pn-1, pn-1 -> pn)
    // and use that as heuristic
    public class TestScene : IScene
    {
        private Position p0, p1, p2, p3;
        private Color clr = Color.Black;
        private TileImage img;

        private TaskProgress prog;

        public TestScene()
        {
            /*p0 = new Position(6, 25);
            p1 = new Position(15, 13);
            p2 = new Position(25, 26);
            p3 = new Position(35, 13);*/
            
            p0 = new Position(6, 25);
            p1 = new Position(6, 6);
            p2 = new Position(16, 6);
            p3 = new Position(26, 6);

            img = TileImageLoader.LoadImage("test");

            new TestBackgroundTask(p => this.prog = p).Run();
        }
        
        public void update(long elapsedTicks)
        {
            if(Input.hasKey(Key.K))
                clr = Color.Green;
            else clr = Color.Black;
        }

        public void render()
        {
            for (uint iy = 0; iy < Screen.Height; ++iy)
            {
                for (uint ix = 0; ix < Screen.Width; ++ix)
                {
                    Screen.setTile(new Position(ix, iy), new Tile(clr, Color.Black, 0));
                }
            }
            
            
            Tile t = new Tile(Color.White, Color.Black, 0);
            
            for (double d = 0.0; d <= 1.0; d += 0.005)
            {
                var p = evaluate(d);

                var px = evaluate(d - 0.005);
                var py = evaluate(d + 0.005);

                var dif = (py - px).Normalized;

                var pl = dif.PerpendicularClockwise.Normalized;
                var pr = dif.PerpendicularCounterClockwise.Normalized;

                for (double d2 = 0.0; d2 < 1.0; d2 += 0.1)
                {
                    Screen.setTile((p + d2*pl).ToPosition(), t);
                    Screen.setTile((p + d2*pr).ToPosition(), t);
                }
          
                Screen.setTile(p.ToPosition(), t);
            }
            
            img.DrawTransparent(new Position(4, 4), new Tile(Color.Black, Color.Black, 0));


            if (!prog.IsDone)
            {
                for (int i = 0; i < prog.Message.Length; ++i)
                {
                    Screen.setTile(new Position((uint) (0 + i), 0),
                        new Tile(Color.Black, Color.White, (byte) prog.Message[i]));
                }

                int barCount = (int)((((double) prog.CurrentPhase) / (double) prog.TotalPhases) * 20.0);
                
                Screen.setTile(new Position(0U, 1U), new Tile(Color.Black, Color.White, (byte)'['));
                Screen.setTile(new Position(21U, 1U), new Tile(Color.Black, Color.White, (byte)']'));
                
                for(int i = 0; i < barCount; ++i)
                    Screen.setTile(new Position(1U + (uint)i, 1U), new Tile(Color.Black, Color.White, (byte)'='));
            }
        }

        
        public Vector evaluate(double t)
        {
            return Math.Pow(1 - t, 3.0) * p0.ToVector() + 3 * t * Math.Pow(1 - t, 2.0) * p1.ToVector()
                                                            + 3 * t * t * (1 - t) * p2.ToVector() +
                                                            Math.Pow(t, 3.0) * p3.ToVector();
        }
    }
}