using System;
using System.Runtime.InteropServices;

namespace game.Ascii
{
    /// <summary>
    /// This class represent a 3 component vector of unsigned integers modeling an RGB color.
    /// This is equivalent to glm::uvec3 on the C++ side.
    /// The range for each component is [0, 255].
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        private UInt32 r;    
        private UInt32 g;
        private UInt32 b;

        public Color(uint r, uint g, uint b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            
            checkRanges();
        }

        public uint R
        {
            get => r;
            set
            {
                r = value;
                checkRanges();
            }
        }

        public uint G
        {
            get => g;
            set
            {
                g = value;
                checkRanges();               
            }
        }

        public uint B
        {
            get => b;
            set
            {
                b = value;
                checkRanges();               
            }
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Color && Equals((Color) obj);
        }

        public override int GetHashCode()
        {
            // The invariant states that each component is always in the interval [0, 255]. This
            // allows us to easily construct a hash by bit shifting the components and OR'ing them
            // together.
            return (int)(R | (G << 8) | (B << 16));
        }

        public bool Equals(Color other)
        {
            return r == other.r && g == other.g && b == other.b;
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        private void checkRanges()
        {
            if (!(R >= 0 && R <= 255) && (G >= 0 && G <= 255) && (B >= 0 && B <= 255))
                throw new ArgumentOutOfRangeException("Color component value out of range");
        }
        
        public static Color Black = new Color();
        public static Color White = new Color(255, 255, 255);
        public static Color Red = new Color(255, 0, 0);
        public static Color Green = new Color(0, 255, 0);
        public static Color Blue = new Color(0, 0, 255);
    }
}