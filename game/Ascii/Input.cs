namespace game.Ascii
{
    public static class Input
    {
        /// <summary>
        /// Begin input processing. This should be called every frame just before input is processed
        /// by the game logic.
        /// </summary>
        public static void begin()
        {
            Native.InputNative.input_begin();
        }
        
        /// <summary>
        /// End input processing. This should be called every frame after the game logic is done
        /// processing input.
        /// </summary>
        public static void end()
        {
            Native.InputNative.input_end();
        }

        /// <summary>
        /// Query whether given key is currently pressed or not.
        /// </summary>
        /// <param name="k">Key to check for</param>
        /// <returns>True if the given key is currently pressed, false otherwise</returns>
        public static bool hasKey(Key k)
        {
            return Native.InputNative.input_has_key((int) k);
        }
    }
}