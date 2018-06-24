﻿
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Xml.XPath;
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
             
        /// <summary>
        /// Draws given string to screen at given start position with given colors. If the string would exceed
        /// the screen boundaries, it will be cut off.
        /// </summary>
        /// <param name="p">Starting point</param>
        /// <param name="s">String to draw</param>
        /// <param name="fg">Foreground color</param>
        /// <param name="bg">Background color</param>
        public static void drawString(Position p, string s, Color fg, Color bg)
        {
            for (uint ix = 0; ix < s.Length; ++ix)
            {
                // Gracefully exit if string exceeds screen boundaries
                if (p.X + ix >= Dimensions.X)
                    break;
                
                setTile(new Position(p.X + ix, p.Y), new Tile(bg, fg, (byte)s[(int)ix]));
            }
        }

        public static void drawWindow(Position p, Dimensions d, string title, Color fg, Color bg, bool filled)
        {
            var background = new Tile(bg, bg, 0);
            var horzBorder = new Tile(bg, fg, 196);
            var vertBorder = new Tile(bg, fg, 179);

            // Draw the border and, if requested by the caller, fill in the center of the window
            for (uint iy = p.Y; iy < p.Y + d.Y; ++iy)
            {
                // Draw the horizontal border segments along the x axis
                if (iy == p.Y || iy == p.Y + d.Y - 1)
                {
                    for (uint ix = p.X; ix < p.X + d.X; ++ix)
                    {
                        setTile(new Position(ix, iy), horzBorder);
                    }
                }
                else
                {
                    // Set vertical border segments at both sides of the window
                    setTile(new Position(p.X, iy), vertBorder);
                    setTile(new Position(p.X + d.X - 1, iy), vertBorder);
                    
                    // Fill the inner portion of this vertical slice of the window
                    // with the background color if requested by the caller
                    if (filled)
                    {
                        for (uint ix = p.X + 1; ix < p.X + d.X - 1; ++ix)
                        {
                            setTile(new Position(ix, iy), background);
                        }
                    }                
                }
            }
            
            // Draw corners
            setTile(p, new Tile(bg, fg, 218));
            setTile(new Position(p.X + d.X - 1, p.Y), new Tile(bg, fg, 191));
            setTile(new Position(p.X, p.Y + d.Y - 1), new Tile(bg, fg, 192));
            setTile(new Position(p.X + d.X - 1, p.Y + d.Y - 1), new Tile(bg, fg, 217));
            
            // Draw the window title
            var str = (char)180 + title + (char)195;
            drawString(new Position(p.X + 1, p.Y), str, fg, bg);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="segmentCount"></param>
        /// <param name="currentValue">Current value of the progress bar in percent, stored as [0, 1]</param>
        /// <param name="fg"></param>
        /// <param name="bg"></param>
        public static void drawProgressBar(Position p, uint segmentCount, double currentValue, Color fg, Color bg)
        {
            var filledSegment = new Tile(bg, fg, 219);
            var halfFilledSegment = new Tile(bg, fg, 221);
            var emptySegment = new Tile(bg, bg, 219);//new Tile(bg, new Color(80, 80, 80), 176);
                  
            // Draw completely filled segments
            
            // This applies floor() to the value
            var filledCount = (uint) (currentValue * segmentCount);
            for(uint ix = 0; ix < filledCount; ++ix)
                setTile(new Position(p.X + 1U + ix, p.Y), filledSegment);

            // Draw half-filled segment
            //var x = floatNumber - Math.Truncate(floatNumber);
            bool drewHalfSegment = false;         
            var tmp = currentValue * segmentCount;
            var fraction = tmp - Math.Truncate(tmp);

            if (fraction >= 0.5)
            {
                setTile(new Position(p.X + 1U + filledCount, p.Y), halfFilledSegment);
                drewHalfSegment = true;
            }

            // Draw empty segments
            for (uint ix = (drewHalfSegment ? 1U : 0U) + filledCount; ix < segmentCount; ++ix)
            {
                setTile(new Position(p.X + 1U + ix, p.Y), emptySegment);
            }
            
            // Draw border
            setTile(p, new Tile(bg, fg, 179));
            setTile(new Position(p.X + segmentCount + 1, p.Y), new Tile(bg, fg, 179));
        }
    }
}