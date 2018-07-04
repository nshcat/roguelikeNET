using game.Ascii;
using game.Gui;

namespace game
{
    public class TestContainer : Gui.Container
    {
        private TileImage image;
        
        public TestContainer(Position position)
            : base(position, new Dimensions(21, 9), Color.Black)
        {
            image = TileImageLoader.LoadImage("testgui");
        }

        public override void Render()
        {
            image.Draw(Position);
        }

        public override Dimensions SurfaceDimensions()
        {
            return new Dimensions(10, 7);
        }

        public override Position SurfacePosition()
        {
            return position + new Position(1, 1);
        }
    }
    
    
    public class GUIExampleScene : IScene
    {
        private Gui.Gui g;
        private TestContainer c;
        private TileImage[] images;
        private int currentSelection;
       

        public GUIExampleScene()
        {
            g = new Gui.Gui();
            c = new TestContainer(Position.Origin);
            images = new[]
            {
                TileImageLoader.LoadImage("village"),
                TileImageLoader.LoadImage("hoard"),
                TileImageLoader.LoadImage("sleep"),
                TileImageLoader.LoadImage("trade")
            };
        }
        
        private ButtonStyle style = new ButtonStyle()
        {
            SelectedTemplate = "{0}" + (char)153,
            NonSelectedTemplate = "{0}" + (char)186,
            OverrideForegroundColor = true,
            Foreground = new Color(61, 55, 91),
            InvertOnSelection = true,
            OverrideInvertedForegroundColor = true,
            InvertedForeground = new Color(126, 131, 169),
            Width = 9                         
        };
        
        public void update(long elapsedTicks)
        {
            g.Begin();        
            g.CustomContainer(c);
            g.ControlSpacing = 1;
            g.Button("VILLAGE", style);
            g.Button("HOARD", style);
            g.Button("SLEEP", style);
            g.Button("TRADE", style);

            currentSelection = g.Selection;
            g.End();
        }

        public void render()
        {
            g.Render();
            images[currentSelection].Draw(new Position(11, 1));
        }
    }
}