using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;
using System.Linq;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library
{
    public interface IDataRepository
    {
        [CanBeNull] SystemMaster GetSystemMaster();

        BankAccount GetBankAccount(int bankAccountId, bool getRelatedEntities = true);

        bool SaveBankAccount(BankAccount bankAccount, List<BankAccountRegisterItemAmountDetail> amountDetails,
            CompletedRegisterData completedRegisterData = null);

        bool DeleteBankAccount(int bankAccountId);

        List<BudgetItem> GetBudgetItemsForBankAccount(int bankAccountId);

        BudgetItem GetBudgetItem(int budgetItemId);

        bool SaveBudgetItem(BudgetItem budgetItem, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount,
            BankAccount escrowBankAccount, BankAccount dbEscrowBankAccount,
            IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
            List<BankAccountRegisterItem> registerItemsToDelete);

        bool DeleteBudgetItem(int budgetItemId, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount,
            BankAccount dbEscrowBankAccount);

        IEnumerable<BudgetItem> GetEscrowBudgetItems(int bankAccountId);

        bool SaveGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
            List<BankAccountRegisterItemEscrow> newRegisterEscrowItems,
            IEnumerable<BudgetItem> budgetItems,
            List<BankAccountRegisterItem> registerItemsToDelete = null,
            BankAccount bankAccount = null);

        BudgetItemSource GetBudgetItemSource(int storeId);

        bool SaveBudgetItemSource(BudgetItemSource store);

        bool DeleteBudgetItemSource(int storeId);

        BudgetPeriodHistory GetBudgetPeriodHistory(int budgetId, PeriodHistoryTypes type, DateTime periodEndDate);

        BankAccountPeriodHistory GetBankPeriodHistory(int bankAccountId, PeriodHistoryTypes type,
            DateTime periodEndDate);

        BankAccountRegisterItem GetTransferRegisterItem(string transferGuid);

        IEnumerable<BankAccountRegisterItemEscrow> GetEscrowsOfRegisterItem(int bankRegisterId);
    }

    public class DataRepository : IDataRepository
    {
        [CanBeNull]
        public SystemMaster GetSystemMaster()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.SystemMaster.FirstOrDefault();
        }

        public BankAccount GetBankAccount(int bankAccountId, bool getRelatedEntities = true)
        {
            var context = AppGlobals.GetNewDbContext();

            if (getRelatedEntities)
            {
                return context.BankAccounts.Include(i => i.RegisterItems)
                    .ThenInclude(ti => ti.AmountDetails)
                    .ThenInclude(ti => ti.Source)
                    .Include(i => i.RegisterItems)
                    .ThenInclude(ti => ti.BudgetItem)
                       .ThenInclude(ti => ti.TransferToBankAccount)
                       .Include(i => i.EscrowToBankAccount)
                    .FirstOrDefault(f => f.Id == bankAccountId);
            }

            return context.BankAccounts.Include(i => i.EscrowToBankAccount)
                .FirstOrDefault(f => f.Id == bankAccountId);
        }

        public bool SaveBankAccount(BankAccount bankAccount, List<BankAccountRegisterItemAmountDetail> amountDetails,
            CompletedRegisterData completedRegisterData)
        {
            var context = AppGlobals.GetNewDbContext();
            if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, bankAccount,
                $"Saving Bank Account '{bankAccount.Description}'"))
                return false;

            foreach (var budgetItem in completedRegisterData.BudgetItems)
            {
                budgetItem.BankAccount = null;
                budgetItem.TransferToBankAccount = null;
                if (!context.DbContext.SaveNoCommitEntity(context.BudgetItems, budgetItem,
                    $"Saving Budget Item '{budgetItem.Description}'"))
                    return false;
            }

            foreach (var bankAccountPeriodHistoryRecord in completedRegisterData.BankAccountPeriodHistoryRecords)
            {
                if (context.BankAccountPeriodHistory.Any(a =>
                    a.BankAccountId == bankAccountPeriodHistoryRecord.BankAccountId &&
                    a.PeriodEndingDate == bankAccountPeriodHistoryRecord.PeriodEndingDate))
                {
                    if (!context.DbContext.SaveNoCommitEntity(context.BankAccountPeriodHistory,
                        bankAccountPeriodHistoryRecord,
                        $"Saving Bank Account Period Ending '{bankAccountPeriodHistoryRecord.PeriodEndingDate.ToString(CultureInfo.InvariantCulture)}'")
                    )
                        return false;
                }
                else
                {
                    if (!context.DbContext.AddNewNoCommitEntity(context.BankAccountPeriodHistory,
                        bankAccountPeriodHistoryRecord,
                        $"Saving Bank Account Period Ending '{bankAccountPeriodHistoryRecord.PeriodEndingDate.ToString(CultureInfo.InvariantCulture)}'")
                    )
                        return false;
                }
            }

            foreach (var budgetPeriodHistoryRecord in completedRegisterData.BudgetPeriodHistoryRecords)
            {
                if (context.BudgetPeriodHistory.Any(a =>
                    a.BudgetItemId == budgetPeriodHistoryRecord.BudgetItemId &&
                    a.PeriodEndingDate == budgetPeriodHistoryRecord.PeriodEndingDate))
                {
                    if (!context.DbContext.SaveNoCommitEntity(context.BudgetPeriodHistory, budgetPeriodHistoryRecord,
                        $"Saving Budget Period Ending '{budgetPeriodHistoryRecord.PeriodEndingDate.ToString(CultureInfo.InvariantCulture)}'")
                    )
                        return false;
                }
                else
                {
                    if (!context.DbContext.AddNewNoCommitEntity(context.BudgetPeriodHistory, budgetPeriodHistoryRecord,
                        $"Saving Budget Period Ending '{budgetPeriodHistoryRecord.PeriodEndingDate.ToString(CultureInfo.InvariantCulture)}'")
                    )
                        return false;
                }
            }

            context.BankAccountRegisterItems.RemoveRange(completedRegisterData.CompletedRegisterItems);

            foreach (var registerItem in bankAccount.RegisterItems)
            {
                context.BankAccountRegisterItemAmountDetails.RemoveRange(
                    context.BankAccountRegisterItemAmountDetails.Where(w => w.RegisterId == registerItem.Id));
            }
            context.BankAccountRegisterItemAmountDetails.AddRange(amountDetails);

            context.History.AddRange(completedRegisterData.NewHistoryRecords);

            if (!context.DbContext.SaveEfChanges($"Saving Bank Account '{bankAccount.Description}'"))
                return false;

            foreach (var newSourceHistoryRecord in completedRegisterData.NewSourceHistoryRecords)
            {
                newSourceHistoryRecord.HistoryId = newSourceHistoryRecord.HistoryItem.Id;
                newSourceHistoryRecord.HistoryItem = null;
            }

            context.SourceHistory.AddRange(completedRegisterData.NewSourceHistoryRecords);

            return context.DbContext.SaveEfChanges($"Saving Bank Account '{bankAccount.Description}' Source History");
        }

        public bool DeleteBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            var bankAccount = context.BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
            return bankAccount != null && context.DbContext.DeleteEntity(context.BankAccounts, bankAccount,
                $"Deleting Bank Account '{bankAccount.Description}'");
        }

        public List<BudgetItem> GetBudgetItemsForBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItems.Where(w => w.BankAccountId == bankAccountId
                                                  || w.TransferToBankAccountId == bankAccountId).ToList();
        }

        public BudgetItem GetBudgetItem(int budgetItemId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItems
                .Include(i => i.BankAccount)
                .Include(i => i.TransferToBankAccount)
                .FirstOrDefault(f => f.Id == budgetItemId);
        }

        public bool SaveBudgetItem(BudgetItem budgetItem, BankAccount dbBankAccount,
            BankAccount dbTransferToBankAccount, BankAccount escrowBankAccount,
            BankAccount dbEscrowBankAccount, IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
            List<BankAccountRegisterItem> registerItemsToDelete)
        {
            var context = AppGlobals.GetNewDbContext();
            if (!context.DbContext.SaveNoCommitEntity(context.BudgetItems, budgetItem, "Saving Budget Item"))
                return false;

            if (dbBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbBankAccount, "Saving Bank Account"))
                    return false;
            }

            if (dbTransferToBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbTransferToBankAccount, "Saving Transfer To Bank Account"))
                    return false;
            }

            if (escrowBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, escrowBankAccount,
                    "Saving Escrow Bank Account"))
                    return false;
            }

            if (dbEscrowBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbEscrowBankAccount,
                    "Database Saving Escrow Bank Account"))
                    return false;
            }

            context.BankAccountRegisterItems.AddRange(newBankRegisterItems);

            context.BankAccountRegisterItems.RemoveRange(registerItemsToDelete);

            return context.DbContext.SaveEfChanges("Saving Budget Item");
        }

        public bool DeleteBudgetItem(int budgetItemId, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount,
            BankAccount dbEscrowBankAccount)
        {
            var context = AppGlobals.GetNewDbContext();
            if (dbBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbBankAccount, "Saving Bank Account"))
                    return false;
            }

            if (dbTransferToBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbTransferToBankAccount, "Saving Transfer To Bank Account"))
                    return false;
            }

            if (dbEscrowBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbEscrowBankAccount,
                    "Database Saving Escrow Bank Account"))
                    return false;
            }

            var budgetItem = context.BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);
            return budgetItem != null && context.DbContext.DeleteEntity(context.BudgetItems, budgetItem, $"Deleting Budget Item '{budgetItem.Description}'");
        }

        public IEnumerable<BudgetItem> GetEscrowBudgetItems(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItems.Where(w => w.BankAccountId == bankAccountId && w.DoEscrow);
        }

        public bool SaveGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
            List<BankAccountRegisterItemEscrow> newRegisterEscrowItems,
            IEnumerable<BudgetItem> budgetItems,
            List<BankAccountRegisterItem> registerItemsToDelete = null,
            BankAccount bankAccount = null)
        {
            var context = AppGlobals.GetNewDbContext();

            if (registerItemsToDelete != null && registerItemsToDelete.Any())
                context.DbContext.RemoveRange(registerItemsToDelete);

            if (bankAccount != null)
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, bankAccount,
                    $"Saving Bank Account '{bankAccount.Description}'"))
                    return false;

            foreach (var budgetItem in budgetItems)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BudgetItems, budgetItem,
                    $"Saving Budget Item '{budgetItem.Description}'"))
                    return false;
            }

            foreach (var bankAccountRegisterItem in newBankRegisterItems)
            {
                context.BankAccountRegisterItems.Add(bankAccountRegisterItem);
            }

            var result = context.DbContext.SaveEfChanges("Saving generated Bank Account Register Items");
            if (!result)
                return false;

            if (newRegisterEscrowItems.Any())
            {
                foreach (var registerEscrowItem in newRegisterEscrowItems)
                {
                    registerEscrowItem.RegisterId = registerEscrowItem.RegisterItem.Id;
                    registerEscrowItem.RegisterItem = null;
                }
                context.BankAccountRegisterItemEscrows.AddRange(newRegisterEscrowItems);
                result = context.DbContext.SaveEfChanges("Saving generated Bank Account Register Item Escrows.");
            }

            return result;
        }

        public BudgetItemSource GetBudgetItemSource(int sourceId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItemSources.FirstOrDefault(f => f.Id == sourceId);
        }

        public bool SaveBudgetItemSource(BudgetItemSource source)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.DbContext.SaveEntity(context.BudgetItemSources, source, $"Saving Source '{source.Name}'");
        }

        public bool DeleteBudgetItemSource(int sourceId)
        {
            var context = AppGlobals.GetNewDbContext();
            var source = context.BudgetItemSources.FirstOrDefault(f => f.Id == sourceId);
            return source != null && context.DbContext.DeleteEntity(context.BudgetItemSources, source, $"Deleting Source '{source.Name}'");
        }

        public BudgetPeriodHistory GetBudgetPeriodHistory(int budgetId, PeriodHistoryTypes type, DateTime periodEndDate)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetPeriodHistory.FirstOrDefault(f =>
                f.BudgetItemId == budgetId && f.PeriodType == (byte) type && f.PeriodEndingDate == periodEndDate);
        }

        public BankAccountPeriodHistory GetBankPeriodHistory(int bankAccountId, PeriodHistoryTypes type, DateTime periodEndDate)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BankAccountPeriodHistory.FirstOrDefault(f =>
                f.BankAccountId == bankAccountId && f.PeriodType == (byte)type && f.PeriodEndingDate == periodEndDate);
        }

        public BankAccountRegisterItem GetTransferRegisterItem(string transferGuid)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BankAccountRegisterItems.FirstOrDefault(f => f.RegisterGuid == transferGuid);
        }

        public IEnumerable<BankAccountRegisterItemEscrow> GetEscrowsOfRegisterItem(int bankRegisterId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BankAccountRegisterItemEscrows.Include(i => i.BudgetItem)
                .Where(w => w.RegisterId == bankRegisterId);
        }
    }
}
