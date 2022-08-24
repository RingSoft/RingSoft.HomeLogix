using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
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
        public const int RegisterTypeCustomContentId = 100;

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
        public LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory> BudgetPeriodLookup { get; set; }

        public LookupDefinition<HistoryLookup, History> HistoryLookup { get; set; }

        public LookupDefinition<BankAccountPeriodHistoryLookup, BankAccountPeriodHistory> BankPeriodHistoryLookup { get; set; }
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
            var budgetAlias = HistoryLookup.Include(p => p.BudgetItem).JoinDefinition.Alias;
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

            HistoryLookup.AddVisibleColumnDefinition(p => p.Difference, "Difference",
                formula, 15, "").HasDecimalFieldType(DecimalFieldTypes.Currency).DoShowNegativeValuesInRed()
                .DoShowPositiveValuesInGreen();
            HistoryLookup.InitialOrderByType = OrderByTypes.Descending;
            History.HasLookupDefinition(HistoryLookup);

            var budgetPeriodLookup =
                new LookupDefinition<BudgetPeriodHistoryLookup, BudgetPeriodHistory>(BudgetPeriodHistory);
            budgetPeriodLookup.AddVisibleColumnDefinition(p => p.PeriodEndingDate, "Period Ending Date",
                p => p.PeriodEndingDate, 25);
            budgetPeriodLookup.AddVisibleColumnDefinition(p => p.ProjectedAmount, "Budget Amount", 
                p => p.ProjectedAmount, 25);
            budgetPeriodLookup.AddVisibleColumnDefinition(p => p.ActualAmount, "Actual Amount",
                p => p.ActualAmount, 25);
             budgetAlias = budgetPeriodLookup.Include(p => p.BudgetItem).JoinDefinition.Alias;

            table = DataProcessor.SqlGenerator.FormatSqlObject(BudgetPeriodHistory.TableName);
            var projectedAmountField = DataProcessor.SqlGenerator.FormatSqlObject(
                BudgetPeriodHistory.GetFieldDefinition(p => p.ProjectedAmount).FieldName);
            var actualAmountField = DataProcessor.SqlGenerator.FormatSqlObject(
                BudgetPeriodHistory.GetFieldDefinition(p => p.ActualAmount).FieldName);

            formula = $"CASE {budgetAlias}.{typeField} WHEN {(int)BudgetItemTypes.Income}"
                      + $" THEN {table}.{actualAmountField} - {table}.{projectedAmountField}"
                      + $" ELSE {table}.{projectedAmountField} - {table}.{actualAmountField} END";
            budgetPeriodLookup.AddVisibleColumnDefinition(p => p.Difference, "Difference"
                    , formula, 25, "")
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
        }

        protected override void SetupModel()
        {
            BankAccounts.HasRecordDescription("Bank Account").HasDescription("Bank Account");

            BankAccounts.GetFieldDefinition(p => p.CurrentBalance)
                .HasDecimalFieldType(DecimalFieldTypes.Currency)
                .HasDescription("Current Balance");

            BankAccounts.GetFieldDefinition(p => p.ProjectedLowestBalanceAmount)
                .HasDecimalFieldType(DecimalFieldTypes.Currency);

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

            History.GetFieldDefinition(p => p.ItemType).HasContentTemplateId(RegisterTypeCustomContentId);

            History.GetFieldDefinition(p => p.ProjectedAmount).HasDecimalFieldType(DecimalFieldTypes.Currency).DoShowNegativeValuesInRed();

            History.GetFieldDefinition(p => p.ActualAmount).HasDecimalFieldType(DecimalFieldTypes.Currency).DoShowNegativeValuesInRed();

            SourceHistory.GetFieldDefinition(p => p.Amount).HasDecimalFieldType(DecimalFieldTypes.Currency);
        }

    }
}
