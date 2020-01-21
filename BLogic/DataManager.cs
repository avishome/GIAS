using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
namespace BLogic
{
    internal class DataManager
    {
        private Log log;

            private List<Cluster> list = new List<Cluster>();

        public DataManager(Log log)
        {
            this.log = log;
        }

        public async Task InputFromAsync(string url)
        {
            LogEvent task = log.LogMessege("connect to remote service "+ url,true);

            OnlineStreem Streem = new OnlineStreem(url, task);
            await Streem.PostCallAPI();

            log.LogMessege("data recived: " + Streem.data.Count +" items", task ,false);

            Catalog(Streem.toReportList());

            log.LogMessege("there are: " + list.Count + " Clusters", task, false);
            
         }

        public async Task labelDataAsync(string s,string token)
        {
            LogEvent task = log.LogMessege("labeld data", true);
            foreach (Cluster c in list)
            {
                foreach (Report r in c)
                    if (r.needAttr())
                    {
                        FindLocAsync(r,s, token, task);

                        break;
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
            r.SetLoc(Streem.getLoc());
            task.LogMessege(r.loc, false);
            return r.loc;
        }

        private void Catalog(Cluster reports)
        {
            foreach(Report report in reports)
            {
                if (!findCluster(report))
                    createNewCluster(report);
            }
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
                    return true;
                }
            }
            return false;
        }
    }
}