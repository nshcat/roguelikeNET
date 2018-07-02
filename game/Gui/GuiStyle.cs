using game.Ascii;

namespace game.Gui
{
    // TODO fix up this class. proper defaults, constructor etc
    public class GuiStyle
    {
        public Color Foreground { get; set; }
        public Color Background { get; set; }
        
        public ButtonStyle ButtonStyle { get; set; } = ButtonStyle.Default;

        public GuiStyle(Color foreground, Color background)
        {
            Foreground = foreground;
            Background = background;
        }

        public GuiStyle(GuiStyle other)
        {
            Background = other.Background;
            Foreground = other.Foreground;
            ButtonStyle = new ButtonStyle(other.ButtonStyle);
        }
        
        public static readonly GuiStyle SimpleMonochrome = new GuiStyle(Color.White, Color.Black);
    }
}