using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using game.Ascii;

namespace game.Gui
{
    // TODO: Handle selection move event AFTER everything is defined (on End())
    // => That means that every frame, the input information of the previous one is used
    // TODO: dynamically growing containers
    // TODO: columns, for example for buttons
    
    /// <summary>
    /// A class that manages the state of a graphical user interface. It follows the paradigm of "immediate
    /// mode gui", which means that the GUI is not defined by stateful objects that are defined at a central point
    /// in time, but rather by how the user code calls the provided methods. Things like user input are automatically
    /// handled by the implementation.
    /// </summary>
    public sealed class Gui
    {
        private Container container;
        private List<Control> renderQueue;

        /// <summary>
        /// Current vertical position
        /// </summary>
        private int vpos;
        
        /// <summary>
        /// Current nesting level
        /// </summary>
        private int nestLevel;
        
        /// <summary>
        /// Current number of selectable controls
        /// </summary>
        private int selectableCount = 0;

        /// <summary>
        /// The currently selected control.
        /// </summary>
        private int currentSelection;
        
        private bool HasContainer => container != null;

        public Gui()
        {
            container = null;
            renderQueue = new List<Control>();
        }
 
        
        #region Container methods
        /// <summary>
        /// Spawn new window as container for consecutive control insertions
        /// </summary>
        /// <param name="title">Title of the window, shown in the top left corner</param>
        /// <param name="tl">Position of the window, top left corner</param>
        /// <param name="dim">Total dimensions of the window</param>
        /// <param name="front">Foreground color</param>
        /// <param name="back">Background color</param>
        public void Window(string title, Position tl, Dimensions dim, Color front, Color back)
        {
            SetContainer(new Window(title, tl, dim, front, back));
        }
        
        /// <summary>
        /// Spawn new window as container for consecutive control insertions. Uses the currently
        /// set style for colorization.
        /// </summary>
        /// <param name="title">Title of the window, shown in the top left corner</param>
        /// <param name="tl">Position of the window, top left corner</param>
        /// <param name="dim">Total dimensions of the window</param>
        public void Window(string title, Position tl, Dimensions dim)
        {
            Window(title, tl, dim, Style.Foreground, Style.Background);
        }
        #endregion
        
        
        #region Control methods

        // TODO InputBoxSettings (like stepping, maximum, minimum)
        public void IntegerBox(string text, int textWidth, int valWidth, ref int value)
        {
            // TODO do better
            
            bool isSelected = currentSelection == selectableCount;

            if (isSelected)
            {
                if (Input.hasKey(Key.Right))
                    ++value;
                
                if (Input.hasKey(Key.Left))
                    --value;
            }
            
            AddSelectableControl(new IntegerBox(CalculatePosition(), isSelected, Style.Foreground, text, textWidth, valWidth, value));
            UpdateVerticalPosition();
        }

        /// <summary>
        /// Insert button with explicit width and text color.
        /// </summary>
        /// <param name="text">Text to display as button label</param>
        /// <param name="width">Explicit width of the button</param>
        /// <param name="front">Text color</param>
        /// <returns>Flag indicating if the button was pressed by the user this frame</returns>
        public bool Button(string text, int width, Color front)
        {
            // This is not off-by-one, since we did not increment the count yet
            bool isSelected = currentSelection == selectableCount;
            
            AddSelectableControl(new Button(CalculatePosition(), text, width, front, isSelected));
            UpdateVerticalPosition();

            return isSelected && Input.hasKey(Key.Enter);
        }

        /// <summary>
        /// Insert button with explicit width, using the current style for colorization
        /// </summary>
        /// <param name="text">Text to display as button label</param>
        /// <param name="width">Explicit width of the button</param>
        /// <returns>Flag indicating if the button was pressed by the user this frame</returns>
        public bool Button(string text, int width)
        {
            return Button(text, width, Style.Foreground);
        }

        /// <summary>
        /// Insert button with explicit text color. The button will
        /// automatically be sized to fit the label perfectly.
        /// </summary>
        /// <param name="text">Text to display as button label</param>
        /// <param name="front">Text color</param>
        /// <returns>Flag indicating if the button was pressed by the user this frame</returns>
        public bool Button(string text, Color front)
        {
            return Button(text, text.Length, front);
        }
        
