using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class HomeLogixDbContext : DbContext, IHomeLogixDbContext
    {
        //install Microsoft.EntityFrameworkCore.Tools NuGet

        //Restart Visual Studio.

        //Add-Migration <Name>

        //Remove-Migration <Name>

        public DbContext DbContext => this;

        public bool IsDesignTime { get; set; }

        //-----------------------------------------------------------------------
        public virtual DbSet<SystemMaster> SystemMaster { get; set; }
        public virtual DbSet<BudgetItem> BudgetItems { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<BankAccountRegisterItem> BankAccountRegisterItems { get; set; }
        public virtual DbSet<BankAccountRegisterItemAmountDetail> BankAccountRegisterItemAmountDetails { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        //-----------------------------------------------------------------------

        private static HomeLogixLookupContext _lookupContext;
        
        public HomeLogixDbContext(HomeLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
        }

        public HomeLogixDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsDesignTime)
                optionsBuilder.UseSqlite("DataSource=C:\\");
            else 
                optionsBuilder.UseSqlite(_lookupContext.SqliteDataProcessor.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemMaster>().Property(p => p.HouseholdName)
                .HasColumnType(SqliteConstants.StringColumnType);

            modelBuilder.ApplyConfiguration(new BudgetItemConfiguration());
            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
            modelBuilder.ApplyConfiguration(new BankAccountRegisterItemConfiguration());
            modelBuilder.ApplyConfiguration(new BankAccountRegisterItemAmountConfiguration());

            modelBuilder.Entity<Store>().Property(p => p.Id)
                .HasColumnType(SqliteConstants.IntegerColumnType);
            modelBuilder.Entity<Store>().Property(p => p.Name)
                .HasColumnType(SqliteConstants.StringColumnType);

            base.OnModelCreating(modelBuilder);
        }
    }
}
