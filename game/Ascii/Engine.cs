using System;

namespace game.Ascii
{
    public static class Engine
    {
        /// <summary>
        /// Initialize the engine global state and all sub systems.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void initialize(string[] args)
        {
            // The native library expects the program name to be the first entry in argv,
            // like its convention in C.
            string[] argv = new string[args.Length + 1];
            Array.Copy(args, 0, argv, 1, args.Length);
            argv[0] = "roguelikeNET";
            
            Native.EngineNative.engine_initialize(argv.Length, argv);
        }
    }
}