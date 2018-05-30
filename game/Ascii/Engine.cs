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
            Native.EngineNative.initialize(args.Length, args);
        }
    }
}