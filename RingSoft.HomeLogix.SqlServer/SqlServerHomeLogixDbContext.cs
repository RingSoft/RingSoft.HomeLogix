using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.SqlServer
{
    public class SqlServerHomeLogixDbContext : DbContextEfCore, IHomeLogixDbContext
    {
        public DbContext DbContext => this;
        public DbSet<SystemMaster> SystemMaster { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountRegisterItem> BankAccountRegisterItems { get; set; }
        public DbSet<BankAccountRegisterItemAmountDetail> BankAccountRegisterItemAmountDetails { get; set; }
        public DbSet<BudgetItemSource> BudgetItemSources { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<SourceHistory> SourceHistory { get; set; }
        public DbSet<BudgetPeriodHistory> BudgetPeriodHistory { get; set; }
        public DbSet<BankAccountPeriodHistory> BankAccountPeriodHistory { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<BankTransactionBudget> BankTransactionBudget { get; set; }
        public DbSet<QifMap> QifMaps { get; set; }
        public DbSet<MainBudget> MainBudget { get; set; }

        public bool IsDesignTime { get; set; }

        private static HomeLogixLookupContext _lookupContext;

        public SqlServerHomeLogixDbContext()
        {
            //DbConstants.ConstantGenerator = new SqlServerDbConstants();
            EfCoreGlobals.DbAdvancedFindContextCore = this;
            SystemGlobals.AdvancedFindDbProcessor = new AdvancedFindDataProcessorEfCore();
            HomeLogixModelBuilder.DbContext = this;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsDesignTime)
            {
                var sqlProcessor = new SqlServerDataProcessor();
                sqlProcessor.Server = "localhost\\SQLEXPRESS";
                sqlProcessor.Database = "RSHomeLogixTemp";
                sqlProcessor.SecurityType = SecurityTypes.WindowsAuthentication;
                optionsBuilder.UseSqlServer(sqlProcessor.ConnectionString);
            }
            else
                optionsBuilder.UseSqlServer(_lookupContext.SqlServerDataProcessor.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
            HomeLogixModelBuilder.BuildModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }


        public DbContext GetDbContextEf()
        {
            return this;
        }

        public override DbContextEfCore GetNewDbContextEfCore()
        {
            return new SqlServerHomeLogixDbContext();
        }

        public IAdvancedFindDbContextEfCore GetNewDbContext()
        {
            return new SqlServerHomeLogixDbContext();
        }

        public void SetLookupContext(HomeLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            _lookupContext.LocalDbContext = this;
            DbConstants.ConstantGenerator = new SqlServerDbConstants();

        }
    }
}
