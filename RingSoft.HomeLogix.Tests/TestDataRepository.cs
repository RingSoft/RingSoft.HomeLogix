using System;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using System.Collections.Generic;
using System.Linq;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

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
                    ProjectedEndingBalance = bankAccount.ProjectedEndingBalance,
                    ProjectedLowestBalanceDate = bankAccount.ProjectedLowestBalanceDate,
                    ProjectedLowestBalanceAmount = bankAccount.ProjectedLowestBalanceAmount,
                    MonthlyBudgetDeposits = bankAccount.MonthlyBudgetDeposits,
                    MonthlyBudgetWithdrawals = bankAccount.MonthlyBudgetWithdrawals,
                    LastGenerationDate = bankAccount.LastGenerationDate,
                    Notes = bankAccount.Notes
                };
                return result;
            }

            return null;
        }

        public IEnumerable<BankAccountRegisterItem> GetRegisterItemsForBankAccount(int bankAccountId)
        {
            return new List<BankAccountRegisterItem>();
        }

        public IEnumerable<BankAccountRegisterItemAmountDetail> GetBankAccountRegisterItemDetails(int registerId)
        {
            throw new NotImplementedException();
        }

        public bool SaveBankAccount(BankAccount bankAccount,
            CompletedRegisterData clearedRegisterData = null)
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

        public bool SaveRegisterItem(BankAccountRegisterItem registerItem)
        {
            throw new NotImplementedException();
        }

        public bool SaveRegisterItems(List<BankAccountRegisterItem> registerItems)
        {
            throw new NotImplementedException();
        }

        public bool SaveRegisterItem(BankAccountRegisterItem registerItem, List<BankAccountRegisterItemAmountDetail> amountDetails)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRegisterItems(List<BankAccountRegisterItem> registerItems)
        {
            throw new NotImplementedException();
        }

        public List<BudgetItem> GetBudgetItemsForBankAccount(int bankAccountId)
        {
            throw new System.NotImplementedException();
        }

        public BudgetItem GetBudgetItem(int? budgetItemId)
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
                    TransferToBankAccountId = budgetItem.TransferToBankAccountId,
                    TransferToBankAccount = null,
                    MonthlyAmount = budgetItem.MonthlyAmount,
                    CurrentMonthAmount = budgetItem.CurrentMonthAmount,
                    CurrentMonthEnding = budgetItem.CurrentMonthEnding,
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
            BankAccount dbTransferToBankAccount, List<BankAccountRegisterItem> newBankRegisterItems,
            List<BankAccountRegisterItem> registerItemsToDelete)
        {
            if (budgetItem.Id == 0)
            {
                budgetItem.Id = BudgetItems.Count + 1;
            }

            var existingBudgetItem = BudgetItems.FirstOrDefault(f => f.Id == budgetItem.Id);
            if (existingBudgetItem != null) 
                BudgetItems.Remove(existingBudgetItem);

            var amountDetails = new List<BankAccountRegisterItemAmountDetail>();
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

        public bool SaveBudgetItem(BudgetItem budgetItem, List<BudgetPeriodHistory> budgetPeriodHistoryRecords, History newHistoryRecord)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBudgetItem(int budgetItemId, BankAccount dbBankAccount,
            BankAccount dbTransferToBankAccount)
        {
            var amountDetails = new List<BankAccountRegisterItemAmountDetail>();
            if (dbBankAccount != null)
                SaveBankAccount(dbBankAccount);

            if (dbTransferToBankAccount != null)
                SaveBankAccount(dbTransferToBankAccount);

            var budgetItem = BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);
            if (budgetItem != null)
                BudgetItems.Remove(budgetItem);

            return true;
        }

        public bool SaveGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
            IEnumerable<BudgetItem> budgetItems,
            List<BankAccountRegisterItem> registerItemsToDelete = null, BankAccount bankAccount = null)
        {
            throw new System.NotImplementedException();
        }

        public BudgetItemSource GetBudgetItemSource(int sourceId)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveBudgetItemSource(BudgetItemSource source)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteBudgetItemSource(int sourceId)
        {
            throw new System.NotImplementedException();
        }

        public BudgetPeriodHistory GetBudgetPeriodHistory(int budgetId, PeriodHistoryTypes type, DateTime periodEndDate)
        {
            throw new NotImplementedException();
        }

        public BankAccountPeriodHistory GetBankPeriodHistory(int bankAccountId, PeriodHistoryTypes type, DateTime periodEndDate)
        {
            throw new NotImplementedException();
        }

        public BankAccountRegisterItem GetTransferRegisterItem(string transferGuid)
        {
            throw new NotImplementedException();
        }

        public bool SaveNewRegisterItem(BankAccountRegisterItem registerItem, BankAccountRegisterItem transferRegisterItem = null)
        {
            throw new NotImplementedException();
        }

        public BudgetPeriodHistory GetMaxMonthBudgetPeriodHistory()
        {
            throw new NotImplementedException();
        }

        public BudgetTotals GetBudgetTotals(DateTime monthEndDate, DateTime previousMonthEnding,
            DateTime nextMonthEnding)
        {
            throw new NotImplementedException();
        }

        public History GetHistoryItem(int historyId)
        {
            throw new NotImplementedException();
        }

        public SourceHistory GetSourceHistory(int historyId, int detailId)
        {
            throw new NotImplementedException();
        }

        public List<BankTransaction> GetBankTransactions(int bankAccountId)
        {
            throw new NotImplementedException();
        }

        public bool SaveBankTransactions(List<BankTransaction> transactions, List<BankTransactionBudget> splits,
            int bankAccountId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTransactions(int bankAccountId)
        {
            throw new NotImplementedException();
        }

        public BudgetPeriodHistory GetBudgetPeriodHistory(int budgetId, byte periodType, DateTime periodEndDate)
        {
            throw new NotImplementedException();
        }
    }
}
