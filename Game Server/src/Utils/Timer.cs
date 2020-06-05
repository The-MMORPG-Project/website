using System.Timers;

namespace Valk.Networking 
{
    class Timer
    {
        private System.Timers.Timer timer;

        public Timer(double interval, ElapsedEventHandler function) 
        {
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += function;
            timer.AutoReset = true;
        }

        public void Start() 
        {
            timer.Start();
        }

        public void Stop() 
        {
            timer.Stop();
        }

        public void Dispose() 
        {
            timer.Dispose();
        }
    }
}