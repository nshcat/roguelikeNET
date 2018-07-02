using game.Ascii;

namespace game.Gui
{
    public class GuiStyle
    {
        public Color Foreground { get; protected set; }
        public Color Background { get; protected set; }

        public GuiStyle(Color foreground, Color background)
        {
            Foreground = foreground;
            Background = background;
        }
        
        public static GuiStyle SimpleMonochrome = new GuiStyle(Color.White, Color.Black);
    }
}