
using System;

namespace game.Ascii
{
    public static class Screen
    {
        public static void clear()
        {
            Native.ScreenNative.screen_clear();
        }

        public static void clearTile(Position p)
        {
            Native.ScreenNative.screen_clear_tile(ref p);
        }

        public static void setTile(Position p, Tile t)
        {
            var fg = t.FrontColor;
            var bg = t.BackColor;
            
            Native.ScreenNative.screen_set_tile(ref p, ref fg, ref bg, t.Glyph);
        }

        public static void setDepth(Position p, byte d)
        {
            if(d > 255)
                throw new ArgumentException("Depth argument out of range [0, 255]");
            
            Native.ScreenNative.screen_set_depth(ref p, d);
        }

        public static Dimensions getDimensions()
        {
            var d = new Dimensions();
            
            Native.ScreenNative.screen_get_dimensions(ref d);

            return d;
        }
    }
}