using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace game.Ascii
{
    /// <summary>
    /// A class implementing a 2-dimensional vector of unsigned ints that represents a set of dimensions in glyphs.
    /// This is equivalent to glm::uvec2 on the C++ side.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Dimensions
    {
        private UInt32 x;
        private UInt32 y;

        public Dimensions(uint x, uint y)
        {
            this.x = x;
            this.y = y;
        }

        public uint X
        {
            get => x;
            set => x = value;
        }

        public uint Y
        {
            get => y;
            set => y = value;
        }

        public static bool operator ==(Dimensions left, Dimensions right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Dimensions left, Dimensions right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Dimensions other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Dimensions && Equals((Dimensions) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) x * 397) ^ (int) y;
            }
        }
        
        public static Dimensions Origin = new Dimensions(0, 0);
    }
}