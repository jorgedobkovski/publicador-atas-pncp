using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PublicadorARP.Models.Auth;
using System.Globalization;

namespace PublicadorARP.Context
{
    public class DataContext : IdentityDbContext<User, Profile, int>
    {
        private string connectionString = "Data Source=172.16.0.40; Persist Security Info=True; Initial Catalog=supel_db11; User ID=u_supel_escrita; Password=Supel2.0;MultipleActiveResultSets=True;TrustServerCertificate=True";

        public DataContext()
        {

        }
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            optionsBuilder                
                .UseSqlServer(connectionString)
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("PublicadorARP");
        }
    }
}
