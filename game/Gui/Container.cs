using game.Ascii;

namespace game.Gui
{
    public abstract class Container
    {
        protected Position position;
        protected Dimensions dimensions;
        protected Color back;

        public Position Position => position;
        public Dimensions Dimensions => dimensions;
        public Color Back => back;


        protected Container(Position position, Dimensions dimensions, Color back)
        {
            this.position = position;
            this.dimensions = dimensions;
            this.back = back;
        }

        public abstract void Render();

        /// <summary>
        /// Returns the dimensions of the drawable surface inside the container.
        /// </summary>
        /// <returns>Dimensions of the drawable surface</returns>
        public abstract Dimensions SurfaceDimensions();

        /// <summary>
        /// Returns the top left position of the drawable surface inside the container.
        /// </summary>
        /// <returns>Top left position of the drawable surface</returns>
        public abstract Position SurfacePosition();
    }
}