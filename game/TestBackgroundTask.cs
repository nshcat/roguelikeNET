using System;
using System.Threading;
using game.Ascii;

namespace game
{
    public class TestBackgroundTask : BackgroundTask
    {
        public TestBackgroundTask(Action<TaskProgress> c)
            : base(c)
        {
        }
        
        protected override void Task()
        {
            var rnd = new Random();
            var phases = new string[]
            {
                "Creating map buffer",
                "Generating elevation map",
                "Generating temperature map",
                "Generating drainage map",
                "Placing hills",
                "Placing volcanoes",
                "Generating biomes",
                "Placing lakes",
                "Generating rivers",
                "Generating natual resources",
                "Generating fauna information",
                "Generating flora information",                    
                "Generating initial settlements",
                "Generating fortifications",
                "Generating populations",
                "Cleaning up map state"         
            };

            for (int i = 0; i < phases.Length; ++i)
            {
                Callback(new TaskProgress(phases[i], i, phases.Length, false));
                Thread.Sleep(rnd.Next(500, 1500));
            }
            
            Callback(new TaskProgress("Done", phases.Length, phases.Length, false));
            Thread.Sleep(rnd.Next(200, 1200));
            Callback(new TaskProgress("Done", phases.Length, phases.Length, true));
        }
    }
}