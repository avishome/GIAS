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
            foreach (Cluster c in List) {
                CalcAvgLoc(c);
                //c.RealPlace = c.list.FirstOrDefault();
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

        public List<Cluster> List { get
            {
                if (list == null)
                    NewMethod();
                return list;
            }
                    set { list = value; } }

        private void NewMethod()
        {
            using (var context = new DB14())
            {
                list = context.C.ToList();
                //var temp2 = context.A.ToList();
                foreach (Cluster c in list)
                {

                    c.list = context.R.Where(l => l.ClusterId == c.Id).ToList();
                    Report def = c.list.FirstOrDefault();
                    Report gooddef = context.R.Where(l => l.ClusterId == c.Id && (l.pic != "" && l.pic != null)).FirstOrDefault();
                    c.RealPlace = (gooddef != null) ? gooddef : def;
                }
            }
        }

        public List<Report> Point { get {
                List<Report> temp = new List<Report>();
                foreach (Cluster i in List) 
                    if(!(i.FirstOrDefault() is null && i.size()<2))temp.Add(i.FirstOrDefault()); 
                    else if(!(i.AdrressByAlgo is null)) temp.Add(i.AdrressByAlgo);
                return temp;
            } }

        public DataManager(Log log)
        {
            this.log = log;
        }

        public void addReport(Report report)
        {
            Catalog(new Cluster(report));
        }

        public async Task InputFromUrl(string url)
        {
            LogEvent task = log.LogMessege("connect to remote service "+ url,true);

            OnlineStreem Streem = new OnlineStreem(url, task);
            await Streem.PostCallAPI();

            log.LogMessege("data recived: " + Streem.data.Count +" items", task ,false);

            Catalog(Streem.toReportList());

            log.LogMessege("there are: " + List.Count + " Clusters | Done", task, false);
            
         }

        public async Task labelDataAsync(string s,string token)
        {
            using (var context = new DB14())
            {
                var a = context.A.ToList();
                List = context.C.ToList();

                LogEvent task = log.LogMessege("labeld data", true);
                
                foreach (Cluster c in List)
                {
                    var d = context.R.ToList();
                    c.list = context.R.Where(l => l.ClusterId == c.Id).ToList();
                    
                    foreach (Report r in c)
                    {
               
                        if (r.needAttr())
                        {
                            await FindLocAsync(r, s, token, task);
                            
                        }
                    }
                    context.SaveChanges();
                    
                }
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
            using (var context = new DB14())
            {
                log.LogMessege("create new");
                var nr = new Cluster(report);
                //context.R.Add(report);
                context.C.Add(nr);
                log.LogMessege("try save the new one");
                context.SaveChanges();
                //list.Add();
                log.LogMessege("create new Clusters at " + report.GetDateTime().ToString(), false);
            }
        }

        private bool findCluster(Report report)
        {
            if (exist(report)) return true;

            using (var context = new DB14())
            {
                var studentAndCourseList = context.C.ToList();
                if (studentAndCourseList.Count == 0) return false;

                foreach (Cluster c in studentAndCourseList)
                {
                    c.list = context.R.Where(l => l.ClusterId == c.Id).ToList();
                    if (c.Belong(report))
                    {
                        c.push(report);
                        CalcAvgLoc(c); /// get real location by algo

                        context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        private bool exist(Report report) {
            using (var context = new DB14())
            {
                var studentAndCourseList = context.R.ToList();
                var loc = context.A.ToList();
                foreach (var item in studentAndCourseList)
                {
                    if (item.Id == report.Id)
                    {
                        log.LogMessege("Report Id exist: " + item.Id);
                        return true;
                    }
                }
                log.LogMessege("Report dont find at data base" + report.Id);
                return false;
            }
        }
        public void Save() {
            using (var ctx = new DB14())
            {
                var t1 = ctx.R.ToList();
                var t2 = ctx.C.ToList();
                var t3 = ctx.A.ToList();
                foreach (var l in List) {
                    var resToUpdate = ctx.C.AsNoTracking().Select(x => x.Id == l.Id);
                    if (resToUpdate != null)
                    {
                        ctx.C.Attach(l);
                        ctx.Entry(l).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                        ctx.C.Add(l);
                }
                log.LogMessege("saveing");
                ctx.SaveChanges();
            }
        }
        public void viewData() {
            using (var context = new DB14())
            {
                log.LogMessege("hi");
                var studentAndCourseList = context.R.ToList();
                log.LogMessege(studentAndCourseList.Count.ToString());
                foreach (var item in studentAndCourseList)
                {
                    log.LogMessege("Report Id: "+ item.Id);
                }
            }
        }
        public Cluster GetListReports()
        {
            using (var context = new DB14())
            {
                var addres = context.A.ToList();
                var reports = context.R.ToList();
                return new Cluster() { list = reports };
            }
        }
    }
}