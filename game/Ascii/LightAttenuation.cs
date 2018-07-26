using System;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace game.Ascii
{
    /// <summary>
    /// Immutable struct type describing the attenuation function used by a light source
    /// </summary>
    /// <remarks>
    /// This type does not offer any way to query the attenuation values, since this would need GPU
    /// calculations to be replicated here (e.g. if the user sets factors, but queries for radius.
    /// In that case it would need to either calculate the radius based on the factors, or
    /// throw an exception, which both are bad solutions for properties, and not very intuitive).
    /// The intended use-case here is to always construct a new attenuation instance from the supplied
    /// static methods based on some formula when the attenuation needs to change, so no query of state
    /// is needed.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct LightAttenuation : ICloneable
    {
        // On the C side, the light structure actually receives both the factors
        // aswell as the radius. This is a bit redundant, since the factors can be
        // calculated from the radius, if desired. We therefore hide that implementation
        // detail from the user, and offer a class that treats radius and explicit factors
        // as if they describe the same piece of data, while internally keeping track of
        // what fields of the C struct to use.

        /// <summary>
        /// The attenuation factors.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private float[] _attenFactors;
        
        /// <summary>
        /// The radius of the light.
        /// </summary>
        private float _radius;

        /// <summary>
        /// Flag telling the renderer whether the radius or the explicit attenuation factors
        /// should be used
        /// </summary>
        /// <remarks>
        /// Note that the type choice here is intentional, boolean values on the GPU are 32bits wide, and the
        /// C equivalent of this struct is supposed to be directly mappable into GPU memory, and therefore follows
        /// this convention.
        /// </remarks>
        private UInt32 _useRadius;

        /// <summary>
        /// Create new light attenuation instance from given radius
        /// </summary>
        /// <param name="radius">Radius of the light</param>
        /// <returns>Light attenuation instance describing given radius</returns>
        public static LightAttenuation FromRadius(float radius)
        {
            return new LightAttenuation
            {
                _radius = radius,
                _useRadius = 1,
                _attenFactors = new float[3]
            };
        }
        
        /// <summary>
        /// Create new light attenuation instance from given explicit factors.
        /// </summary>
        /// <param name="factors">An array of exactly 3 float values, describing the three factors a, b and c of the
        /// attenuation polynomial</param>
        /// <returns>Light attenuation instance based on given factors</returns>
        /// <exception cref="ArgumentException">If the length of given factor array is not exactly 3</exception>
        public static LightAttenuation FromFactors(params float[] factors)     
        {
            if(factors.Length != 3)
                throw new ArgumentException(String.Format("Expected 3 factors, got {0}", factors.Length));
            
            var att = new LightAttenuation()
            {
                _useRadius = 0,
                _attenFactors = new float[3]
            };
            
            factors.CopyTo(att._attenFactors, 0);

            return att;
        } 
        
        /// <summary>
        /// Clone this instance
        /// </summary>
        /// <returns>Deep copy of this instance</returns>
        public object Clone()
        {
            var other = new LightAttenuation()
            {
                _radius = this._radius,
                _useRadius = this._useRadius,
                _attenFactors = new float[3]
            };
            
            _attenFactors.CopyTo(other._attenFactors, 0);

            return other;
        }
    }
}