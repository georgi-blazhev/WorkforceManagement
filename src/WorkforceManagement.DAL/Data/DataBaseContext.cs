using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.DAL.Data
{
    [ExcludeFromCodeCoverage]
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<TimeOffRequest> TimeOffRequests { get; set; }
        public DbSet<UserTimeOffRequest> UserTimeOffRequests { get; set; }
        public DbSet<Holiday> OfficialHolidays { get; set; }
        public DbSet<DayOff> DaysOff { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)       
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>().HasMany(t => t.Members).WithMany(o => o.Teams);
            modelBuilder.Entity<Team>().HasOne(t => t.TeamLeader).WithOne().OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Team>().HasOne(t => t.Creator).WithMany().OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<TimeOffRequest>()
                .HasMany(t => t.Approvers)
                .WithMany(t => t.RequestsRequiringDecision)
                .UsingEntity<UserTimeOffRequest>(ut => ut.HasOne<User>().WithMany().HasForeignKey(u => u.ApproverId),
                ut => ut.HasOne<TimeOffRequest>().WithMany().HasForeignKey(t => t.TimeOffRequestId))
                .Property(ut => ut.Decision);

            modelBuilder.Entity<TimeOffRequest>().HasOne(t => t.Creator);

            // Field sizes
            modelBuilder.Entity<User>()
                .Property(p => p.FirstName)
                .HasMaxLength(50);
            modelBuilder.Entity<User>()
                .Property(p => p.LastName)
                .HasMaxLength(50);

            modelBuilder.Entity<Team>()
                .Property(p => p.Title)
                .HasMaxLength(50);
            modelBuilder.Entity<Team>()
                .Property(p => p.Description)
                .HasMaxLength(240);

            modelBuilder.Entity<TimeOffRequest>()
                .Property(t => t.Reason)
                .HasMaxLength(240);

            modelBuilder.Entity<UserTimeOffRequest>()
                .Property(t => t.Decision)
                .HasMaxLength(30);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
