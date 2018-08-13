using System;
using game.AutoJson;

namespace game.Ascii
{
    using GlyphOrdinal = Byte;
    
    [AutoJson.Deserializable]
    public class Tile : ICloneable
    {
        [AutoJson.Key("background")]
        [AutoJson.Required]
        public Color BackColor { get; set; }
        
        [AutoJson.Key("foreground")]
        [AutoJson.Required]
        public Color FrontColor { get; set; }
        
        [AutoJson.Key("glyph")]
        [AutoJson.Required]
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


        /// <summary>
        /// Private, parameterless constructor used by AutoJson
        /// </summary>
        private Tile()
        {
            
        }

        protected bool Equals(Tile other)
        {
            return BackColor.Equals(other.BackColor) && FrontColor.Equals(other.FrontColor) && Glyph == other.Glyph;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tile) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = BackColor.GetHashCode();
                hashCode = (hashCode * 397) ^ FrontColor.GetHashCode();
                hashCode = (hashCode * 397) ^ Glyph.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Tile left, Tile right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Tile left, Tile right)
        {
            return !Equals(left, right);
        }


        /// <summary>
        /// Perform deep copy of this object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Tile((Color)BackColor.Clone(), (Color)FrontColor.Clone(), Glyph);
        }
    }
}