using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.EfCore;
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
        public virtual DbSet<BudgetItemSource> BudgetItemSources { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<SourceHistory> SourceHistory { get; set; }
        public virtual DbSet<BudgetPeriodHistory> BudgetPeriodHistory { get; set; }
        public virtual DbSet<BankAccountPeriodHistory> BankAccountPeriodHistory { get; set; }
        public DbSet<AdvancedFind> AdvancedFinds { get; set; }
        public DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }

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
            modelBuilder.ApplyConfiguration(new HistoryConfiguration());
            modelBuilder.ApplyConfiguration(new SourceHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetPeriodHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new BankAccountPeriodHistoryConfiguration());

            modelBuilder.Entity<BudgetItemSource>().Property(p => p.Id)
                .HasColumnType(SqliteConstants.IntegerColumnType);
            modelBuilder.Entity<BudgetItemSource>().Property(p => p.IsIncome)
                .HasColumnType(SqliteConstants.BoolColumnType);
            modelBuilder.Entity<BudgetItemSource>().Property(p => p.Name)
                .HasColumnType(SqliteConstants.StringColumnType);

            AdvancedFindDataProcessorEfCore.ConfigureAdvancedFind(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public DbContext GetDbContextEf()
        {
            return this;
        }

        public IAdvancedFindDbContextEfCore GetNewDbContext()
        {
            return new HomeLogixDbContext();
        }
    }
}
