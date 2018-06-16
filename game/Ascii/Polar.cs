using System;
using System.Net.Http.Headers;

namespace game.Ascii
{
    public class Polar
    {
        /// <summary>
        /// Angle in radians. In interval [0, 2pi)
        /// </summary>
        public double Angle { get; set; }
        
        /// <summary>
        /// The distance from the origin
        /// </summary>
        public double Radius { get; set; }


        public Polar(double radius, double angle)
        {
            Angle = angle;
            Radius = radius;
        }


        #region Equality Members
        protected bool Equals(Polar other)
        {
            return Angle.Equals(other.Angle) && Radius.Equals(other.Radius);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Polar) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Angle.GetHashCode() * 397) ^ Radius.GetHashCode();
            }
        }

        public static bool operator ==(Polar left, Polar right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Polar left, Polar right)
        {
            return !Equals(left, right);
        }
        #endregion
        
        #region Conversion  
        /// <summary>
        /// Convert instance to cartesian vector
        /// </summary>
        /// <returns></returns>
        public Vector ToVector()
        {
            var x = Radius * Math.Cos(Angle);
            var y = Radius * Math.Sin(Angle);
            
            return new Vector(x, y);
        }

        /// <summary>
        /// Convert given cartesian vector to polar coordinates
        /// </summary>
        /// <param name="v">Cartesian vector to convert</param>
        /// <returns></returns>
        public static Polar FromVector(Vector v)
        {
            return v.ToPolar();
        }
        
        /// <summary>
        /// Convert instance to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[r: " + Radius + ", a: " + Angle + "]";
        }
        #endregion
    }
}