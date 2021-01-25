using System.Collections.Generic;
using System.Linq;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Tests
{
    public class TestDataRepository : IDataRepository
    {
        public const int JointCheckingBankAccountId = 1;
        public const int JaneCheckingBankAccountId = 2;
        public const int JuniorCheckingBankAccountId = 3;
        public const int SavingsBankAccountId = 4;

        public const int JohnIncomeBudgetItemId = 1;
        public const int JaneIncomeBudgetItemId = 2;
        public const int HousePaymentBudgetItemId = 3;
        public const int GroceriesBudgetItemId = 4;
        public const int TransferBudgetItemId = 5;
        public List<BankAccount> BankAccounts { get; }

        public List<BudgetItem> BudgetItems { get; }

        public TestDataRepository()
        {
            BankAccounts = new List<BankAccount>();
            BudgetItems = new List<BudgetItem>();
        }

        public void Initialize()
        {
            CreateBankAccounts();
        }

        private void CreateBankAccounts()
        {
            var bankAccountViewModel = new BankAccountViewModel();
            bankAccountViewModel.Id = JointCheckingBankAccountId;
            bankAccountViewModel.KeyAutoFillValue = new AutoFillValue(null, "Joint Checking Account");
            bankAccountViewModel.DoSave();
        }

        public SystemMaster GetSystemMaster()
        {
            throw new System.NotImplementedException();
        }

        public BankAccount GetBankAccount(int bankAccountId, bool getRelatedEntities = true)
        {
            return BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
        }

        public bool SaveBankAccount(BankAccount bankAccount)
        {
            var existingBankAccount = GetBankAccount(bankAccount.Id);
            if (existingBankAccount == null)
            {
                if (bankAccount.Id == 0)
                    bankAccount.Id = BankAccounts.Count + 1;

                BankAccounts.Add(bankAccount);
            }
            else
            {
                existingBankAccount.Description = bankAccount.Description;
                existingBankAccount.CurrentBalance = bankAccount.CurrentBalance;
                existingBankAccount.EscrowBalance = bankAccount.EscrowBalance;
                existingBankAccount.ProjectedEndingBalance = bankAccount.ProjectedEndingBalance;
                existingBankAccount.ProjectedLowestBalanceDate = bankAccount.ProjectedLowestBalanceDate;
                existingBankAccount.ProjectedLowestBalanceAmount = bankAccount.ProjectedLowestBalanceAmount;
                existingBankAccount.MonthlyBudgetDeposits = bankAccount.MonthlyBudgetDeposits;
                existingBankAccount.MonthlyBudgetWithdrawals = bankAccount.MonthlyBudgetWithdrawals;
                existingBankAccount.CurrentMonthDeposits = bankAccount.CurrentMonthDeposits;
                existingBankAccount.CurrentMonthWithdrawals = bankAccount.CurrentMonthWithdrawals;
                existingBankAccount.PreviousMonthDeposits = bankAccount.PreviousMonthDeposits;
                existingBankAccount.PreviousMonthWithdrawals = bankAccount.PreviousMonthWithdrawals;
                existingBankAccount.CurrentYearDeposits = bankAccount.CurrentYearDeposits;
                existingBankAccount.CurrentYearWithdrawals = bankAccount.CurrentYearWithdrawals;
                existingBankAccount.PreviousYearDeposits = bankAccount.PreviousYearDeposits;
                existingBankAccount.PreviousYearWithdrawals = bankAccount.PreviousYearWithdrawals;
                existingBankAccount.EscrowToBankAccountId = bankAccount.EscrowToBankAccountId;
                existingBankAccount.EscrowToBankAccount = bankAccount.EscrowToBankAccount;
                existingBankAccount.EscrowDayOfMonth = bankAccount.EscrowDayOfMonth;
                existingBankAccount.Notes = bankAccount.Notes;
            }

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
            return BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);
        }

        public bool SaveBudgetItem(BudgetItem budgetItem, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount)
        {
            var existingBudgetItem = GetBudgetItem(budgetItem.Id);
            if (existingBudgetItem == null)
            {
                if (budgetItem.Id == 0)
                    budgetItem.Id = BudgetItems.Count + 1;

                BudgetItems.Add(budgetItem);
            }
            else
            {
                existingBudgetItem.Type = budgetItem.Type;
                existingBudgetItem.Description = budgetItem.Description;
                existingBudgetItem.BankAccountId = budgetItem.BankAccountId;
                existingBudgetItem.BankAccount = budgetItem.BankAccount;
            }

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
