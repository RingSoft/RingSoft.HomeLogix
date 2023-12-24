using RingSoft.DbLookup.Testing;
using RingSoft.HomeLogix.DataAccess.Model;
using IDataRepository = RingSoft.HomeLogix.Library.IDataRepository;

namespace RingSoft.HomeLogix.Tests
{
    public class TestDataRegistry : DataRepositoryRegistry , DataAccess.IDbContext
    {
    }


    public class HomeLogixTestDataRepository : TestDataRepository
    {
        public new TestDataRegistry DataContext { get; }

        public HomeLogixTestDataRepository(TestDataRegistry context) : base(context)
        {
            DataContext = context;

            DataContext.AddEntity(new DataRepositoryRegistryItem<BudgetItem>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<BankAccount>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<BankAccountRegisterItem>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<History>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<BudgetPeriodHistory>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<BankAccountPeriodHistory>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<BudgetItemSource>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<SourceHistory>());
        }

        public DataAccess.IDbContext GetDataContext()
        {
            return DataContext;
        }
    }
}
    