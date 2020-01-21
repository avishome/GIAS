using System;

namespace Entities
{
    public class Report
    {
        private string id;
        private DateTime dateTime;
        public string p1;
        public string p2;
        private string pic="null";
        public string loc = "";

        public Report(string id, DateTime dateTime, string[] p1, string p2)
        {
            this.dateTime = dateTime;
            if (p1.Length != 2) throw new Exception("position not in good format"+ p1.ToString());
            this.p1 = p1[0];
            this.p2 = p1[1];
            if(p2.Length!=0) pic = p2;
            this.id = id;
        }

        public DateTime GetDateTime()
        {
            return dateTime;
        }

        public bool needAttr()
        {
            if(loc=="") return true;
            return false;
        }

        public void SetLoc(string v)
        {
            loc = v;
        }
        /*public async System.Threading.Tasks.Task<string> FindLocAsync(string s, string token, LogEvent task)
        {
            string request = string.Format(s, p1, p2, token);
            task.LogMessege("sent request to: " + request, true);
            OnlineStreem Streem = new OnlineStreem(request, task);
            await Streem.PostCallAPI();
            SetLoc(Streem.getLoc());
            task.LogMessege(loc, false);
            return loc;
        }*/
    }
}