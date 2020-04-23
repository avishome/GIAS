using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Entities
{
    public class Cluster : IEnumerable<Report>
    {
        [Key]
        public String Id { get; set; }
        public ICollection<Report> list { get; set; }
        [NotMapped]
        public Report RealAdress { get; set; }
        [NotMapped]
        public Report AdrressByAlgo { get; set; }
        public int Length { get { return list.Count(); } set { } }
        [NotMapped]
        public Report RealPlace { get { foreach (Report r in list) if (!(r.pic is null) && (r.pic != "")) return r; return RealAdress; } set { RealAdress = value; } }
        public Cluster(Report report)
        {
            list = new LinkedList<Report>();
            Id = generateID();
            list.Add(report);
            RealAdress = report;
            AdrressByAlgo = report;

        }

        public Cluster()
        {
            list = new LinkedList<Report>();
            Id = generateID();
        }


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