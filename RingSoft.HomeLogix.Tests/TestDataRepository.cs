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
            var bankAccount = BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
            if (bankAccount != null)
            {
                var result = new BankAccount
                {
                    Id = bankAccount.Id,
                    Description = bankAccount.Description,
                    CurrentBalance = bankAccount.CurrentBalance,
                    EscrowBalance = bankAccount.EscrowBalance,
                    ProjectedEndingBalance = bankAccount.ProjectedEndingBalance,
                    ProjectedLowestBalanceDate = bankAccount.ProjectedLowestBalanceDate,
                    ProjectedLowestBalanceAmount = bankAccount.ProjectedLowestBalanceAmount,
                    MonthlyBudgetDeposits = bankAccount.MonthlyBudgetDeposits,
                    MonthlyBudgetWithdrawals = bankAccount.MonthlyBudgetWithdrawals,
                    CurrentMonthDeposits = bankAccount.CurrentMonthDeposits,
                    CurrentMonthWithdrawals = bankAccount.CurrentMonthWithdrawals,
                    PreviousMonthDeposits = bankAccount.PreviousMonthDeposits,
                    PreviousMonthWithdrawals = bankAccount.PreviousMonthWithdrawals,
                    CurrentYearDeposits = bankAccount.CurrentYearDeposits,
                    CurrentYearWithdrawals = bankAccount.CurrentYearWithdrawals,
                    PreviousYearDeposits = bankAccount.PreviousYearDeposits,
                    PreviousYearWithdrawals = bankAccount.PreviousYearWithdrawals,
                    EscrowToBankAccountId = bankAccount.EscrowToBankAccountId,
                    EscrowToBankAccount = null,
                    EscrowDayOfMonth = bankAccount.EscrowDayOfMonth,
                    LastGenerationDate = bankAccount.LastGenerationDate,
                    Notes = bankAccount.Notes
                };

                if (result.EscrowToBankAccountId != null)
                    result.EscrowToBankAccount = GetBankAccount((int) result.EscrowToBankAccountId, false);

                return result;
            }

            return null;
        }

        public bool SaveBankAccount(BankAccount bankAccount)
        {
            if (bankAccount.Id == 0)
                bankAccount.Id = BankAccounts.Count + 1;

            var existingBankAccount = BankAccounts.FirstOrDefault(f => f.Id == bankAccount.Id);
            if (existingBankAccount != null)
                BankAccounts.Remove(existingBankAccount);

            BankAccounts.Add(bankAccount);

            return true;
        }

        public bool DeleteBankAccount(int bankAccountId)
        {
            var bankAccount = BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
            if (bankAccount != null)
                BankAccounts.Remove(bankAccount);

            return true;
        }

        public BudgetItem GetBudgetItem(int budgetItemId)
        {
            var budgetItem = BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);

            if (budgetItem != null)
            {
                var result = new BudgetItem
                {
                    Id = budgetItem.Id,
                    Type = budgetItem.Type,
                    Description = budgetItem.Description,
                    BankAccountId = budgetItem.BankAccountId,
                    BankAccount = null,
                    Amount = budgetItem.Amount,
                    RecurringPeriod = budgetItem.RecurringPeriod,
                    RecurringType = budgetItem.RecurringType,
                    StartingDate = budgetItem.StartingDate,
                    EndingDate = budgetItem.EndingDate,
                    DoEscrow = budgetItem.DoEscrow,
                    TransferToBankAccountId = budgetItem.TransferToBankAccountId,
                    TransferToBankAccount = null,
                    LastCompletedDate = budgetItem.LastCompletedDate,
                    NextTransactionDate = budgetItem.NextTransactionDate,
                    MonthlyAmount = budgetItem.MonthlyAmount,
                    CurrentMonthAmount = budgetItem.CurrentMonthAmount,
                    PreviousMonthAmount = budgetItem.PreviousMonthAmount,
                    CurrentYearAmount = budgetItem.CurrentYearAmount,
                    PreviousYearAmount = budgetItem.PreviousYearAmount,
                    EscrowBalance = budgetItem.EscrowBalance,
                    Notes = budgetItem.Notes,
                };
                result.BankAccount = GetBankAccount(result.BankAccountId, false);
                if (result.TransferToBankAccountId != null)
                    result.TransferToBankAccount = GetBankAccount((int) result.TransferToBankAccountId, false);

                return result;
            }
            return null;
        }

        public bool SaveBudgetItem(BudgetItem budgetItem, BankAccount dbBankAccount,
            BankAccount dbTransferToBankAccount, BankAccount escrowBankAccount,
            BankAccount dbEscrowBankAccount)
        {
            if (budgetItem.Id == 0)
            {
                budgetItem.Id = BudgetItems.Count + 1;
            }

            var existingBudgetItem = BudgetItems.FirstOrDefault(f => f.Id == budgetItem.Id);
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

            if (escrowBankAccount != null)
                SaveBankAccount(escrowBankAccount);

            if (dbEscrowBankAccount != null)
                SaveBankAccount(dbEscrowBankAccount);

            return true;
        }

        public bool DeleteBudgetItem(int budgetItemId, BankAccount dbBankAccount,
            BankAccount dbTransferToBankAccount, BankAccount dbEscrowBankAccount)
        {
            if (dbBankAccount != null)
                SaveBankAccount(dbBankAccount);

            if (dbTransferToBankAccount != null)
                SaveBankAccount(dbTransferToBankAccount);

            if (dbEscrowBankAccount != null)
                SaveBankAccount(dbEscrowBankAccount);

            var budgetItem = BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);
            if (budgetItem != null)
                BudgetItems.Remove(budgetItem);

            return true;
        }
    }
}
