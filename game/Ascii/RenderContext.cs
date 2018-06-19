namespace game.Ascii
{
    public static class RenderContext
    {
        public static bool shouldClose()
        {
            return Native.RenderContextNative.render_context_should_close();
        }

        public static void beginFrame()
        {
            Native.RenderContextNative.render_context_begin_frame();
        }
        
        public static void endFrame()
        {
            Native.RenderContextNative.render_context_end_frame();
        }
    }
}