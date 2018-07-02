using System.Reflection;
using game.Ascii;

namespace game.Gui
{
    internal class Window : Container
    {
        protected Color front;
        protected string title;

        public Window(string title, Position position, Dimensions dimensions, Color front, Color back)
            : base(position, dimensions, back)
        {
            this.front = front;
            this.title = title;
        }

        public override void Render()
        {
            Screen.drawWindow(position, dimensions, title, front, back, true);
        }

        public override Dimensions SurfaceDimensions()
        {
            return new Dimensions(dimensions.X - 2, dimensions.Y - 2);
        }

        public override Position SurfacePosition()
        {
            return position + new Position(1, 1);
        }
    }
}