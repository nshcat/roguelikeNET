
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using game.Ascii.Native;

namespace game.Ascii
{
    public static class Screen
    {
        public static Dimensions Dimensions => getDimensions();
        public static uint Width => Dimensions.X;
        public static uint Height => Dimensions.Y;
        
        public static void clear()
        {
            Native.ScreenNative.screen_clear();
        }

        public static void clearTile(Position p)
        {
            if (Renderer.IsBatchMode)
                Renderer.enqueueCommand(RenderCommand.ClearTileCommand(p));
            else
                Native.ScreenNative.screen_clear_tile(ref p);
        }

        public static void setTile(Position p, Tile t)
        {
            if (Renderer.IsBatchMode)
            {
                Renderer.enqueueCommands(
                    RenderCommand.SetGlyphCommand(p, t.Glyph),
                    RenderCommand.SetBackgroundCommand(p, t.BackColor),
                    RenderCommand.SetForegroundCommand(p, t.FrontColor)
                );
            }
            else
            {
                var fg = t.FrontColor;
                var bg = t.BackColor;
                Native.ScreenNative.screen_set_tile(ref p, ref fg, ref bg, t.Glyph);
            }
        }

        public static void setDepth(Position p, byte d)
        {
            if(d > 255)
                throw new ArgumentException("Depth argument out of range [0, 255]");

            if (Renderer.IsBatchMode)
                Renderer.enqueueCommand(RenderCommand.SetDepthCommand(p, d));
            else
                Native.ScreenNative.screen_set_depth(ref p, d);
        }

        private static Dimensions getDimensions()
        {
            var d = new Dimensions();
            
            Native.ScreenNative.screen_get_dimensions(ref d);

            return d;
        }  
    }
}