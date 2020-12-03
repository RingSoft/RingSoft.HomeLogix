using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
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
            BudgetItemsLookup.AddVisibleColumnDefinition(p => p.Description, "Budget Item",
                p => p.Description, 50);

        }

        protected override void SetupModel()
        {
            
        }

    }
}
