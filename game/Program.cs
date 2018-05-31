using System;
using game.Ascii;

namespace game
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.initialize(args);
            Debug.createTestScene();

            Logger.postMessage(SeverityLevel.Warning, "managed", "Hello from .net!");
            Logger.postMessage(SeverityLevel.Debug, "Hello from .net, without tag!");
            Logger.postMessage(SeverityLevel.Debug, "managed", "graphics.height: " + Configuration.getValue<uint>("graphics.height"));
            
            while (!RenderContext.shouldClose())
            {
                RenderContext.beginFrame();
                Renderer.render();
                RenderContext.endFrame();
            }
        }
    }
}