using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class ScreenNative
    {
        [DllImport("libascii")]
        public static extern void screen_get_dimensions(ref Dimensions d);
        
        [DllImport("libascii")]
        public static extern void screen_clear();
        
        [DllImport("libascii")]
        public static extern void screen_clear_tile(ref Position p);
        
        [DllImport("libascii")]
        public static extern void screen_set_tile(ref Position p, ref Color f, ref Color b, byte g);
        
        [DllImport("libascii")]
        public static extern void screen_set_depth(ref Position p, byte d);
    }
}