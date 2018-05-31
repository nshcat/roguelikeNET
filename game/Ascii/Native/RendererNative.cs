using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class RendererNative
    {
        [DllImport("libascii.so")]
        public static extern void renderer_render();
    }
}