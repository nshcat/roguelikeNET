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
        /// Whether the button should use a special text color when it gets selected.
        /// This can be used if inverted colors are desired, but the background color is
        /// not suitable for it. Note that this only affects the color of the actual button
        /// label, not any of the template characters.
        /// </summary>
        public bool OverrideInvertedForegroundColor { get; set; } = false;
        
        /// <summary>
        /// Override front text color. Only used if <see cref="OverrideForegroundColor"/> is true.
        /// </summary>
        public Color Foreground { get; set; } = Color.White;
        
        /// <summary>
        /// Override back text color. Only used if <see cref="OverrideBackgroundColor"/> is true.
        /// </summary>
        public Color Background { get; set; } = Color.Black;

        /// <summary>
        /// Custom color used for the text in case <see cref="InvertOnSelection"> is true. This is only used if
        /// <see cref="OverrideInvertedForegroundColor"/> is true.
        /// </summary>
        public Color InvertedForeground { get; set; } = Color.Black;

        /// <summary>
        /// The width of the button. If this is set to <see cref="AutoWidth"/>, the implementation
        /// automatically picks the smallest width possible. Note that this only applies to the
        /// button text and the text padding, not any possible characters introduced by the template.
        /// This means that a width of 3 and a template like "_{0}_" will result in a total button width
        /// of 5 if a button text of size 3 is supplied.
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