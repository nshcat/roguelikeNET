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
    public class Color
    {
        private Int32 r;
        private Int32 g;
        private Int32 b;

        public Color(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            
            checkRanges();
        }

        public Color()
        {
            this.r = 0;
            this.g = 0;
            this.b = 0;
        }

        public int R
        {
            get => r;
            set
            {
                r = value;
                checkRanges();
            }
        }

        public int G
        {
            get => g;
            set
            {
                g = value;
                checkRanges();               
            }
        }

        public int B
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
            var item = obj as Color;

            if (item == null)
            {
                return false;
            }

            return this.R == item.R &&
                   this.G == item.G &&
                   this.B == item.B;
        }

        public override int GetHashCode()
        {
            // The invariant states that each component is always in the interval [0, 255]. This
            // allows us to easily construct a hash by bit shifting the components and OR'ing them
            // together.
            return R | (G << 8) | (B << 16);
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