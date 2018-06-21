namespace game
{
    public interface IScene
    {
        /// <summary>
        /// Update scene simulation state according to given amount of passed game ticks.
        /// </summary>
        /// <param name="elapsedTicks">Amount of game ticks that have passed since the last call to this method</param>
        void update(long elapsedTicks);

        /// <summary>
        /// Render scene to screen
        /// </summary>
        void render();
    }
}