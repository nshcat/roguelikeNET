using game.Ascii;

namespace game.Procedual
{
    /// <summary>
    /// A common interface for different, two-dimensional noise algorithms
    /// </summary>
    public interface INoise
    {
        float ValueAt(float x, float y);
    }
}