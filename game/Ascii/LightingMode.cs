namespace game.Ascii
{
    /// <summary>
    /// An enumeration detailing how a particular cell interacts with the lighting system.
    /// </summary>
    public enum LightingMode
    {
        /// <summary>
        /// Cell blocks light and does not receive any lighting at all.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Cell blocks light, but receives a small amount of light. This is intended to
        /// be used for walls of rooms.
        /// </summary>
        Dim  = 1,
        
        /// <summary>
        /// Cell receives full lighting.
        /// </summary>
        Full = 2
    }
}