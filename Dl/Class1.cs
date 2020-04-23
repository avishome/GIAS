
using System.Security.Claims;
using System.Threading.Tasks;
using Entities;
using System.Data.Entity;

namespace Dl
{
    public class DB14 : DbContext
    {
        //Data Source=http://35.184.140.193;Initial Catalog=iconic-exchange-226008:us-central1:giasdb;Persist Security Info=True;User ID=sqlserver;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False
        public DB14(): base(){
            
        }
        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Report>().ToTable("Report", "Entity");
            modelBuilder.Entity<Cluster>().ToTable("Cluster", "Entity");
            base.OnModelCreating(modelBuilder);
        }*/
        public DbSet<Report> R { get; set; }
        public DbSet<Cluster> C { get; set; }
        public DbSet<Adrress> A { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<Report>()
                .HasRequired<Cluster>(s => s.Cluster)
                .WithMany(g => g.list)
                .HasForeignKey<string>(s => s.ClusterId);
        }
    }
}

