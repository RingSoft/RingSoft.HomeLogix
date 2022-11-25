using System;
using System.Collections;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Tests
{
    public class DataRepositoryRegistryItemBase
    {
        public Type Entity { get; internal set; }
    }
    public class DataRepositoryRegistryItem<TEntity> : DataRepositoryRegistryItemBase where TEntity : class
    {
        public List<TEntity> Table { get; private set; }

        public DataRepositoryRegistryItem(TEntity entity)
        {
            Table = new List<TEntity>();
            Entity = typeof(TEntity);
        }
    }
    public class DataRepositoryRegistry
    {
        public List<DataRepositoryRegistryItemBase> Entities { get; private set; } = new List<DataRepositoryRegistryItemBase>();

        public void AddEntity(DataRepositoryRegistryItemBase entity)
        {
            Entities.Add(entity);
        }

        public List<TEntity> GetList<TEntity>() where TEntity : class
        {
            var result = new List<TEntity>();
            var entity = Entities.FirstOrDefault(p => p.Entity == typeof(TEntity));
            if (entity != null)
            {
                var existingEntity = entity as DataRepositoryRegistryItem<TEntity>;
                return existingEntity.Table;
            }
            return result;
        }

        public IQueryable<TEntity> GetEntity<TEntity>() where TEntity : class
        {
            var entity = Entities.FirstOrDefault(p => p.Entity == typeof(TEntity));
            if (entity != null)
            {
                var existingEntity = entity as DataRepositoryRegistryItem<TEntity>;
                return existingEntity.Table.AsQueryable();
            }

            return null;
        }
    }
    public class TestDataRepository : IDataRepository
    {
        public DataRepositoryRegistry DbContext { get; private set; }
        public TestDataRepository()
        {
            DbContext = new DataRepositoryRegistry();
            DbContext.AddEntity(new DataRepositoryRegistryItem<BankAccount>(new BankAccount()));
            DbContext.AddEntity(new DataRepositoryRegistryItem<BudgetItem>(new BudgetItem()));
        }

        public IDbContext GetDataContext()
        {
            throw new NotImplementedException();
        }

        public SystemMaster GetSystemMaster()
        {
            throw new System.NotImplementedException();
        }

        public bool SaveSystemMaster(SystemMaster systemMaster)
        {
            throw new NotImplementedException();
        }

        public bool SaveBudgetPeriodRecord(IDbContext context, BudgetPeriodHistory budgetPeriodHistoryRecord)
        {
            throw new NotImplementedException();
        }

        public BankAccount GetBankAccount(int bankAccountId, bool getRelatedEntities = true)
        {
            var bankAccount = DbContext.GetEntity<BankAccount>().FirstOrDefault(f => f.Id == bankAccountId);
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
            var table = DbContext.GetList<BankAccount>();
            if (bankAccount.Id == 0)
                bankAccount.Id = table.Count + 1;

            var existingBankAccount = table.FirstOrDefault(f => f.Id == bankAccount.Id);
            if (existingBankAccount != null)
                table.Remove(existingBankAccount);

            table.Add(bankAccount);

            return true;
        }

        public bool DeleteBankAccount(int bankAccountId)
        {
            var table = DbContext.GetList<BankAccount>();
            var bankAccount = table.FirstOrDefault(f => f.Id == bankAccountId);
            if (bankAccount != null)
                table.Remove(bankAccount);

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
            var table = DbContext.GetList<BudgetItem>();
            var budgetItem = table.FirstOrDefault(f => f.Id == budgetItemId);

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
            var table = DbContext.GetList<BudgetItem>();
            if (budgetItem.Id == 0)
            {
                budgetItem.Id = table.Count + 1;
            }

            var existingBudgetItem = table.FirstOrDefault(f => f.Id == budgetItem.Id);
            if (existingBudgetItem != null) 
                table.Remove(existingBudgetItem);

            var amountDetails = new List<BankAccountRegisterItemAmountDetail>();
            table.Add(budgetItem);

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

            var table = DbContext.GetList<BudgetItem>();
            var budgetItem = table.FirstOrDefault(f => f.Id == budgetItemId);
            if (budgetItem != null)
                table.Remove(budgetItem);

            return true;
        }

        public bool SaveGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
            IEnumerable<BudgetItem> budgetItems,
            List<BankAccountRegisterItem> registerItemsToDelete = null, BankAccount bankAccount = null)
        {
            throw new System.NotImplementedException();
        }

        public BankAccountRegisterItem GetBankAccountRegisterItem(int registerId)
        {
            throw new NotImplementedException();
        }

        public BudgetItemSource GetBudgetItemSource(int sourceId)
        {
            throw new System.NotImplementedException();
        }

        public decimal GetSourceTotal(int storeId)
        {
            throw new NotImplementedException();
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

        public BudgetTotals GetBankBudgetTotals(DateTime monthEndDate, int bankAccountId)
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

        public QifMap GetQifMap(int qifMapId)
        {
            throw new NotImplementedException();
        }

        public QifMap GetQifMap(string bankText)
        {
            throw new NotImplementedException();
        }

        public bool SaveQifMap(QifMap qifMap)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<History> GetPhoneHistoryList(DateTime currentDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SourceHistory> GetPhoneSourceHistory(DateTime currentDate)
        {
            throw new NotImplementedException();
        }

        public bool HistoryExists(int budgetId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public TEntity GetEntity<TEntity>(IEnumerable<TEntity> items, Func<TEntity, bool> predicate) where TEntity : class
        {
            return items.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class
        {
            var entity = DbContext.GetEntity<TEntity>();
            return entity;
        }

        public BudgetPeriodHistory GetBudgetPeriodHistory(int budgetId, byte periodType, DateTime periodEndDate)
        {
            throw new NotImplementedException();
        }
    }
}
