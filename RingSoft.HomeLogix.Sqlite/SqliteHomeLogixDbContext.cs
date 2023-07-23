using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class SqliteHomeLogixDbContext : DbContextEfCore, IHomeLogixDbContext
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
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<BankTransactionBudget> BankTransactionBudget { get; set; }
        public DbSet<QifMap> QifMaps { get; set; }
        public DbSet<MainBudget> MainBudget { get; set; }

        //-----------------------------------------------------------------------
        private static string? _connectionString;

        public static string? ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    return _lookupContext.SqliteDataProcessor.ConnectionString;
                }
                return _connectionString;
            }
            set { _connectionString = value; }
        }


        private static HomeLogixLookupContext _lookupContext;

        public void SetLookupContext(HomeLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            DbConstants.ConstantGenerator = new SqliteDbConstants();
        }
        public SqliteHomeLogixDbContext()
        {
            DbConstants.ConstantGenerator = new SqliteDbConstants();
            //EfCoreGlobals.DbAdvancedFindContextCore = this;
            //SystemGlobals.AdvancedFindDbProcessor = new AdvancedFindDataProcessorEfCore();
            HomeLogixModelBuilder.DbContext = this;
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
            DbConstants.ConstantGenerator = new SqliteDbConstants();
            HomeLogixModelBuilder.BuildModel(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public DbContext GetDbContextEf()
        {
            return this;
        }

        public override DbContextEfCore GetNewDbContextEfCore()
        {
            return new SqliteHomeLogixDbContext();
        }

        public IAdvancedFindDbContextEfCore GetNewDbContext()
        {
            return new SqliteHomeLogixDbContext();
        }
        public override void SetProcessor(DbDataProcessor processor)
        {

        }

        public override void SetConnectionString(string? connectionString)
        {
            ConnectionString = connectionString;
        }

    }
}
