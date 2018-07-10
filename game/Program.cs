using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using game.Ascii;
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
            Engine.initialize(new GameInfo("dragonhoard", "0.0.1", "libascii test"), args);
            
            EntityComponent.EntityManager.Initialize();
            
            Renderer.IsBatchMode = true;
            
            CurrentScene = new TestScene();//GUIExampleScene();

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
            // TODO: default vlaues dont seem to work!
            string json =
            "{  \"meow\" : 13, \"nyan\" : \"bla\", \"bar\" : [ \"first\", \"second\", \"third\" ], \"foo\" : { \"chirp\" : 42 } }";

            var o = AutoJson.JsonLoader.Deserialize<Test>(JObject.Parse(json));

            Console.WriteLine("meow: {0}, nyan: {1}, foo.chirp: {2}, bar: {3}", o.Meow, o.Nyan, o.Foo.Chirp,
                String.Join(", ", o.Bar));
            
            
            var p = new Program();
            p.run(args);
        }
    }
}