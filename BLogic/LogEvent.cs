using System;

namespace BLogic
{
    
    public class LogEvent
    {
        public Callback Status;
        private string v;

        public LogEvent(string v)
        {
            this.v = v;
            
        }

        internal string getMessege()
        {
            return v;
        }
        public void LogMessege(string msg, Boolean callback)
        {
            this.Status(new LogEvent(msg));
        }

        internal void Subscribe(Callback callback)
        {
            this.Status+=callback;
        }
    }
}