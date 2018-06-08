using System;
using System.Runtime.CompilerServices;

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
        public Polar toPolar()
        {
            var radius = Math.Sqrt(X*X + Y*Y);
            var angle = Math.Atan2(Y, X);
            return new Polar(radius, angle);
        }

        /// <summary>
        /// Rounds the member values to the nearest whole number, using ceil().
        /// </summary>
        /// <returns></returns>
        public Vector ceil()
        {
            return new Vector(Math.Ceiling(X), Math.Ceiling(Y));
        }
        
        /// <summary>
        /// Rounds the member values to the nearest whole number, using floor().
        /// </summary>
        /// <returns></returns>
        public Vector floor()
        {
            return new Vector(Math.Floor(X), Math.Floor(Y));
        }

        /// <summary>
        /// Converts instance to a position to be used with screen routines. This uses floor to round
        /// to the nearest whole integer. If ceil ist desired, <see cref="ceil"/> should be used
        /// before calling this method.
        /// This will throw if any of the components are negative.
        /// </summary>
        /// <returns></returns>
        public Position toPosition()
        {
            if (X < 0 || Y < 0)
                throw new ArgumentOutOfRangeException("Can't convert vector with negative components to point");
            
            return new Position(
                    (uint)Math.Floor(X),
                    (uint)Math.Floor(Y)
                );
        }

        /// <summary>
        /// Convert given polar coordinates to a cartesian vector.
        /// </summary>
        /// <param name="p">Polar coordinates to convert</param>
        /// <returns></returns>
        public static Vector FromPolar(Polar p)
        {
            return p.toVector();
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
        
        public static Vector operator /(Vector lhs, double f)
        {
            return new Vector(lhs.X / f, lhs.Y / f);
        }
        #endregion
    }
}