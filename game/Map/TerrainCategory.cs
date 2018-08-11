namespace game
{
    /// <summary>
    /// An enumeration describing different broad categories of terrain types.
    /// They describe if and how fast a tile of a specific terrain type can be traversed
    /// by a specific class of actor.
    /// </summary>
    public enum TerrainCategory
    {
        /// <summary>
        /// A terrain type that allows traversal to every actor, including land-bound ones.
        /// It could impose a movement penalty though, which is handled by the terrain type.
        /// </summary>
        /// <example>
        /// Normal ground or shallow water, with the latter imposing a movement penalty to
        /// land-bound actors.
        /// </example>
        Passable,
        
        /// <summary>
        /// A terrain type that does not allow traversal by land-bound actors, but flying actors
        /// and other flying entities (like projectiles) can traverse it.
        /// </summary>
        /// <example>
        /// Deep water or lava
        /// </example>
        Impassable,
        
        /// <summary>
        /// A terrain type that does not allow traversal by any actor or projectile. It might
        /// be able to be destroyed by spells though.
        /// </summary>
        /// <example>
        /// Dungeon wall
        /// </example>
        Wall   
    }
}