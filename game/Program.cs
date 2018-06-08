using System;
using System.Runtime.CompilerServices;
using game.Ascii;

namespace game
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.initialize(args);
            //Debug.createTestScene();

            for (byte i = 0; i < 20; ++i)
            {
                var p = new Position(i, 0);
                
                Screen.setTile(p, new Tile(Color.White, Color.Black, 0));
                Screen.setDepth(p, (byte)(i*2));
            }

            Logger.postMessage(SeverityLevel.Warning, "managed", "Hello from .net!");
            Logger.postMessage(SeverityLevel.Debug, "Hello from .net, without tag!");
            Logger.postMessage(SeverityLevel.Debug, "managed", "graphics.height: " + Configuration.getValue<uint>("graphics.height"));
            
            Console.WriteLine(Configuration.getValue<string>("graphics.tileset"));
            
            Logger.postMessage(SeverityLevel.Debug, "managed", String.Format("User path: {0}", Paths.UserDirectory));
            Logger.postMessage(SeverityLevel.Debug, "managed", String.Format("Data path: {0}", Paths.DataDirectory));
            

            while (!RenderContext.shouldClose())
            {
                RenderContext.pumpEvents();
                RenderContext.beginFrame();
                Renderer.render();
                RenderContext.endFrame();
            }
        }
    }
}