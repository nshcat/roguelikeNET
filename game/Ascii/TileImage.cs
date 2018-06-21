using System;
using System.Drawing;
using game.Ascii;

namespace game.Ascii
{
    /// <summary>
    /// A class representing a 2D image made up of individual tiles. Can be drawn on to
    /// the screen by game code.
    /// </summary>
    public class TileImage : ICloneable
    {
        /// <summary>
        /// The width of the image
        /// </summary>
        public int Width => data.GetLength(1);

        /// <summary>
        /// The height of the image
        /// </summary>
        public int Height => data.GetLength(0);

        /// <summary>
        /// The actual image data. The first dimension corresponds to the
        /// y coordinate, the second one to the x coordinate.
        /// </summary>
        protected Tile[,] data;
       
        /// <summary>
        /// Create image from given tile data. This constructor is only supposed to be used
        /// by asset management systems.
        /// </summary>
        /// <param name="data"></param>
        public TileImage(Tile[,] data)
        {
            this.data = data;
        }
        
        /// <summary>
        /// Draw image to the screen, with the top left tile being at given position.
        /// This method takes care to not draw out-of-bounds. If the image is too big and
        /// exceeds screen boundaries, offending tiles will not be drawn.
        /// </summary>
        /// <param name="p">Position of top left tile of the image when drawn to screen</param>
        public void Draw(Position p)
        {
            DrawTransparent(p, null);
        }
        
        
        /// <summary>
        /// Draw image to the screen with given key tile interpreted as being transparent.
        /// This method takes care to not draw out-of-bounds. If the image is too big and
        /// exceeds screen boundaries, offending tiles will not be drawn.
        /// </summary>
        /// <param name="p">Position of top left tile of the image when drawn to screen</param>
        /// <param name="key">Tile to be interpreted as transparent</param>
        public void DrawTransparent(Position p, Tile key)
        {
            for (uint iy = 0; iy < Height; ++iy)
            {
                // Cancel if vertical position is out of bounds
                if ((iy + p.Y) >= Screen.Height)
                    break;

                for (uint ix = 0; ix < Width; ++ix)
                {
                    // Cancel if horizontal position is out of bounds
                    if ((ix + p.X) >= Screen.Width)
                        break;

                    // Retrieve tile
                    var tile = data[iy, ix];

                    if (key == null || tile != key)
                    {
                        // Draw tile to screen
                        Screen.setTile(new Position(ix, iy) + p, data[iy, ix]);
                    }     
                }
            }
        }

        /// <summary>
        /// Retrieve tile at given position in the image.
        /// </summary>
        /// <param name="p">Position defining which tile to return</param>
        /// <returns>Tile at given position in the image</returns>
        /// <exception cref="ArgumentException">If the given position is out of bounds</exception>
        public Tile GetTile(Position p)
        {
            if(p.X >= Width || p.Y > Height)
                throw new ArgumentException("Tile position out of bounds");

            return data[p.Y, p.X];
        }
     
        /// <summary>
        /// Perform deep copy of this tile image.
        /// </summary>
        /// <returns>Deep copy of this tile image</returns>
        public object Clone()
        {
            var data = new Tile[Height, Width];

            for (uint iy = 0; iy < Height; ++iy)
            {
                for (uint ix = 0; ix < Width; ++ix)
                {
                    data[iy, ix] = (Tile)this.data[iy, ix].Clone();
                }
            }

            return new TileImage(data);
        }
    }
}