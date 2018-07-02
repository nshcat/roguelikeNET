using game.Ascii;

namespace game.Gui
{
    internal abstract class SelectableControl : Control
    {
        protected bool IsSelected
        {
            get;
            set;
        }
        
        protected SelectableControl(Position position, bool isSelected)
            : base(position)
        {
            this.IsSelected = isSelected;
        }
    }
}