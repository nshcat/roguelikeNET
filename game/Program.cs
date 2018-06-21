using System;
using game.Ascii;

using SadRex;

namespace game
{
    class Program
    {
        private DateTime lastFrame;
        private bool isFirstFrame = true;
        private readonly long millisPerTick = 50L;
        private long leftoverTime;
        
        public Scene CurrentScene
        {
            get;
            set;
        }

        public void run(string[] args)
        {
            Engine.initialize(args);

            Renderer.IsBatchMode = false;
            
            CurrentScene = new TestScene();
            
            while (!RenderContext.shouldClose())
            {
                Input.begin();            
                CurrentScene.update(updateTimer());
                Input.end();
                
                RenderContext.beginFrame();
                Screen.clear();
                CurrentScene.render();
                Renderer.render();
                RenderContext.endFrame();
            }
        }

        private long updateTimer()
        {
            // It's the first frame. Initialize lastFrame to current time
            // and do one simulation tick.
            if (isFirstFrame)
            {
                lastFrame = DateTime.Now;
                isFirstFrame = false;
                return 1;
            }
            else
            {
                var thisFrame = DateTime.Now;
                var elapsedTime = (long) (thisFrame - lastFrame).TotalMilliseconds + leftoverTime;

                lastFrame = thisFrame;
                
                leftoverTime = elapsedTime % millisPerTick;

                // Make sure that at least one tick is generated
                return Math.Max(elapsedTime / millisPerTick, 1L);
            }
        }
        
        static void Main(string[] args)
        {      
            var p = new Program();
            p.run(args);
        }
    }
}