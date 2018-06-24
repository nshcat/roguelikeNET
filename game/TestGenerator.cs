using System;
using System.Threading;
using game.Ascii;

namespace game
{
    public class TestGenerator : Generator
    {
        public TestGenerator(Action<GeneratorProgress> c)
            : base(c)
        {
        }
        
        protected override void Task()
        {
            var rnd = new Random();
            
            Callback(new GeneratorProgress("Phase 1", 0, 5, false));        
            Thread.Sleep(rnd.Next(400, 4000));
            Callback(new GeneratorProgress("Phase 2", 1, 4, false));        
            Thread.Sleep(rnd.Next(400, 4000));
            Callback(new GeneratorProgress("Phase 3", 2, 5, false));        
            Thread.Sleep(rnd.Next(400, 4000));
            Callback(new GeneratorProgress("Phase 4", 3, 5, false));        
            Thread.Sleep(rnd.Next(400, 4000));
            Callback(new GeneratorProgress("Phase 5", 4, 5, false));        
            Thread.Sleep(rnd.Next(400, 4000));
            Callback(new GeneratorProgress("Done", 5, 5, false));   
            Thread.Sleep(rnd.Next(400, 4000));
            Callback(new GeneratorProgress("Done", 5, 5, true)); 
        }
    }
}