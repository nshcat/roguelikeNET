using System;
using System.Collections.Specialized;
using System.IO;
using game.Ascii;
using game.Gui;
using game.Procedual;

namespace game
{
    enum TestActions
    {
        ActionUp,
        ActionDown
    }
    
    // TODO: Create class that can use N points.
    // Provide method "SampleIncrement" that gives the increment that will lead
    // to good result when drawing. Could add up distance between the points (p0->p1, p1-> ... -> pn-1, pn-1 -> pn)
    // and use that as heuristic
    public class TestScene : IScene
    {
        private int x = 15;
        private Position p0, p1, p2, p3;
        private Color clr = Color.Black;
        private TileImage img;
        private Gui.Gui g = new Gui.Gui();

        private bool hasSpawned;
        private TaskProgress prog = new TaskProgress("Starting generator", 0, 1, false);

        private InputMapper testMapping = new InputMapper("test");

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

            var y = new CtorTest
            {
                A = 0,
                B = 0
            };
            
            var x = new CtorTest(y)
            {
                B = 3
            };
            
            Console.WriteLine($"x.B: {x.B}, x.A: {x.A}");
           
            testMapping.SetBinding("ActionDown", new KeyBinding(Key.J));
        }
        
        int selection = 0;
        
        public void update(long elapsedTicks)
        {
            if(testMapping.HasInput(TestActions.ActionUp))
                Logger.postMessage(SeverityLevel.Debug, "InputTest", "\"ActionUp\" was pressed");
            
            if(testMapping.HasInput(TestActions.ActionDown))
                Logger.postMessage(SeverityLevel.Debug, "InputTest", "\"ActionDown\" was pressed");
            
            if (Input.hasKey(Key.Enter) && !hasSpawned)
            {
                //new MapGenerator(p => this.prog = p, new Dimensions(1500U, 1500U), new Random().Next(), outImg).Run();
                //new TestBackgroundTask(p => this.prog = p).Run();
                hasSpawned = true;
            }

            /*if(Input.hasKey(Key.K))
                clr = Color.Green;
            else clr = Color.Black;*/


            selection = 0;
            
            g.Begin();

            g.Style = new GuiStyle(g.Style)
            {
                ButtonStyle = new ButtonStyle()
                {
                    InvertOnSelection = false,
                    SelectedTemplate = "> {0}",
                    NonSelectedTemplate = "  {0}"
                }
            };
            g.Window("test", Position.Origin, new Dimensions(20, 25));
            
            g.Label("meow");
            g.IntegerBox("bla", 6, 3, ref x);
            g.Nest();
            g.Label("nyan");
            g.Button("test1");    
            g.Unnest();
            g.Button("test2");
            if (g.IsSelected)
                selection = 1;
            
            g.Label("blab");
            g.Button("test3");
            if (g.IsSelected)
                selection = 2;
            g.Nest();
            g.Button("test4");
            if (g.IsSelected)
                selection = 3;
            g.Label("blabbb");
            g.Button("test5");
            if (g.IsSelected)
                selection = 4;
            g.Label("blabb");
            g.Unnest();
            g.Button("test6");
            if (g.IsSelected)
                selection = 5;
            g.Button("test7");
            if (g.IsSelected)
                selection = 6;
            g.End();
        }

        public void render()
        {
            g.Render();
            
            Screen.DrawString(new Position(20, 0), selection.ToString(), Color.White, Color.Black);
            
            /*for (uint iy = 0; iy < Screen.Height; ++iy)
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
            
            img.DrawTransparent(new Position(4, 4), new Tile(Color.Black, Color.Black, 0));*/


            if (hasSpawned && !prog.IsDone)
            {
                var bg = new Color(43, 43, 43);
                var fg = new Color(205, 205, 205);
                
                double percent = ((double) prog.CurrentPhase) / (double) prog.TotalPhases;
                
                Screen.DrawWindow(new Position(0U, 0U), new Dimensions(44u, 10U), "Generating map", fg, bg, true);
                
                Screen.DrawString(new Position(2U, 2U), "> " + prog.Message + "..", fg, bg);
                
                Screen.DrawProgressBar(new Position(2U, 4U), 33, percent, fg, bg);
                
                Screen.DrawString(new Position(2U + 33U + 2U + 1U, 4U), (((int)(percent * 100)) + "%").PadLeft(4), fg, bg);
                
                Screen.DrawString(new Position(33U, 7U), "c", Color.Green, bg);
                Screen.DrawString(new Position(33U + 1U, 7U), ": cancel", fg, bg);
                /*for (int i = 0; i < prog.Message.Length; ++i)
                {
                    Screen.setTile(new Position((uint) (0 + i), 0),
                        new Tile(Color.Black, Color.White, (byte) prog.Message[i]));
                }
                
                Screen.drawProgressBar(new Position(0U, 1U), 20, ((double) prog.CurrentPhase) / (double) prog.TotalPhases, Color.White, Color.Black);*/

                /*int barCount = (int)((((double) prog.CurrentPhase) / (double) prog.TotalPhases) * 20.0);
                
                Screen.setTile(new Position(0U, 1U), new Tile(Color.Black, Color.White, (byte)'['));
                Screen.setTile(new Position(21U, 1U), new Tile(Color.Black, Color.White, (byte)']'));
                
                for(int i = 0; i < barCount; ++i)
                    Screen.setTile(new Position(1U + (uint)i, 1U), new Tile(Color.Black, Color.White, (byte)'='));*/
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