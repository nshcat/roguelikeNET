using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class RendererNative
    {
        [DllImport("libascii")]
        public static extern void renderer_render();
    }
}