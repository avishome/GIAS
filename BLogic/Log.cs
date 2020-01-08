using System;

namespace BLogic
{
    public delegate void Callback(LogEvent log);
    internal class Log
    {
        public event Callback Status;
        public Log()
        {
            
        }

        public Log(Callback callback)
        {
            this.Status += callback;
        }

        public void LogMessege(string msg) {
            Status(new LogEvent(msg));
        }
        public LogEvent LogMessege(string msg, Boolean callback)
        {
            LogEvent forReturn = new LogEvent(msg);
            Status(forReturn);
            return returnCallBack(callback, forReturn);
        }

        public LogEvent LogMessege(string msg, LogEvent logEvent, Boolean callback) {
            LogEvent forReturn = new LogEvent(msg);
            logEvent.Status(forReturn);
            return returnCallBack(callback, forReturn);
        }
        private static LogEvent returnCallBack(bool callback, LogEvent forReturn)
        {
            if (!callback) return null;
            return forReturn;
        }

        //Status += new Callback(func);
        //public void func(LogEvent x) {}
    }
}