        /// <summary>
        /// Insert button with given text, using current style for coloring. The button will
        /// automatically be sized to fit the label perfectly.
        /// </summary>
        /// <param name="text">Text to display as button label</param>
        /// <returns>Flag indicating if the button was pressed by the user this frame</returns>
        public bool Button(string text)
        {
            return Button(text, text.Length, Style.Foreground);
        }    
        
        /// <summary>
        /// Insert a label with given explicit text color.
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="front">Color to use for string rendering</param>
        public void Label(string text, Color front)
        {
            CheckHasContainer();      
            AddControl(new Label(CalculatePosition(), text, front));
            UpdateVerticalPosition();
        }
        
        /// <summary>
        /// Insert a label and use stored gui style for coloring
        /// </summary>
        /// <param name="text">Text to display</param>
        public void Label(string text)
        {
            CheckHasContainer();      
            AddControl(new Label(CalculatePosition(), text, Style.Foreground));
            UpdateVerticalPosition();
        }
        
        /// <summary>
        /// Inserts a blank as if an empty control was inserted. This means that control
        /// spacing is also applied.
        /// </summary>
        public void Blank()
        {
            CheckHasContainer();
            UpdateVerticalPosition();
        }

        /// <summary>
        /// Insert empty lines.
        /// </summary>
        /// <param name="height">Number of empty lines to insert, default 0.</param>
        public void Spacer(int height = 0)
        {
            CheckHasContainer();
            ++vpos;
        }

        /// <summary>
        /// Increase nesting level by given amount. This causes following controls
        /// to appear indented.
        /// </summary>
        /// <param name="amount">Amount to rise nesting level by</param>
        public void Nest(int amount = 1)
        {
            CheckHasContainer();
            nestLevel += amount;
        }

        /// <summary>
        /// Lower current nesting level by given amount.
        /// </summary>
        /// <param name="amount">Amount to unnest by</param>
        /// <exception cref="InvalidOperationException">If the current nesting level does not match up with given amount</exception>
        public void Unnest(int amount = 1)
        {
            CheckHasContainer();
            
            if(nestLevel <= 0)
                throw new InvalidOperationException("Gui is currently not in a nested state");
            
            if(amount < nestLevel)
                throw new InvalidOperationException("Tried to unnest more nest levels than currently available");

            nestLevel -= amount;
        }
        #endregion
        
        
        #region Configuration methods and properties

        /// <summary>
        /// Describes by how many blank lines consecutive controls are divided by
        /// </summary>
        public int ControlSpacing { get; set; } = 0;

        /// <summary>
        /// Describes by how many spaces a nested level is indented in relation
        /// to its parent
        /// </summary>
        public int IndentationDepth { get; set; } = 2;
        
        
        /// <summary>
        /// The currently set style for the gui. This will be used when no explicit
        /// styling parameters are passed to the various control methods.
        /// </summary>
        public GuiStyle Style { get; set; } = GuiStyle.SimpleMonochrome;

        #endregion


        #region Implementation methods

        private void SetContainer(Container c)
        {
            if(HasContainer)
                throw new InvalidOperationException("Cannot use more than one container per gui object");

            container = c;
        }

        private void AddControl(Control c)
        {
            renderQueue.Add(c);
        }
        
        private void AddSelectableControl(Control c)
        {
            AddControl(c);
            ++selectableCount;
        }

        private void UpdateVerticalPosition()
        {
            vpos += ControlSpacing + 1;
        }
        
        private void CheckHasContainer()
        {
            if(!HasContainer)
                throw new InvalidOperationException("Cannot use control method without specifying a container first");            
        }

        private Position CalculatePosition()
        {
            return new Position(nestLevel * IndentationDepth + container.SurfacePosition().X, vpos + container.SurfacePosition().Y);
        }
        
        #endregion
             
        
        #region General operations

        public void Begin()
        {
            vpos = 0;
            selectableCount = 0;
        }

        public void End()
        {
            // Readjust selection in the case that the amount of selected control changed
            if (currentSelection >= selectableCount)
                currentSelection = selectableCount - 1;
            
            // Process selection input
            if (Input.hasKey(Key.Up))
                currentSelection = Math.Max(0, currentSelection - 1);

            if (Input.hasKey(Key.Down))
                currentSelection = Math.Min(selectableCount - 1, currentSelection + 1);
        }
        
        public void Render()
        {
            // Draw underlying container
            container.Render();
            
            // Draw all registered render elements in order
            foreach (var e in renderQueue)
            {
                e.Render(container);
            }
            
            // Clear everything
            container = null;
            renderQueue.Clear();
        }
        #endregion
    }
}