using System;
using game.Ascii;

namespace game.Gui
{
    internal class Button : SelectableControl
    {
        protected string text;
        protected ButtonStyle style;
        
        
        public Button(Position position, string text, ButtonStyle style, bool isSelected)
            : base(position, isSelected)
        {
            this.text = text;
            this.style = style;
        }

        public override void Render(Container c, GuiStyle s)
        {
            // Retrieve colors to use for renderering
            Color fg = style.OverrideForegroundColor ? style.Foreground : s.Foreground;
            Color bg = style.OverrideBackgroundColor ? style.Background : s.Background;

            // Invert colors on selection if requested
            if (IsSelected && style.InvertOnSelection)
            {
                fg.Swap(ref bg);
            }

            // Determine width
            var width = style.Width == ButtonStyle.AutoWidth ? text.Length : style.Width;

            // Create button string from appropiate template
            var template = IsSelected ? style.SelectedTemplate : style.NonSelectedTemplate;
            
            // The user can specify a special text color for inverted color mode.
            // If that is the case, only the actual button text is colored with this
            // special color, not any of the characters that might be present in the template
            // string.
            if (style.OverrideInvertedForegroundColor && IsSelected)
            {
                var pos = template.IndexOf("{0}");

                var prefix = template.Substring(0, pos);
                var postfix = template.Substring(pos + 3);

                var buttonText = text.Substring(0, Math.Min(width, text.Length)).PadBoth(width);
                
                Screen.drawString(position, prefix, fg, bg);
                Screen.drawString(position + new Position(prefix.Length, 0), buttonText, style.InvertedForeground, bg);
                Screen.drawString(position + new Position(prefix.Length + buttonText.Length, 0), postfix, fg, bg);
            }
            else // Normal case, everything is colored in the same way
            {         
                var buttonText = String.Format(template,
                    text.Substring(0, Math.Min(width, text.Length)).PadBoth(width));

                // Center text and draw to screen
                Screen.drawString(position, buttonText, fg, bg);
            }
        }
    }
}