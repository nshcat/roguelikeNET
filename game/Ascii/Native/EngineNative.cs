using System;
using System.Runtime.InteropServices;

namespace game.Ascii.Native
{
    public static class EngineNative
    {
        /// <summary>
        /// Initialize the engine global state and all sub systems.
        /// </summary>
        /// <param name="argc">Number of command line arguments</param>
        /// <param name="argv">Command line arguments</param>
        [DllImport("libascii")]
        public static extern void engine_initialize(ref GameInfo info, Int32 argc, string[] argv);
    }
}