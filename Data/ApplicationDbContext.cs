using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MileageExpenseTracker.Models;

namespace MileageExpenseTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<MileageClaim> MileageClaims { get; set; }
        public DbSet<MileageTrip> MileageTrips { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure MileageClaim
            modelBuilder.Entity<MileageClaim>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RatePerKm).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TotalKilometers).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TotalReimbursement).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Approver)
                    .WithMany()
                    .HasForeignKey(e => e.ApproverId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.EmployeeId, e.Status, e.StartDate });
            });

            // Configure MileageTrip
            modelBuilder.Entity<MileageTrip>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Kilometers).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Reimbursement).HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.Claim)
                    .WithMany(c => c.Trips)
                    .HasForeignKey(e => e.ClaimId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.ClaimId, e.TripDate });
            });
        }
    }
}
