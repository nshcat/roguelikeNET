using game.Ascii;

namespace game
{
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
        /// <returns>A deterministic hash value, in [0, MAXINT]</returns>
        int PositionHash(Position p);
    }
}