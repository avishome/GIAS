
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Report 
    {
        [Key]
        public string Id { get; set; }
        public string ClusterId { get; set; }
        public Cluster Cluster { get; set; }
        public bool IsAccurate { get; set; }
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
            if (p2.Length != 0) IsAccurate = true; else IsAccurate = false;
            this.Id = id;
            //loc = new Adrress("ff");
        }
        public Report()
        {
            IsAccurate = false;
        }



        public DateTime GetDateTime()
        {
            return dateTime;
        }

        public bool needAttr()
        {
            if(loc is null || loc.display_name == "") return true;
            return false;
        }

    }
}