﻿using System;
using System.Runtime.InteropServices;

namespace game.Ascii
{
    /// <summary>
    /// This class represent a 3 component vector of unsigned integers modeling an RGB color.
    /// This is equivalent to glm::uvec3 on the C++ side.
    /// The range for each component is [0, 255].
    /// </summary>
    [AutoJson.Deserializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Color : ICloneable
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

        public Color(float r, float g, float b)
        {
            this.r = (uint)(r*255.0f);
            this.g = (uint)(g*255.0f);
            this.b = (uint)(b*255.0f);
            
            checkRanges();
        }
        
        [AutoJson.Key("r")]
        [AutoJson.Required]
        public uint R
        {
            get => r;
            set
            {
                r = value;
                checkRanges();
            }
        }

        [AutoJson.Key("g")]
        [AutoJson.Required]
        public uint G
        {
            get => g;
            set
            {
                g = value;
                checkRanges();               
            }
        }

        [AutoJson.Key("b")]
        [AutoJson.Required]
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

        public object Clone()
        {
            return new Color(R, G, B);
        }

        public void Swap(ref Color other)
        {
            Swap(ref r, ref other.r);
            Swap(ref g, ref other.g);
            Swap(ref b, ref other.b);
        }

        private void Swap(ref UInt32 lhs, ref UInt32 rhs)
        {
            var tmp = lhs;
            lhs = rhs;
            rhs = tmp;
        }

        public static Color Black = new Color();
        public static Color White = new Color(255, 255, 255);
        public static Color Red = new Color(255, 0, 0);
        public static Color Green = new Color(0, 255, 0);
        public static Color Blue = new Color(0, 0, 255);
    }
}