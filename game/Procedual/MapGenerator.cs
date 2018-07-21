using System;
using System.Collections.Generic;
using System.Threading;
using game.Ascii;

namespace game.Procedual
{
    public class MapGenerator : BackgroundTask
    {
        protected List<IMapGeneratorPhase> phases;
        protected MapGeneratorState state;
        protected IMapGeneratorOutput output;

        public MapGenerator(Action<TaskProgress> cb, Dimensions dim, int seed, IMapGeneratorOutput outp)
            : base(cb)
        {
            output = outp;
            state = new MapGeneratorState(dim, seed);        
            phases = new List<IMapGeneratorPhase>();
          
            // Insert phases
            /*phases.Add(new ElevationPhase());
            phases.Add(new ThermalErosionPhase(8));
            phases.Add(new TemperaturePhase());*/
        }
        
        protected override void Task()
        {
            // Execute all registered phases in order
            for (int i = 0; i < phases.Count; ++i)
            {
                // Call process report callback to indicate current phase.
                // The total number of phases is one more than amount of stored phases
                // since map export needs to be done after all phases have finished their
                // work.
                Callback(new TaskProgress(phases[i].Label(), i, phases.Count + 1, false));
                
                // Apply phase on current state
                phases[i].Apply(state);
            }
            
            // Export the map using given map output implementation
            Callback(new TaskProgress("Exporting map", phases.Count, phases.Count + 1, false));
            output.Apply(state);
            
            // Signal task completion
            Callback(new TaskProgress("Done", phases.Count + 1, phases.Count + 1, false));
            
            // Show "done" message for a short bit
            Thread.Sleep(1000);
            Callback(new TaskProgress("Done", phases.Count + 1, phases.Count + 1, true));
        }
    }
}