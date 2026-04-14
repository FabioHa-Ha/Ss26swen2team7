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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TransportType>().HasData(
                new TransportType { Id = 1, Name = "Hike" },
                new TransportType { Id = 2, Name = "Bike" },
                new TransportType { Id = 3, Name = "Running" },
                new TransportType { Id = 4, Name = "Vacation" }
            );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<TourLog> TourLogs { get; set; }
    }
}