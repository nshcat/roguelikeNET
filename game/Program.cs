using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using game.Ascii;
using game.AutoJson;
using game.EntityComponent;
using game.EntityComponent.Components;
using Newtonsoft.Json.Linq;
using SadRex;

namespace game
{
    class Program
    {
        private DateTime lastFrame;
        private bool isFirstFrame = true;
        private readonly long millisPerTick = 50L;
        private long leftoverTime;
        
        public IScene CurrentScene
        {
            get;
            set;
        }

        public void run(string[] args)
        {
            // Initialize engine
            Engine.Initialize(new GameInfo("dragonhoard", "0.0.1", "libascii test", "dragonhoard"), args);
            
            // Initialize important, global systems
            EntityManager.Initialize();
            TerrainTypeManager.Initialize();
                        
          
            Renderer.IsBatchMode = false;
            
            CurrentScene = new MapScene();//TestScene();//GUIExampleScene();
            
            while (!RenderContext.shouldClose())
            {
                Input.Begin();
                CurrentScene.update(updateTimer());
                Input.End();

                RenderContext.beginFrame();
                Screen.Clear();
                CurrentScene.render();
                Renderer.render();
                RenderContext.endFrame();
            }
            
            Engine.Deinitialize();
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