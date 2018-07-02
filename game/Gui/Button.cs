using System;
using game.Ascii;

namespace game.Gui
{
    internal class Button : SelectableControl
    {
        protected string text;
        protected int width;
        protected Color front;
        
        public Button(Position position, string text, int width, Color front, bool isSelected)
            : base(position, isSelected)
        {
            this.text = text;
            this.width = width;
            this.front = front;
        }

        public override void Render(Container c)
        {
            Color fg, bg;

            if (IsSelected)
            {
                fg = c.Back;
                bg = front;
            }
            else
            {
                fg = front;
                bg = c.Back;
            }
            
            Screen.drawString(position, text.PadBoth(width), fg, bg);
        }
    }
}