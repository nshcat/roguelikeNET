using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace game.Ascii
{
    /// <summary>
    /// 2D Vector. Uses floating point numbers inside, but supports rounding to the nearest glyph.
    /// Note that the values used do not represent pixels, they represent glyphs. The vector with values
    /// (0.5, 0.5) refers to the center of the first, top left glyph, no matter what size in pixels it might have.
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// X component of the vector
        /// </summary>
        public double X { get; set; }
        
        /// <summary>
        /// Y component of the vector
        /// </summary>
        public double Y { get; set; }
        
        /// <summary>
        /// The length of the vector
        /// </summary>
        public double Length => Math.Sqrt(X * X + Y * Y);

        /// <summary>
        /// Returns a new vector that is the normalized form of this instance
        /// </summary>
        public Vector Normalized => this / Length;
        
        /// <summary>
        /// Returns a new vector that is perpendicular to this vector. Uses the vector
        /// that appears first in clockwise direction.
        /// </summary>
        public Vector PerpendicularClockwise => new Vector(Y, -X);
        
        /// <summary>
        /// Returns a new vector that is perpendicular to this vector. Uses the vector
        /// that appears first in counter-clockwise direction.
        /// </summary>
        public Vector PerpendicularCounterClockwise => new Vector(-Y, X);
        
        
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }
        
        #region Equality members
        private bool Equals(Vector other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(Vector left, Vector right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vector left, Vector right)
        {
            return !Equals(left, right);
        }
        
        #endregion
        
        #region Conversion
        /// <summary>
        /// Convert vector to polar coordinates
        /// </summary>
        /// <returns></returns>
        public Polar ToPolar()
        {
            var radius = Math.Sqrt(X*X + Y*Y);
            var angle = Math.Atan2(Y, X) + Math.PI;
            return new Polar(radius, angle);
        }

        /// <summary>
        /// Rounds the member values to the nearest whole number, using ceil().
        /// </summary>
        /// <returns></returns>
        public Vector Ceil()
        {
            return new Vector(Math.Ceiling(X), Math.Ceiling(Y));
        }
        
        /// <summary>
        /// Rounds the member values to the nearest whole number, using floor().
        /// </summary>
        /// <returns></returns>
        public Vector Floor()
        {
            return new Vector(Math.Floor(X), Math.Floor(Y));
        }

        /// <summary>
        /// Rounds the member values using round() with given midpoint rounding strategy.
        /// </summary>
        /// <param name="r">Midpoint rounding strategy that will be used</param>
        /// <returns></returns>
        public Vector Round(MidpointRounding r = MidpointRounding.AwayFromZero)
        {
            return new Vector(Math.Round(X, r), Math.Round(Y, r));
        }

        /// <summary>
        /// Converts instance to a position to be used with screen routines. This uses floor to round
        /// to the nearest whole integer. If ceil ist desired, <see cref="ceil"/> should be used
        /// before calling this method.
        /// This will throw if any of the components are negative.
        /// </summary>
        /// <returns></returns>
        public Position ToPosition()
        {
            if (X < 0 || Y < 0)
                throw new ArgumentOutOfRangeException("Can't convert vector with negative components to point");
            
            return new Position(
                    (uint)Math.Floor(X),
                    (uint)Math.Floor(Y)
                );
        }

        /// <summary>
        /// Convert instance to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[x: " + X + ", y: " + Y + "]";
        }

        /// <summary>
        /// Convert given polar coordinates to a cartesian vector.
        /// </summary>
        /// <param name="p">Polar coordinates to convert</param>
        /// <returns></returns>
        public static Vector FromPolar(Polar p)
        {
            return p.ToVector();
        }
        
        /// <summary>
        /// Convert given position to a vector
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector FromPosition(Position p)
        {
            return p.ToVector();
        }
        #endregion
        
        #region Operators

        public static Vector operator +(Vector lhs, Vector rhs)
        {
            return new Vector(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
        
        public static Vector operator -(Vector lhs, Vector rhs)
        {
            return new Vector(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }
        
        public static Vector operator *(Vector lhs, double f)
        {
            return new Vector(lhs.X * f, lhs.Y * f);
        }
        
        public static Vector operator *(double f, Vector rhs)
        {
            return new Vector(rhs.X * f, rhs.Y * f);
        }
        
        public static Vector operator /(Vector lhs, double f)
        {
            return new Vector(lhs.X / f, lhs.Y / f);
        }
        #endregion
        
        #region Operations
        /// <summary>
        /// Calculates the dot product between two vectors
        /// </summary>
        /// <param name="other">Second vector to use in the calculation, the first one being the current instance</param>
        /// <returns>The dot product of the two vectors</returns>
        public double Dot(Vector other)
        {
            return (X * other.X) + (Y * other.Y);
        }
        #endregion Operations
    }
}