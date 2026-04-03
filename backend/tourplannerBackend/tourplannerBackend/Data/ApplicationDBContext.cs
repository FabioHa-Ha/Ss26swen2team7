using Microsoft.EntityFrameworkCore;
using tourplannerBackend.Model;

namespace tourPlannerBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<TourLog> TourLogs { get; set; }
    }
}