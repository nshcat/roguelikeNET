using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace game.Ascii
{
    /// <summary>
    /// A class implementing a 2-dimensional vector of unsigned ints that represents a position on
    /// the 2D screen, in "glyph-space". Note that the components cannot not be negative. For that the class
    /// Vector or PolarVector should be used.
    /// This is equivalent to glm::uvec2 on the C++ side.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Position
    {
        private UInt32 x;
        private UInt32 y;

        public Position(uint x, uint y)
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

        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Position other)
        {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Position && Equals((Position) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) x * 397) ^ (int) y;
            }
        }

        public Vector ToVector()
        {
            return new Vector(X, Y);
        }

        public static Position FromVector(Vector v)
        {
            return v.ToPosition();
        }
        
        public static Position operator +(Position lhs, Position rhs)
        {
            return new Position(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
        
        public static Position Origin = new Position(0, 0);
    }
}