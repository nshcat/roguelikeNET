using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace game.Ascii
{
    /// <summary>
    /// A struct type describing a single light source in the game world.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Light : ICloneable
    {
        private Position position;
        private float intensity;
        private readonly float padding1;
        private float colorR;
        private float colorG;
        private float colorB;
        private float colorAlpha;
        private LightAttenuation attenuation;
        private readonly float padding2;
        private readonly float padding3;
        private readonly float padding4;

        /// <summary>
        /// Construct new light info instance
        /// </summary>
        /// <param name="intensity">Base intensity</param>
        /// <param name="color">Base color</param>
        /// <param name="attenuation">Attenuation information</param>
        public Light(float intensity, Color color, LightAttenuation attenuation) : this()
        {
            this.intensity = intensity;
            this.attenuation = attenuation;
            
            SetColor(color);
        }

        /// <summary>
        /// The position of this light on the screen, with the origin being the
        /// top left corner.
        /// </summary>
        /// <remarks>
        /// This uses the same coordinate system as all other normal screen routines.
        /// </remarks>
        public Position Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// Intensity of the light source. This will be attenuated based on the falloff function
        /// described by <see cref="Attenuation"/> and the distance to the light source.
        /// </summary>
        public float Intensity
        {
            get => intensity;
            set => intensity = value;
        }

        /// <summary>
        /// Base color of this light source.
        /// </summary>
        public Color Color
        {
            get => new Color(colorR, colorG, colorB);
            set => SetColor(value);
        }

        /// <summary>
        /// Attenuation instance describing the function used to determine light intensity falloff.
        /// </summary>
        public LightAttenuation Attenuation
        {
            get => attenuation;
            set => attenuation = value;
        }

        /// <summary>
        /// Clone this instance.
        /// </summary>
        /// <returns>Deep copy if this instance</returns>
        public object Clone()
        {
            return new Light(Intensity, Color, (LightAttenuation) Attenuation.Clone())
            {
                Position = this.Position
            };
        }

        /// <summary>
        /// Deconstruct given integral color into its floating point components in range [0, 1]
        /// </summary>
        /// <param name="c">Color instance to deconstruct</param>
        private void SetColor(Color c)
        {
            colorR = c.R / 255.0f;
            colorG = c.G / 255.0f;
            colorB = c.B / 255.0f;
            colorAlpha = 1.0f;
        }
    }
}