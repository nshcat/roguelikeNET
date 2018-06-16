using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class DebugNative
    {
        [DllImport("libascii")]
        public static extern void debug_create_test_scene();
        
        [DllImport("libascii")]
        public static extern void test_color(ref Color c);
    }
}