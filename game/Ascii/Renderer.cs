namespace game.Ascii
{
    public static class Renderer
    {
        public static void render()
        {
            Native.RendererNative.renderer_render();
        }
    }
}