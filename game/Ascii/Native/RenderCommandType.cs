namespace game.Ascii.Native
{
    /// <summary>
    /// An enumeration that is used to determine which component of the screen cell
    /// a particular renderer command affects
    /// </summary>
    public enum RenderCommandType
    {
        SetGlyph,
        SetBackground,
        SetForeground,
        SetDepth,
        ClearTile
    }
}