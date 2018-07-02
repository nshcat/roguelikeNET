using System;
using System.Runtime.InteropServices;
using game.Ascii;

namespace game.Gui
{
    internal class IntegerBox : SelectableControl
    {
        protected int currentValue;
        
        /// <summary>
        /// Width of the text portion only
        /// </summary>
        protected int textWidth;
    
        /// <summary>
        /// Width of the value portion only
        /// </summary>
        protected int valueWidth;

        protected string text;

        protected Color front;
        
        public IntegerBox(Position position, bool isSelected, Color front, string text, int textWidth, int valueWidth, int currentValue)
            : base(position, isSelected)
        {
            this.currentValue = currentValue;
            this.valueWidth = valueWidth;
            this.textWidth = textWidth;
            this.text = text;
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
            
            var str = text.Substring(0, Math.Min(textWidth, text.Length)).PadRight(textWidth);
            
            Screen.drawString(position, str, fg, bg);

            var valStr = currentValue.ToString();
            var valStrCropped = valStr.Substring(0, Math.Min(valueWidth, valStr.Length)).PadLeft(valueWidth);

            var valStrComplete = $"<{valStrCropped}>";
            
            Screen.drawString(position + new Position(textWidth + 1, 0),valStrComplete, front, c.Back);
        }
    }
}