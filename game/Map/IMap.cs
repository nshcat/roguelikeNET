using game.Ascii;

namespace game
{
    /// <summary>
    /// The base interface for all map implementations.
    /// A map is a component that manages all the different levels of a dungeon and
    /// their interconnections.
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Calculate a deterministic hash value for given point. This is meant to be used
        /// for terrain types that utilize the varied ground functionality.
        /// </summary>
        /// <remarks>
        /// This is offered as part of the map interface in order to allow the map instance
        /// to cache those values, since they, by requirement, are never allowed to change.
        /// Since this method is potentially called a lot, this is worth it.
        /// </remarks>
        /// <param name="p">Position to calculate the hash value of</param>
        /// <returns>A deterministic hash value, in [0, 1)</returns>
        double GetPositionHash(Position p);

        /// <summary>
        /// The unique string identifier of the currently active dungeon level.
        /// </summary>
        string CurrentLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieve dimensions of the currently active dungeon level
        /// </summary>
        Dimensions Dimensions
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieve terrain type of cell at given dungeon level and position.
        /// </summary>
        /// <param name="level">Unique string identifier of the dungeon level</param>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        TerrainType this[string level, int x, int y]
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieve terrain type of cell at current dungeon level and given position.
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        TerrainType this[int x, int y]
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieve terrain type of cell at current dungeon level and given position.
        /// </summary>
        /// <param name="p">Position of the cell</param>
        TerrainType this[Position p]
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieve terrain type of cell at given dungeon level and position.
        /// </summary>
        /// <param name="level">Unique string identifier of the dungeon level</param>
        /// <param name="p">Position of the cell</param>
        TerrainType this[string level, Position p]
        {
            get;
            set;
        }

        /// <summary>
        /// Retrieve pretty name of a specific dungeon level
        /// </summary>
        /// <param name="level">Unique string identifier of the dungeon level in question</param>
        /// <returns></returns>
        string GetPrettyName(string level);
    }
}