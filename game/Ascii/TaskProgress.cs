namespace game.Ascii
{
    public class TaskProgress
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

        public TaskProgress(string msg, int phase, int totalPhases, bool isDone)
        {
            Message = msg;
            CurrentPhase = phase;
            TotalPhases = totalPhases;
            IsDone = isDone;
        }
    }
}