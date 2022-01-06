using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.DAL.Data
{
    public class DataBaseContext : IdentityDbContext<User>
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<TimeOffRequest> TimeOffRequests { get; set; }
        public DataBaseContext(DbContextOptions<DataBaseContext> options ) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)       
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Team>().HasMany(t => t.Members).WithMany(o => o.Teams);
            modelBuilder.Entity<Team>().HasOne(t => t.TeamLeader);
            modelBuilder.Entity<Team>().HasOne(t => t.Creator);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
