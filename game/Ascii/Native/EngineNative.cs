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
        [DllImport("libascii.so")]
        public static extern void engine_initialize(Int64 argc, string[] argv);
    }
}