using System;

namespace game.Ascii
{
    using GlyphOrdinal = Byte;
    
    public class Tile
    {
        public Color BackColor { get; set; }
        public Color FrontColor { get; set; }
        public GlyphOrdinal Glyph { get; set; }

        public Tile(Color backColor, Color frontColor, byte glyph)
        {
            BackColor = backColor;
            FrontColor = frontColor;
            Glyph = glyph;
        }
        
        public Tile(Color frontColor, byte glyph)
        {
            BackColor = Color.Black;
            FrontColor = frontColor;
            Glyph = glyph;
        }
    }
}