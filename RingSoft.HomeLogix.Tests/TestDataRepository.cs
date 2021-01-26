using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.HomeLogix.Tests
{
    public class TestDataRepository : IDataRepository
    {
        public List<BankAccount> BankAccounts { get; }

        public List<BudgetItem> BudgetItems { get; }

        public TestDataRepository()
        {
            BankAccounts = new List<BankAccount>();
            BudgetItems = new List<BudgetItem>();
        }

        public SystemMaster GetSystemMaster()
        {
            throw new System.NotImplementedException();
        }

        public BankAccount GetBankAccount(int bankAccountId, bool getRelatedEntities = true)
        {
            var result = BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
            return result;
        }

        public bool SaveBankAccount(BankAccount bankAccount)
        {
            if (bankAccount.Id == 0)
                bankAccount.Id = BankAccounts.Count + 1;

            var existingBankAccount = GetBankAccount(bankAccount.Id);
            if (existingBankAccount != null)
                BankAccounts.Remove(existingBankAccount);

            BankAccounts.Add(bankAccount);

            return true;
        }

        public bool DeleteBankAccount(int bankAccountId)
        {
            var bankAccount = GetBankAccount(bankAccountId);
            if (bankAccount != null)
                BankAccounts.Remove(bankAccount);

            return true;
        }

        public BudgetItem GetBudgetItem(int budgetItemId)
        {
            var result = BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);

            if (result != null)
            {
                result.BankAccount = GetBankAccount(result.BankAccountId, false);
                if (result.TransferToBankAccountId != null)
                    result.TransferToBankAccount = GetBankAccount((int) result.TransferToBankAccountId, false);
            }
            return result;
        }

        public bool SaveBudgetItem(BudgetItem budgetItem, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount)
        {
            if (budgetItem.Id == 0)
            {
                budgetItem.Id = BudgetItems.Count + 1;
            }

            var existingBudgetItem = GetBudgetItem(budgetItem.Id);
            if (existingBudgetItem != null) 
                BudgetItems.Remove(existingBudgetItem);

            BudgetItems.Add(budgetItem);

            if (budgetItem.BankAccount != null)
                SaveBankAccount(budgetItem.BankAccount);

            if (budgetItem.TransferToBankAccount != null)
                SaveBankAccount(budgetItem.TransferToBankAccount);

            if (dbBankAccount != null)
                SaveBankAccount(dbBankAccount);

            if (dbTransferToBankAccount != null)
                SaveBankAccount(dbTransferToBankAccount);

            return true;
        }

        public bool DeleteBudgetItem(int budgetItemId)
        {
            var budgetItem = GetBudgetItem(budgetItemId);
            if (budgetItem != null)
                BudgetItems.Remove(budgetItem);

            return true;
        }
    }
}
