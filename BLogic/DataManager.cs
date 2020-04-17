using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using LogClass;
using Dl;
using System.Linq;

namespace BLogic
{
    public class DataManager
    {
        private Log log;

        private List<Cluster> list = new List<Cluster>();

        public (int t, int f) Analize(double v)
        {
            int t = 0;
            int f = 0;
            foreach (Cluster c in list) {
                CalcAvgLoc(c);
                c.RealPlace = c.list.FirstOrDefault();
                if (DistanceTo(double.Parse(c.RealPlace.p1), double.Parse(c.RealPlace.p2),
                    double.Parse(c.AdrressByAlgo.p1), double.Parse(c.AdrressByAlgo.p2)) > v)
                    f++;
                else
                    t++;
            }
            return (t, f);
        }



        public double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }

        public List<Cluster> List { get { return list; } set { } }
        public List<Report> Point { get {
                List<Report> temp = new List<Report>();
                foreach (Cluster i in list) 
                    if(!(i.FirstOrDefault() is null && i.size()<2))temp.Add(i.FirstOrDefault()); 
                    else if(!(i.AdrressByAlgo is null)) temp.Add(i.AdrressByAlgo);
                return temp;
            } }

        public DataManager(Log log)
        {
            this.log = log;
        }

        public async Task InputFromUrl(string url)
        {
            LogEvent task = log.LogMessege("connect to remote service "+ url,true);

            OnlineStreem Streem = new OnlineStreem(url, task);
            await Streem.PostCallAPI();

            log.LogMessege("data recived: " + Streem.data.Count +" items", task ,false);

            Catalog(Streem.toReportList());

            log.LogMessege("there are: " + list.Count + " Clusters | Done", task, false);
            
         }

        public async Task labelDataAsync(string s,string token)
        {
            LogEvent task = log.LogMessege("labeld data", true);
            foreach (Cluster c in list)
            {
                foreach (Report r in c)
                {
                    if (r.needAttr())
                    {
                        await FindLocAsync(r, s, token, task);


                    }
                    
                }
                break;
            }
        }

        private async System.Threading.Tasks.Task<string> FindLocAsync(Report r, string s, string token, LogEvent task)
        {
            string request = string.Format(s, r.p1, r.p2, token);
            task.LogMessege("sent request to: " + request, true);
            OnlineStreem Streem = new OnlineStreem(request, task);
            await Streem.PostCallAPI();
            r.loc = Streem.getLoc();
            task.LogMessege(r.loc.display_name, false);
            return r.loc.display_name;
        }

        private void Catalog(Cluster reports)
        {
            foreach(Report report in reports)
            {
                if (!findCluster(report))
                    createNewCluster(report);
            }
        }

        public void CalcAvgLoc(Cluster reports)
        {
            double x = 0;
            double y = 0;
            int num = reports.size();
            foreach (Report report in reports)
            {
                try
                {
                    double Getx = Double.Parse(report.p1);
                    double GetY = Double.Parse(report.p2);
                    x += Getx;
                    y += GetY;
                }
                catch { }
                
            }
            reports.SetAdrressByAlgo(x/num, y/num);
        }

        private void createNewCluster(Report report)
        {
            list.Add(new Cluster(report));
            log.LogMessege("create new Clusters at " + report.GetDateTime().ToString(), false);
        }

        private bool findCluster(Report report)
        {
            foreach (Cluster c in list) {
                if (c.Belong(report)) {
                    c.push(report);
                    CalcAvgLoc(c); /// get real location by algo
                    
                    return true;
                }
            }
            return false;
        }
        public void Save() {
            using (var ctx = new DB())
            {
                ctx.C.AddRange(list);
                ctx.SaveChanges();
            }
        }
        public void viewData() {
            using (var context = new DB())
            {
                var studentAndCourseList = context.R.ToList();

                foreach (var item in studentAndCourseList)
                {
                    Console.WriteLine("Report Id: "+ item.Id);
                }
            }
        }
    }
}