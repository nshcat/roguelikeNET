﻿using game.Ascii;

namespace game.Gui
{
    // TODO: allow custom background color
    internal class Label : Control
    {
        protected Color front;
        protected string text;
        
        public Label(Position position, string text, Color front)
            : base(position)
        {
            this.text = text;
            this.front = front;
        }

        public override void Render(Container c)
        {
            Screen.drawString(position, text, front, c.Back);
        }
    }
}