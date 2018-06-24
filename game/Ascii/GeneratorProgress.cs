using System;

namespace game.Ascii
{
    public class GeneratorProgress
    {
        public string Message
        {
            get;
        }

        public int CurrentPhase
        {
            get;
        }

        public int TotalPhases
        {
            get;
        }

        public bool IsDone
        {
            get;
        }

        public GeneratorProgress(string msg, int phase, int totalPhases, bool isDone)
        {
            Message = msg;
            CurrentPhase = phase;
            TotalPhases = totalPhases;
            IsDone = isDone;
        }
    }
}