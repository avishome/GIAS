using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Entities
{
    public class Cluster : IEnumerable<Report>, INotifyPropertyChanged
    {
        public String Id { get; set; }
        public ICollection<Report> list { get; set; }
        public Report RealAdress { get; set; }
        public Report AdrressByAlgo { get; set; }
        public int Length { get { return list.Count(); } set { } }
        public Report RealPlace { get { foreach (Report r in list) if (!(r.pic is null) && (r.pic != "")) return r; return RealAdress; } set { RealAdress = value; } }
        public Cluster(Report report)
        {
            list = new LinkedList<Report>();
            Id = generateID();
            list.Add(report);
        }

        public Cluster()
        {
            list = new LinkedList<Report>();
            Id = generateID();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public IEnumerator<Report> GetEnumerator()
        {
            return ((IEnumerable<Report>)list).GetEnumerator();
        }

        public void push(Report item)
        {
            list.Add(item);
        }

        public int size()
        {
            return list.Count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Report>)list).GetEnumerator();
        }

        public bool Belong(Report report)
        {
            if (list.OrderByDescending(t => t.GetDateTime()).First().GetDateTime() <= report.GetDateTime().AddMinutes(10) &&
                list.OrderByDescending(t => t.GetDateTime()).Last().GetDateTime() >= report.GetDateTime().AddMinutes(-10))
                return true;
            return false;
        }

        public void SetAdrressByAlgo(double v1, double v2)
        {
            AdrressByAlgo = new Report() { Id = generateID(), p1 = v1.ToString(), p2 = v2.ToString() };
        }
    }
}