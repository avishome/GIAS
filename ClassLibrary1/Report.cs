using PropertyChanged;
using System;
using System.ComponentModel;

namespace Entities
{
    public class Report : INotifyPropertyChanged
    {
        
        public string Id { get; set; }
        public DateTime dateTime { get; set; }
        public string p1 { get; set; }
        public string p2 { get; set; }
        public string pic { get; set; }
        public Adrress loc { get; set; }
        public string picture { get {
                return "http://www.us-central1-vivid-fragment-225620.cloudfunctions.net/download?file=" + pic; } }
                public Report(string id, DateTime dateTime, string[] p1, string p2)
        {
            this.dateTime = dateTime;
            if (p1.Length != 2) throw new Exception("position not in good format" + p1.ToString());
            this.p1 = p1[0];
            this.p2 = p1[1];
            if (p2.Length != 0) pic = p2;
            this.Id = id;
            //loc = new Adrress("ff");
        }
        public Report()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime GetDateTime()
        {
            return dateTime;
        }

        public bool needAttr()
        {
            if(!(loc is null)) return true;
            return false;
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