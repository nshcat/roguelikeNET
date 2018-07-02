using game.Ascii;

namespace game.Gui
{
    public class ButtonStyle
    {
        /// <summary>
        /// Special constant indicating that the implementation should automatically
        /// calculate the button width
        /// </summary>
        public static int AutoWidth = -1;
        
        /// <summary>
        /// Whether the button should use the global style for coloring of the
        /// text or not
        /// </summary>
        public bool OverrideForegroundColor { get; set; } = false;
        
        /// <summary>
        /// Whether the button should use the global style for background coloring
        /// or not
        /// </summary>
        public bool OverrideBackgroundColor { get; set; } = false;
        
        /// <summary>
        /// Override front text color. Only used if <see cref="OverrideForegroundColor"/> is true.
        /// </summary>
        public Color Foreground { get; set; } = Color.White;
        
        /// <summary>
        /// Override back text color. Only used if <see cref="OverrideBackgroundColor"/> is true.
        /// </summary>
        public Color Background { get; set; } = Color.Black;

        /// <summary>
        /// The width of the button. If this is set to <see cref="AutoWidth"/>, the implementation
        /// automatically picks the smallest width possible.
        /// </summary>
        public int Width { get; set; } = AutoWidth;
        
        /// <summary>
        /// Whether the button should display selection as inversion of the front and back color.
        /// </summary>
        public bool InvertOnSelection { get; set; } = true;
        
        /// <summary>
        /// Template that should be used to generate the appearance of a currently not selected
        /// button.
        /// </summary>
        public string NonSelectedTemplate { get; set; } = "{0}";
        
        /// <summary>
        /// Template that should be used to generate the appearance of a currently selected
        /// button.
        /// </summary>
        public string SelectedTemplate { get; set; } = "{0}";

        public ButtonStyle()
        {
            
        }

        public ButtonStyle(ButtonStyle other)
        {
            OverrideBackgroundColor = other.OverrideBackgroundColor;
            OverrideForegroundColor = other.OverrideForegroundColor;
            Width = other.Width;
            SelectedTemplate = other.SelectedTemplate;
            NonSelectedTemplate = other.NonSelectedTemplate;
            InvertOnSelection = other.InvertOnSelection;
            Foreground = other.Foreground;
            Background = other.Background;
        }
        
        public static readonly ButtonStyle Default = new ButtonStyle();
    }
}