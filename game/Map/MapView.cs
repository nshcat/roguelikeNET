using game.Ascii;

namespace game
{
    /// <summary>
    /// A class responsible for drawing a segment of the current dungeon floor.
    /// </summary>
    /// <remarks>
    /// This class is given a position and dimensions, which describe the part of the screen it should draw to.
    /// The current map segment is derived from <see cref="Center"/>, which will always lay directly in the center
    /// of the map view. It can be modified by other components, for example to always center the camera on a moving
    /// actor.
    /// </remarks>
    public class MapView
    {
        /// <summary>
        /// The actual map implementation associated with this instance.
        /// </summary>
        protected IMap Map
        {
            get;
            set;
        }

        /// <summary>
        /// The position in map coordinates where the map view should be centered on.
        /// </summary>
        public Position Center
        {
            get;
            set;
        }

        /// <summary>
        /// The dimensions of the map view. This determines how big the drawn-to area
        /// of the screen will be.
        /// </summary>
        public Dimensions Dimensions
        {
            get;
            protected set;
        }

        /// <summary>
        /// Top-left position of this map view on the screen.
        /// </summary>
        public Position Position
        {
            get;
            protected set;
        }

        /// <summary>
        /// Construct new MapView instance.
        /// </summary>
        /// <param name="map">The underlying map data source</param>
        /// <param name="topLeft">Top left position on screen</param>
        /// <param name="dims">Dimensions of the view</param>
        public MapView(IMap map, Position topLeft, Dimensions dims)
        {
            Map = map;
            Position = topLeft;
            Dimensions = dims;
        }

        /// <summary>
        /// Render map view to screen.
        /// </summary>
        public void Render()
        {
            // Local coordinates: Relative coordinates inside the map view
            // Screen coordinates: LC + Position
            // Map coordinates: Coordinates on the currently active dungeon level
            
            // Calculate Top-Left position of map view in map coordinates.
            // This is the vector that has to be added to every local coordinate
            // to retrieve the corresponding map coordinate
            var shift = new Vector((int)Center.X - (Dimensions.X / 2), (int)Center.Y - (Dimensions.Y / 2)).Floor();
            
            // Iterate over all local coordinates and try to draw map cell, if in bounds
            for (int ix = 0; ix < Dimensions.X; ++ix)
            {
                for (int iy = 0; iy < Dimensions.Y; ++iy)
                {
                    var localPos = new Position(ix, iy);
                    var mapPos = new Vector(ix, iy) + shift;
                    var screenPos = localPos + Position;

                    // Only draw cell if its both in map and screen bounds.
                    // This way, cells that lay outside the map will just be black.
                    if (InMapBounds(mapPos) && InScreenBounds(screenPos))
                    {
                        // Convert map position to integral value
                        var mapPosIntegral = mapPos.ToPosition();
                        
                        // Retrieve terrain type for current map cell
                        var type = Map[mapPosIntegral];
                        
                        // Special action is required if the type uses the varied ground feature
                        if (type.IsVaried)
                        {
                            // Calulcate position hash. This is deterministic and will always result in the same
                            // value for a given position value.
                            var hash = Map.GetPositionHash(mapPosIntegral);
                            
                            // Select tile from palette based on hash value
                            var tile = type.Palette.PickValue(hash);
                            
                            Screen.SetTile(screenPos, tile);
                        }
                        else
                        {
                            // Just draw the tile information
                            Screen.SetTile(screenPos, type.Tile);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the (possibly negative) position described by given vector
        /// lies in bounds of the currently active dungeon level.
        /// </summary>
        /// <param name="v">Position to check</param>
        /// <returns>Flag indicating result of the check</returns>
        private bool InMapBounds(Vector v)
        {
            // Negative components directly disqualify this position
            if (v.X < 0 || v.Y < 0)
                return false;
            
            // Convert to position. This is possible since we checked for negative values.
            var p = v.ToPosition();

            return (p.X < Map.Dimensions.X && p.Y < Map.Dimensions.Y);
        }

        /// <summary>
        /// Checks if given postion lies inside the screen bounds.
        /// </summary>
        /// <param name="p">Position to check</param>
        /// <returns>Flag indicating result of the check</returns>
        private bool InScreenBounds(Position p)
        {
            return (p.X < Screen.Width && p.Y < Screen.Height);
        }
    }
}