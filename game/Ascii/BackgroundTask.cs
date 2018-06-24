using System;
using System.Threading.Tasks;

namespace game.Ascii
{
    /// <summary>
    /// Base class for operations that need to be executed in a background thread
    /// while the game does logic and drawing. The executed task has the ability
    /// to post progress updates to the caller by the means of a callback function.
    /// An example for such a task might be a map generator or loading screen, with
    /// the game displaying the periodically updated progress information as a progress
    /// bar / indicator.
    /// </summary>
    public abstract class BackgroundTask
    {
        protected Action<TaskProgress> Callback
        {
            get;
        }

        public bool IsRunning
        {
            get;
            protected set;
        }

        /// <summary>
        /// Constructor that registers given callback as progress status callback
        /// </summary>
        /// <param name="callback">Callback to use for progress indication</param>
        protected BackgroundTask(Action<TaskProgress> callback)
        {
            Callback = callback;
        }

        /// <summary>
        /// Constructor that registers an empty callback as progress status callback
        /// </summary>
        protected BackgroundTask()
            : this(_ => { })
        {
            
        }
        
        /// <summary>
        /// This method implements the actual logic of the task, and is implemented by sub classes.
        /// </summary>
        protected abstract void Task();

        /// <summary>
        /// Run this task in the background.
        /// </summary>
        public void Run()
        {
            // Do not allow multiple concurrent runs
            if (IsRunning)
                throw new InvalidOperationException("Detected concurrent runs of same BackgroundTask instance");
            
            System.Threading.Tasks.Task.Run(
                () =>
                {
                    IsRunning = true;
                    Task();
                    IsRunning = false;
                }
            );
        }
    }
}