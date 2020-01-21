using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Entities
{
    public class Cluster : IEnumerable<Report>
    {
        List<Report> list = new List<Report>();


        public Cluster(Report report)
        {
            list.Add(report);
        }

        public Cluster()
        {
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
    }
}