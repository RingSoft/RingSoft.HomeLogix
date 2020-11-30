using Microsoft.EntityFrameworkCore;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class HomeLogixDbContext : DbContext, IHomeLogixDbContext
    {
        public DbContext DbContext => this;

        public DbSet<SystemMaster> SystemMaster { get; set; }

        private static HomeLogixLookupContext _lookupContext;

        public HomeLogixDbContext(HomeLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_lookupContext.SqliteDataProcessor.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemMaster>().HasKey(p => p.HouseholdName);

            base.OnModelCreating(modelBuilder);
        }
    }
}
