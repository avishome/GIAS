using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dl
{
    public class DB : DbContext
    {
        public DB(): base(){

        }
        public DbSet<Report> R { get; set; }
        public DbSet<Cluster> C { get; set; }
    }
}
