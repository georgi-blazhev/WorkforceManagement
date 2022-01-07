using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.DAL.Data
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<TimeOffRequest> TimeOffRequests { get; set; }
        public DbSet<UserTimeOffRequest> UserTimeOffRequests { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)       
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>().HasMany(t => t.Members).WithMany(o => o.Teams);

            modelBuilder.Entity<Team>().HasOne(t => t.TeamLeader);

            modelBuilder.Entity<Team>().HasOne(t => t.Creator);

            modelBuilder.Entity<TimeOffRequest>()
                .HasMany(t => t.Approvers)
                .WithMany(t => t.RequestsRequiringDecision)
                .UsingEntity<UserTimeOffRequest>(ut => ut.HasOne<User>().WithMany().HasForeignKey(u => u.ApproverId),
                ut => ut.HasOne<TimeOffRequest>().WithMany().HasForeignKey(t => t.TimeOffRequestId))
                .Property(ut => ut.Decision);

            modelBuilder.Entity<TimeOffRequest>().HasOne(t => t.Creator);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
