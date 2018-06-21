using System.Collections.Generic;
using game.Ascii.Native;

namespace game.Ascii
{
    public static class Renderer
    {
        private static List<RenderCommand> commandQueue = new List<RenderCommand>();

        private static bool isBatchMode;
        
        /// <summary>
        /// Controls whether the renderer will utilize batch mode or not.
        /// </summary>
        public static bool IsBatchMode
        {
            get => isBatchMode;
            set => isBatchMode = value;
        }
        
        /// <summary>
        /// Render to screen.
        /// </summary>
        public static void render()
        {
            if (IsBatchMode)
            {
                // Send commands to libascii
                var buffer = commandQueue.ToArray();
                Native.ScreenNative.screen_apply_commands(buffer, buffer.Length);

                // Clear the command queue for following frame
                commandQueue.Clear();
            }

            // Perform render in usual way
            Native.RendererNative.renderer_render();
        }


        /// <summary>
        /// Add given render command to the command queue. It will be send as one batch to the
        /// native library once the program calls renderBatch.
        /// </summary>
        /// <param name="c">The render command to add to the queue</param>
        internal static void enqueueCommand(RenderCommand c)
        {
            commandQueue.Add(c);
        }
        
        /// <summary>
        /// Add given sequence of render comamnds to the command queue.
        /// </summary>
        /// <param name="cmds">Render command sequence to add to the command queue</param>
        internal static void enqueueCommands(params RenderCommand[] cmds)
        {
            foreach(var c in cmds)
                commandQueue.Add(c);
        }
    }
}