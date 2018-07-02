using game.Ascii;

namespace game.Gui
{
    internal abstract class Control
    {
        protected Position position;

        public Position Position => position;
        
        protected Control(Position position)
        {
            this.position = position;
        }

        public abstract void Render(Container c);
    }
}