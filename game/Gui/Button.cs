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
            var buttonText = String.Format(template, text.Substring(0, Math.Min(width, text.Length)));

            // Center text and draw to screen
            Screen.drawString(position, buttonText.PadBoth(width), fg, bg); 
        }
    }
}