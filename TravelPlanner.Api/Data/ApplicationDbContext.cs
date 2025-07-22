using Microsoft.EntityFrameworkCore;
using TravelPlanner.Api.Models;

namespace TravelPlanner.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Stops)
                .WithOne(s => s.Trip)
                .HasForeignKey(s => s.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ FIX: Define precision for decimal properties to remove warnings
            modelBuilder.Entity<Trip>(e =>
            {
                e.Property(p => p.Budget).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Stop>(e =>
            {
                e.Property(p => p.EstimatedCost).HasColumnType("decimal(18, 2)");
            });
        }
    }
}