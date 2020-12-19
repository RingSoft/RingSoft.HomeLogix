using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.HomeLogix.DataAccess.LookupModel;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class HomeLogixLookupContext : LookupContext
    {
        public TableDefinition<SystemMaster> SystemMaster { get; set; }
        public TableDefinition<BudgetItem> BudgetItems { get; set; }
        public TableDefinition<BankAccount> BankAccounts { get; set; }

        public LookupDefinition<BudgetItemLookup, BudgetItem> BudgetItemsLookup { get; set; }
        public LookupDefinition<BankAccountLookup, BankAccount> BankAccountsLookup { get; set; }
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
            BudgetItemsLookup.AddVisibleColumnDefinition(p => p.Description, "Budget\r\nItem",
                p => p.Description, 25);
            BudgetItemsLookup.AddVisibleColumnDefinition(p => p.ItemType, "Item\r\nType",
                p => p.Type, 20);
            BudgetItemsLookup.AddVisibleColumnDefinition(p => p.RecurringPeriod, "Recurs\r\nEvery",
                p => p.RecurringPeriod, 10);
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
        }

        protected override void SetupModel()
        {
            BankAccounts.GetFieldDefinition(p => p.CurrentBalance)
                .HasDecimalFieldType(DecimalFieldTypes.Currency)
                .HasDescription("Current Balance");
        }

    }
}
