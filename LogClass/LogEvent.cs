using System;

namespace LogClass
{
    
    public class LogEvent
    {
        public Callback Status;
        private string v;
        public LogEvent(string v)
        {
            this.v = v;
            
        }

        public string getMessege()
        {
            return v;
        }
        public void LogMessege(string msg, Boolean callback)
        {
            this.Status(new LogEvent(msg));
        }


        public void Subscribe(Callback callback)
        {
            this.Status+=callback;
        }
    }
}