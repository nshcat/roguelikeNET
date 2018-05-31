using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class DebugNative
    {
        [DllImport("libascii.so")]
        public static extern void debug_create_test_scene();
    }
}