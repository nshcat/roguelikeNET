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
            Callback(new GeneratorProgress("Phase 1", 0, 5, false));        
            Thread.Sleep(5000);
            Callback(new GeneratorProgress("Phase 2", 1, 4, false));        
            Thread.Sleep(5000);
            Callback(new GeneratorProgress("Phase 3", 2, 5, false));        
            Thread.Sleep(5000);
            Callback(new GeneratorProgress("Phase 4", 3, 5, false));        
            Thread.Sleep(5000);
            Callback(new GeneratorProgress("Phase 5", 4, 5, false));        
            Thread.Sleep(5000);
            Callback(new GeneratorProgress("Done", 5, 5, false));   
            Thread.Sleep(5000);
            Callback(new GeneratorProgress("Done", 5, 5, true)); 
        }
    }
}