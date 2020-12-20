using System.Linq;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library
{
    public interface IDataRepository
    {
        [CanBeNull] SystemMaster GetSystemMaster();

        BankAccount GetBankAccount(int bankAccountId);

        bool SaveBankAccount(BankAccount bankAccount);

        bool DeleteBankAccount(int bankAccountId);

        BudgetItem GetBudgetItem(int budgetItemId);

        bool SaveBudgetItem(BudgetItem budgetItem);

        bool DeleteBudgetItem(int budgetItemId);
    }

    public class DataRepository : IDataRepository
    {
        [CanBeNull]
        public SystemMaster GetSystemMaster()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.SystemMaster.FirstOrDefault();
        }

        public BankAccount GetBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
        }

        public bool SaveBankAccount(BankAccount bankAccount)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.DbContext.SaveEntity(context.BankAccounts, bankAccount, "Saving Bank Account");
        }

        public bool DeleteBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            var bankAccount = context.BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
            return context.DbContext.DeleteEntity(context.BankAccounts, bankAccount, "Deleting Bank Account");
        }

        public BudgetItem GetBudgetItem(int budgetItemId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);
        }

        public bool SaveBudgetItem(BudgetItem budgetItem)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.DbContext.SaveEntity(context.BudgetItems, budgetItem, "Saving Budget Item");
        }

        public bool DeleteBudgetItem(int budgetItemId)
        {
            var context = AppGlobals.GetNewDbContext();
            var budgetItem = context.BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);
            return context.DbContext.DeleteEntity(context.BudgetItems, budgetItem, "Deleting Budget Item");
        }
    }
}
