namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// Pad string to given total length, applying padding on both sides.
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="length">Desired minimum toal length</param>
        /// <returns>Padded string of minimal length <see cref="length"/></returns>
        public static string PadBoth(this string str, int length)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }
    }
}