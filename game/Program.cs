using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using game.Ascii;
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
            Engine.Initialize(new GameInfo("dragonhoard", "0.0.1", "libascii test", "dragonhoard"), args);
            
            EntityComponent.EntityManager.Initialize();
            var entity = EntityComponent.EntityManager.Construct("cat");
            var entity2 = EntityComponent.EntityManager.Construct("cat");
            var entity3 = EntityComponent.EntityManager.Construct("dog");
            var entity4 = EntityManager.Construct("snake");
            var entity5 = EntityManager.Construct("bug");
            
            var result = EntityComponent.EntityManager.GetEntities<EntityComponent.Components.TestComponent1>();
            
            
            
            
            var result2 = EntityComponent.EntityManager.AllEntities
                .GetEntities<TestComponent2>()
                .GetEntities<TestComponent1>(x => x.TestData == 42);


            var c = result2.GetComponents<TestComponent1>().ToList();


            var c2 = EntityComponent.EntityManager.AllEntities
                .GetEntities<TestComponent3>()
                .GetComponents<TestComponent3>()
                .ToList();

            var r = EntityComponent.EntityManager.GetTypes<TestComponent1>().ToList();

            var r2 = EntityManager.GetEntities<BaseComponent>().ToList();
            
            
            
            var queue = new PriorityQueue<string, int>();
            
            /*queue.Enqueue("A", 3);
            queue.Enqueue("B", 44);
            queue.Enqueue("C", 1);
            queue.Enqueue("D", 20);
            queue.Enqueue("E", 4);
            queue.Enqueue("F", -4);
            queue.Enqueue("G", 2);
            queue.Enqueue("H", 10);
            queue.Enqueue("I", 1);*/
            
            queue.Enqueue("A", 1);
            queue.Enqueue("B", 2);
            queue.Enqueue("C", 1);
            queue.Enqueue("D", 1);
            queue.Enqueue("E", 1);
            queue.Enqueue("F", 2);
            queue.Enqueue("G", 2);
            queue.Enqueue("H", 1);
            queue.Enqueue("I", 2);

            /*queue.Remove("C");
            queue.Remove("I");
            queue.Remove("H");
            queue.Remove("F");*/
            
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                Logger.PostMessage(SeverityLevel.Debug, $"Value: {item.Item}, Priority: {item.Priority}");
            }
            
            Renderer.IsBatchMode = false;
            
            CurrentScene = new TestScene();//GUIExampleScene();
            
            while (!RenderContext.shouldClose())
            {
                Input.begin();
                CurrentScene.update(updateTimer());
                Input.end();

                RenderContext.beginFrame();
                Screen.Clear();
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