using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class HomeLogixLookupContext : LookupContext, IAdvancedFindLookupContext
    {
        public const int RegisterTypeCustomContentId = 100;
        public const int BudgetItemTypeContentId = 101;

        public TableDefinition<SystemMaster> SystemMaster { get; set; }
        public TableDefinition<BudgetItem> BudgetItems { get; set; }
        public TableDefinition<BankAccount> BankAccounts { get; set; }
        public TableDefinition<BankAccountRegisterItem> BankAccountRegisterItems { get; set; }
        public TableDefinition<BankAccountRegisterItemAmountDetail> BankAccountRegisterItemAmountDetails { get; set; }
        public TableDefinition<BudgetItemSource> BudgetItemSources { get; set; }
        public TableDefinition<History> History { get; set; }
        public TableDefinition<SourceHistory> SourceHistory { get; set; }
        public TableDefinition<BudgetPeriodHistory> BudgetPeriodHistory { get; set; }
        public TableDefinition<BankAccountPeriodHistory> BankAccountPeriodHistory { get; set; }
        public TableDefinition<BankTransaction> BankTransactions { get; set; }
        public TableDefinition<QifMap> QifMaps { get; set; }
        public LookupContextBase Context => this;
        public TableDefinition<RecordLock> RecordLocks { get; set; }
        public TableDefinition<AdvancedFind> AdvancedFinds { get; set; }
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }
        public LookupDefinition<AdvancedFindLookup, AdvancedFind> AdvancedFindLookup { get; set; }
        public LookupDefinition<RecordLockingLookup, RecordLock> RecordLockingLookup { get; set; }
        public LookupDefinition<BudgetItemLookup, BudgetItem> BudgetItemsLookup { get; set; }
        public LookupDefinition<BankAccountLookup, BankAccount> BankAccountsLookup { get; set; }
        public LookupDefinition<BankAccountRegisterLookup, BankAccountRegisterItem> BankRegisterLookup { get; set; }
        public LookupDefinition<BankRegisterAmountDetailsLookup, BankAccountRegisterItemAmountDetail> BankRegisterAmountDetailsLookup { get; set; }
        public LookupDefinition<SourceLookup, BudgetItemSource> BudgetItemSourceLookup { get; set; }
        public LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory> BudgetPeriodLookup { get; set; }

        public LookupDefinition<HistoryLookup, History> HistoryLookup { get; set; }
        public LookupDefinition<SourceHistoryLookup, SourceHistory> SourceHistoryLookup { get; set; }

        public LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory> BankPeriodHistoryLookup { get; set; }
        public LookupDefinition<QifMapLookup, QifMap> QifMapLookup { get; set; }
        //----------------------------------------------------------------------

        public override DbDataProcessor DataProcessor
        {
            get
            {
                switch (DbPlatform)
                {
                    case DbPlatforms.Sqlite:
                        return SqliteDataProcessor;
                    case DbPlatforms.SqlServer:
                        return SqlServerDataProcessor;
                    //case DbPlatforms.MySql:
                    //    break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public SqliteDataProcessor SqliteDataProcessor { get; }

        public SqlServerDataProcessor SqlServerDataProcessor { get; }

        public DbPlatforms DbPlatform { get; set; }
        
        protected override DbContext DbContext => LocalDbContext;

        public DbContext LocalDbContext { get; set; }

        private bool _initialized;

        public HomeLogixLookupContext()
        {
            SqliteDataProcessor = new SqliteDataProcessor();
            SqlServerDataProcessor = new SqlServerDataProcessor();
        }

        public void Initialize(IHomeLogixDbContext dbContext, DbPlatforms platform)
        {
            DbPlatform = platform;
            LocalDbContext = dbContext.DbContext;
            if (_initialized)
                return;
            SystemGlobals.AdvancedFindLookupContext = this;
            var configuration = new AdvancedFindLookupConfiguration(SystemGlobals.AdvancedFindLookupContext);
            configuration.InitializeModel();
            configuration.ConfigureLookups();

            Initialize();
            _initialized = true;
        }

        public void TestInitialize()
        {
            InitializeLookupDefinitions();
        }

        protected override void InitializeLookupDefinitions()
        {
            BudgetItemsLookup = new LookupDefinition<BudgetItemLookup, BudgetItem>(BudgetItems);
            BudgetItemsLookup.AddVisibleColumnDefinition(p => p.Description, "Budget Item",
                p => p.Description, 35);
            BudgetItemsLookup.AddVisibleColumnDefinition(p => p.ItemType, "Item\r\nType",
                p => p.Type, 20);

            BudgetItemsLookup.AddVisibleColumnDefinition(p => p.RecurringPeriod, "Recurs\r\nEvery",
                p => p.RecurringPeriod, 10).HasHorizontalAlignmentType(LookupColumnAlignmentTypes.Left);

            BudgetItemsLookup.AddVisibleColumnDefinition(p => p.RecurringType, "Recurring\r\nType",
                p => p.RecurringType, 20);
            BudgetItemsLookup.AddVisibleColumnDefinition(p => p.Amount, "Amount",
                p => p.Amount, 15);

            BudgetItems.HasLookupDefinition(BudgetItemsLookup);

            BankAccountsLookup = new LookupDefinition<BankAccountLookup, BankAccount>(BankAccounts);
            BankAccountsLookup.AddVisibleColumnDefinition(p => p.Description, "Description",
                p => p.Description, 70);
            BankAccountsLookup.AddVisibleColumnDefinition(p => p.CurrentBalance, "Current Balance",
                p => p.CurrentBalance, 30);
            BankAccounts.HasLookupDefinition(BankAccountsLookup);

            BudgetItemSourceLookup = new LookupDefinition<SourceLookup, BudgetItemSource>(BudgetItemSources);
            BudgetItemSourceLookup.AddVisibleColumnDefinition(p => p.SourceName, "Source Name", 
                p => p.Name, 70);
            BudgetItemSources.HasLookupDefinition(BudgetItemSourceLookup);

            BankRegisterLookup =
                new LookupDefinition<BankAccountRegisterLookup, BankAccountRegisterItem>(BankAccountRegisterItems);
            BankRegisterLookup.AddVisibleColumnDefinition(p => p.RegisterDate, "Register Date", p => p.ItemDate, 50);
            BankRegisterLookup.AddVisibleColumnDefinition(p => p.Description, "Description", p => p.Description, 50);
            BankAccountRegisterItems.HasLookupDefinition(BankRegisterLookup);

            BankRegisterAmountDetailsLookup =
                new LookupDefinition<BankRegisterAmountDetailsLookup, BankAccountRegisterItemAmountDetail>(
                    BankAccountRegisterItemAmountDetails);
            BankRegisterAmountDetailsLookup.AddVisibleColumnDefinition(p => p.Date, "Date", p => p.Date, 20);
            BankRegisterAmountDetailsLookup.Include(p => p.Source)
                .AddVisibleColumnDefinition(p => p.Source, "Source", p => p.Name, 60);
            BankRegisterAmountDetailsLookup.AddVisibleColumnDefinition(p => p.Amount, "Amount", p => p.Amount, 20);
            BankAccountRegisterItemAmountDetails.HasLookupDefinition(BankRegisterAmountDetailsLookup);

            HistoryLookup = new LookupDefinition<HistoryLookup, History>(History);
            var budgetInclude = HistoryLookup.Include(p => p.BudgetItem);
            var budgetAlias = budgetInclude.JoinDefinition.Alias;

            HistoryLookup.AddVisibleColumnDefinition(p => p.Date, "Date",
                p => p.Date, 12);
            HistoryLookup.AddVisibleColumnDefinition(p => p.Description, "Description",
                p => p.Description, 25);
            HistoryLookup.AddVisibleColumnDefinition(p => p.ItemType, "Item Type",
                p => p.ItemType, 20);
            HistoryLookup.AddVisibleColumnDefinition(p => p.ProjectedAmount, "Budget\r\nAmount",
                p => p.ProjectedAmount, 15);
            HistoryLookup.AddVisibleColumnDefinition(p => p.ActualAmount, "Actual\r\nAmount", 
                p => p.ActualAmount, 15);
            
            var table = DataProcessor.SqlGenerator.FormatSqlObject(History.TableName);
            var projectedField = DataProcessor.SqlGenerator.FormatSqlObject(History
                .GetFieldDefinition(p => p.ProjectedAmount).FieldName);
            var actualField = DataProcessor.SqlGenerator.FormatSqlObject(History
                .GetFieldDefinition(p => p.ActualAmount).FieldName);
            budgetAlias = DataProcessor.SqlGenerator.FormatSqlObject(budgetAlias);
            var typeField = DataProcessor.SqlGenerator.FormatSqlObject(BudgetItems
                .GetFieldDefinition(p => (int)p.Type).FieldName);

            var formula = $"CASE {budgetAlias}.{typeField} WHEN {(int)BudgetItemTypes.Income}"
            + $" THEN {table}.{actualField} - {table}.{projectedField}"
            + $" ELSE {table}.{projectedField} - {table}.{actualField} END";

            HistoryLookup.Include(p => p.BudgetItem)
                .AddVisibleColumnDefinition(p => p.Difference, "Difference", formula, 15, FieldDataTypes.Decimal)
                .HasDecimalFieldType(DecimalFieldTypes.Currency).DoShowNegativeValuesInRed()
                .DoShowPositiveValuesInGreen();
            HistoryLookup.InitialOrderByType = OrderByTypes.Descending;
            History.HasLookupDefinition(HistoryLookup);

            var sourceHistoryLookupDefinition = new LookupDefinition<SourceHistoryLookup, SourceHistory>(SourceHistory);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Date,
                "Date", p => p.Date, 20);
            sourceHistoryLookupDefinition.Include(p => p.Source).
                AddVisibleColumnDefinition(p => p.Source, "Source",
                    p => p.Name, 30);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Amount, "Amount", p => p.Amount, 20);
            sourceHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.BankText, "Bank Text", p => p.BankText, 30);
            SourceHistoryLookup = sourceHistoryLookupDefinition;
            SourceHistory.HasLookupDefinition(sourceHistoryLookupDefinition);

            var budgetPeriodLookup =
                new LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory>(BudgetPeriodHistory);
            budgetPeriodLookup.AddVisibleColumnDefinition(p => p.PeriodEndingDate, "Period Ending Date",
                p => p.PeriodEndingDate, 25);
            budgetPeriodLookup.AddVisibleColumnDefinition(p => p.ProjectedAmount, "Budget Amount", 
                p => p.ProjectedAmount, 25);
            budgetPeriodLookup.AddVisibleColumnDefinition(p => p.ActualAmount, "Actual Amount",
                p => p.ActualAmount, 25);
             
            var budgetPeriodInclude = budgetPeriodLookup.Include(p => p.BudgetItem);
            budgetAlias = budgetPeriodInclude.JoinDefinition.Alias;

            table = DataProcessor.SqlGenerator.FormatSqlObject(BudgetPeriodHistory.TableName);
            var projectedAmountField = DataProcessor.SqlGenerator.FormatSqlObject(
                BudgetPeriodHistory.GetFieldDefinition(p => p.ProjectedAmount).FieldName);
            var actualAmountField = DataProcessor.SqlGenerator.FormatSqlObject(
                BudgetPeriodHistory.GetFieldDefinition(p => p.ActualAmount).FieldName);

            formula = $"CASE {budgetAlias}.{typeField} WHEN {(int)BudgetItemTypes.Income}"
                      + $" THEN {table}.{actualAmountField} - {table}.{projectedAmountField}"
                      + $" ELSE {table}.{projectedAmountField} - {table}.{actualAmountField} END";
            budgetPeriodInclude.AddVisibleColumnDefinition(p => p.Difference, "Difference"
                    , formula, 25, FieldDataTypes.Decimal)
                .HasDecimalFieldType(DecimalFieldTypes.Currency)
                .DoShowNegativeValuesInRed()
                .DoShowPositiveValuesInGreen();

            BudgetPeriodLookup = budgetPeriodLookup;

            BudgetPeriodHistory.HasLookupDefinition(BudgetPeriodLookup);

            var bankPeriodHistoryLookupDefinition =
                new LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory>(BankAccountPeriodHistory);

            bankPeriodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.PeriodEndingDate,
                "Date Ending", p => p.PeriodEndingDate, 20);
            bankPeriodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.TotalDeposits, 
                "Total Deposits", p => p.TotalDeposits, 20);
            bankPeriodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.TotalWithdrawals, 
                "Total Withdrawals", p => p.TotalWithdrawals, 20);

            var tableString = DataProcessor.SqlGenerator.FormatSqlObject(BankAccountPeriodHistory.TableName);
            var depositField = DataProcessor.SqlGenerator.FormatSqlObject(BankAccountPeriodHistory.GetFieldDefinition(p => p.TotalDeposits).FieldName);
            var withdrawalsField = DataProcessor.SqlGenerator.FormatSqlObject(
                BankAccountPeriodHistory.GetFieldDefinition(p => p.TotalWithdrawals).FieldName);

            formula = $"{tableString}.{depositField} - {tableString}.{withdrawalsField}";
            bankPeriodHistoryLookupDefinition.AddVisibleColumnDefinition(p => p.Difference,
                    "Difference", formula, 20, "")
                .HasDecimalFieldType(DecimalFieldTypes.Currency)
                .DoShowNegativeValuesInRed()
                .DoShowPositiveValuesInGreen();

            BankPeriodHistoryLookup = bankPeriodHistoryLookupDefinition;
            BankAccountPeriodHistory.HasLookupDefinition(BankPeriodHistoryLookup);

            QifMapLookup = new LookupDefinition<QifMapLookup, QifMap>(QifMaps);
            QifMapLookup.AddVisibleColumnDefinition(p => p.BankText, "Bank Text", q => q.BankText, 34);
            QifMapLookup.Include(p => p.BudgetItem)
                .AddVisibleColumnDefinition(p => p.BudgetItem, "Budget Item", p => p.Description, 33);
            QifMapLookup.Include(p => p.Source)
                .AddVisibleColumnDefinition(p => p.Source, "Source", p => p.Name, 33);
            QifMaps.HasLookupDefinition(QifMapLookup);
        }

        protected override void SetupModel()
        {
            var test = this;
            BankAccounts.HasRecordDescription("Bank Account").HasDescription("Bank Account");
            BankAccounts.PriorityLevel = 10;

            BankAccounts.GetFieldDefinition(p => p.AccountType).IsEnum<BankAccountTypes>();
            BankAccounts.GetFieldDefinition(p => p.CurrentBalance)
                .HasDecimalFieldType(DecimalFieldTypes.Currency)
                .HasDescription("Current Balance");

            BankAccounts.GetFieldDefinition(p => p.ProjectedLowestBalanceAmount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BankAccounts.GetFieldDefinition(p => p.LastCompletedDate).HasDescription("Last Transaction Date");

            BankAccountRegisterItems.GetFieldDefinition(p => p.BudgetItemId).CanSetNull(false);

            BankAccounts.GetFieldDefinition(p => p.Notes).IsMemo();

            BankAccountRegisterItemAmountDetails.GetFieldDefinition(p => p.Amount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BankAccountPeriodHistory.GetFieldDefinition(p => p.TotalDeposits)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BankAccountPeriodHistory.GetFieldDefinition(p => p.TotalWithdrawals)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BudgetItems.GetFieldDefinition(p => p.BankAccountId)
                .HasDescription("Bank Account");

            BudgetItems.PriorityLevel = 20;
            BudgetItems.GetFieldDefinition(p => (int)p.Type).HasContentTemplateId(BudgetItemTypeContentId).EnumTranslation.LoadFromEnum<BudgetItemTypes>();

            BudgetItems.GetFieldDefinition(p => p.Amount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BudgetItems.GetFieldDefinition(p => p.TransferToBankAccountId)
                .HasDescription("Transfer To Bank Account").CanSetNull(false);

            BudgetItems.GetFieldDefinition(p => p.MonthlyAmount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BudgetItems.GetFieldDefinition(p => p.Notes).IsMemo();

            BudgetItems.GetFieldDefinition(p => (int) p.Type).IsEnum<BudgetItemTypes>();

            BudgetItems.GetFieldDefinition(p => (int) p.RecurringType).IsEnum<BudgetItemRecurringTypes>();

            BudgetItemSources.PriorityLevel = 30;

            BudgetPeriodHistory.PriorityLevel = 40;

            BudgetPeriodHistory.GetFieldDefinition(p => p.ProjectedAmount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BudgetPeriodHistory.GetFieldDefinition(p => p.ActualAmount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BudgetPeriodHistory.GetFieldDefinition(p => p.PeriodType).IsEnum<PeriodHistoryTypes>();

            BankAccountPeriodHistory.GetFieldDefinition(p => p.PeriodType).IsEnum<PeriodHistoryTypes>();

            History.PriorityLevel = 100;
            History.GetFieldDefinition(p => p.ItemType).HasContentTemplateId(RegisterTypeCustomContentId);

            History.GetFieldDefinition(p => p.ProjectedAmount).HasDecimalFieldType(DecimalFieldTypes.Currency).DoShowNegativeValuesInRed();

            History.GetFieldDefinition(p => p.ActualAmount).HasDecimalFieldType(DecimalFieldTypes.Currency).DoShowNegativeValuesInRed();

            SourceHistory.PriorityLevel = 200;
            SourceHistory.GetFieldDefinition(p => p.Amount).HasDecimalFieldType(DecimalFieldTypes.Currency);

            BankAccountRegisterItems.GetFieldDefinition(p => p.ItemType).IsEnum<BankAccountRegisterItemTypes>();
        }
    }
}
