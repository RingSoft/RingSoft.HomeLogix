using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class HomeLogixLookupContext : LookupContext
    {
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

        public LookupDefinition<BudgetItemLookup, BudgetItem> BudgetItemsLookup { get; set; }
        public LookupDefinition<BankAccountLookup, BankAccount> BankAccountsLookup { get; set; }
        public LookupDefinition<SourceLookup, BudgetItemSource> BudgetItemSourceLookup { get; set; }

        public LookupDefinition<HistoryLookup, History> HistoryLookup { get; set; }
        //----------------------------------------------------------------------

        public override DbDataProcessor DataProcessor => SqliteDataProcessor;

        public SqliteDataProcessor SqliteDataProcessor { get; }
        
        protected override DbContext DbContext => _dbContext;

        private DbContext _dbContext;

        public HomeLogixLookupContext()
        {
            SqliteDataProcessor = new SqliteDataProcessor();
        }

        public void Initialize(IHomeLogixDbContext dbContext)
        {
            _dbContext = dbContext.DbContext;
            Initialize();
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

            HistoryLookup = new LookupDefinition<HistoryLookup, History>(History);
            HistoryLookup.AddVisibleColumnDefinition(p => p.Date, "Date",
                p => p.Date, 15);
            HistoryLookup.AddVisibleColumnDefinition(p => p.Description, "Description",
                p => p.Description, 40);
            HistoryLookup.AddVisibleColumnDefinition(p => p.ProjectedAmount, "Projected\r\nAmount",
                p => p.ProjectedAmount, 15);
            HistoryLookup.AddVisibleColumnDefinition(p => p.ActualAmount, "Actual\r\nAmount", 
                p => p.ActualAmount, 15);

            var table = DataProcessor.SqlGenerator.FormatSqlObject(History.TableName);
            var projectedField = DataProcessor.SqlGenerator.FormatSqlObject(History
                .GetFieldDefinition(p => p.ProjectedAmount).FieldName);
            var actualField = DataProcessor.SqlGenerator.FormatSqlObject(History
                .GetFieldDefinition(p => p.ActualAmount).FieldName);

            var formula = $"{table}.{projectedField} - {table}.{actualField}";

            HistoryLookup.AddVisibleColumnDefinition(p => p.Difference, "Difference",
                formula, 15).HasDecimalFieldType(DecimalFieldTypes.Currency);
            HistoryLookup.InitialOrderByType = OrderByTypes.Descending;
        }

        protected override void SetupModel()
        {
            BankAccounts.HasRecordDescription("Bank Account").HasDescription("Bank Account");

            BankAccounts.GetFieldDefinition(p => p.CurrentBalance)
                .HasDecimalFieldType(DecimalFieldTypes.Currency)
                .HasDescription("Current Balance");

            BankAccountPeriodHistory.GetFieldDefinition(p => p.TotalDeposits)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BankAccountPeriodHistory.GetFieldDefinition(p => p.TotalWithdrawals)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BudgetItems.GetFieldDefinition(p => p.BankAccountId)
                .HasDescription("Bank Account");

            BudgetItems.GetFieldDefinition(p => p.Amount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BudgetItems.GetFieldDefinition(p => p.TransferToBankAccountId)
                .HasDescription("Transfer To Bank Account");

            BudgetItems.GetFieldDefinition(p => p.MonthlyAmount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BudgetPeriodHistory.GetFieldDefinition(p => p.ProjectedAmount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            BudgetPeriodHistory.GetFieldDefinition(p => p.ActualAmount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

            History.GetFieldDefinition(p => p.ProjectedAmount).HasDecimalFieldType(DecimalFieldTypes.Currency);

            History.GetFieldDefinition(p => p.ActualAmount).HasDecimalFieldType(DecimalFieldTypes.Currency);

            SourceHistory.GetFieldDefinition(p => p.Amount).HasDecimalFieldType(DecimalFieldTypes.Currency);
        }

    }
}
