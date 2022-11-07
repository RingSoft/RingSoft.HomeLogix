﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;
using System.Linq;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library
{
    public enum Relationship
    { LessThan = -1, Equals = 0, GreaterThan = 1 }

    public interface IDataRepository
    {
        [CanBeNull] SystemMaster GetSystemMaster();

        bool SaveSystemMaster(SystemMaster systemMaster);

        BankAccount GetBankAccount(int bankAccountId, bool getRelatedEntities = true);

        IEnumerable<BankAccountRegisterItem> GetRegisterItemsForBankAccount(int bankAccountId);

        IEnumerable<BankAccountRegisterItemAmountDetail> GetBankAccountRegisterItemDetails(int registerId);

        bool SaveBankAccount(BankAccount bankAccount, CompletedRegisterData completedRegisterData = null);

        bool DeleteBankAccount(int bankAccountId);

        bool SaveRegisterItem(BankAccountRegisterItem registerItem);

        bool SaveRegisterItems(List<BankAccountRegisterItem> registerItems);

        bool SaveRegisterItem(BankAccountRegisterItem registerItem,
            List<BankAccountRegisterItemAmountDetail> amountDetails);

        bool DeleteRegisterItems(List<BankAccountRegisterItem> registerItems);

        List<BudgetItem> GetBudgetItemsForBankAccount(int bankAccountId);

        BudgetItem GetBudgetItem(int? budgetItemId);

        bool SaveBudgetItem(BudgetItem budgetItem, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount,
            List<BankAccountRegisterItem> newBankRegisterItems,
            List<BankAccountRegisterItem> registerItemsToDelete);

        bool SaveBudgetItem(BudgetItem budgetItem, List<BudgetPeriodHistory> budgetPeriodHistoryRecords,
            History newHistoryRecord);
        bool DeleteBudgetItem(int budgetItemId, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount);

        bool SaveGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
            IEnumerable<BudgetItem> budgetItems,
            List<BankAccountRegisterItem> registerItemsToDelete = null,
            BankAccount bankAccount = null);

        BankAccountRegisterItem GetBankAccountRegisterItem(int registerId);

        BudgetItemSource GetBudgetItemSource(int storeId);

        decimal GetSourceTotal(int storeId);

        bool SaveBudgetItemSource(BudgetItemSource store);

        bool DeleteBudgetItemSource(int storeId);

        BudgetPeriodHistory GetBudgetPeriodHistory(int budgetId, PeriodHistoryTypes type, DateTime periodEndDate);

        BankAccountPeriodHistory GetBankPeriodHistory(int bankAccountId, PeriodHistoryTypes type,
            DateTime periodEndDate);

        BankAccountRegisterItem GetTransferRegisterItem(string transferGuid);

        bool SaveNewRegisterItem(BankAccountRegisterItem registerItem,
            BankAccountRegisterItem transferRegisterItem = null);

        BudgetPeriodHistory GetMaxMonthBudgetPeriodHistory();

        BudgetTotals GetBudgetTotals(DateTime monthEndDate, DateTime previousMonthEnding, DateTime nextMonthEnding);

        BudgetTotals GetBankBudgetTotals(DateTime monthEndDate, int bankAccountId);

        History GetHistoryItem(int historyId);

        SourceHistory GetSourceHistory(int historyId, int detailId);

        List<BankTransaction> GetBankTransactions(int bankAccountId);

        bool SaveBankTransactions(List<BankTransaction> transactions, List<BankTransactionBudget> splits, int bankAccountId);

        bool DeleteTransactions(int bankAccountId);

        QifMap GetQifMap(int qifMapId);

        QifMap GetQifMap(string bankText);

        bool SaveQifMap(QifMap qifMap);
    }

    public class DataRepository : IDataRepository
    {
        [CanBeNull]
        public SystemMaster GetSystemMaster()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.SystemMaster.FirstOrDefault();
        }

        public bool SaveSystemMaster(SystemMaster systemMaster)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.DbContext.SaveEntity(context.SystemMaster, systemMaster, "Saving System Master");
        }

        public BankAccount GetBankAccount(int bankAccountId, bool getRelatedEntities = true)
        {
            var context = AppGlobals.GetNewDbContext();

            if (getRelatedEntities)
            {
                return context.BankAccounts.Include(i => i.RegisterItems.OrderBy(o => o.ItemDate)
                        .ThenByDescending(t => t.ProjectedAmount))
                    .Include(i => i.RegisterItems)
                    .ThenInclude(ti => ti.AmountDetails)
                    .Include(i => i.RegisterItems)
                    .ThenInclude(ti => ti.BudgetItem)
                       .ThenInclude(ti => ti.TransferToBankAccount)
                    .FirstOrDefault(f => f.Id == bankAccountId);
            }

            return context.BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
        }

        public IEnumerable<BankAccountRegisterItem> GetRegisterItemsForBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BankAccountRegisterItems.OrderBy(o => o.ItemDate)
                .ThenByDescending(t => t.ProjectedAmount)
                .Where(w => w.BankAccountId == bankAccountId).ToList();
        }

        public IEnumerable<BankAccountRegisterItemAmountDetail> GetBankAccountRegisterItemDetails(int registerId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BankAccountRegisterItems.Include(i => i.AmountDetails)
                .ThenInclude(ti => ti.Source)
                .FirstOrDefault(f => f.Id == registerId)
                ?.AmountDetails;
        }

        public bool SaveBankAccount(BankAccount bankAccount,
            CompletedRegisterData completedRegisterData = null)
        {
            var context = AppGlobals.GetNewDbContext();
            if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, bankAccount,
                $"Saving Bank Account '{bankAccount.Description}'"))
                return false;

            //DateTime? checkDate = null;
            if (completedRegisterData != null)
            {
                foreach (var bankAccountPeriodHistoryRecord in completedRegisterData.BankAccountPeriodHistoryRecords)
                {
                    if (context.BankAccountPeriodHistory.Any(a =>
                            a.PeriodType == bankAccountPeriodHistoryRecord.PeriodType &&
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


                foreach (var bankAccountRegisterItem in completedRegisterData.CompletedRegisterItems)
                {
                    //bankAccountRegisterItem.BudgetItemId = 0;
                    bankAccountRegisterItem.BudgetItem = null;
                    //bankAccountRegisterItem.BankAccountId = 0;
                    bankAccountRegisterItem.BankAccount = null;
                }

                if (completedRegisterData != null)
                {
                    foreach (var budgetItem in completedRegisterData.BudgetItems)
                    {
                        budgetItem.BankAccount = null;
                        budgetItem.TransferToBankAccount = null;
                        if (!context.DbContext.SaveNoCommitEntity(context.BudgetItems, budgetItem,
                                $"Saving Budget Item '{budgetItem.Description}'"))
                            return false;
                    }


                    foreach (var budgetPeriodHistoryRecord in completedRegisterData.BudgetPeriodHistoryRecords)
                    {
                        if (!SaveBudgetPeriodRecord(context, budgetPeriodHistoryRecord)) return false;
                    }

                    context.BankAccountRegisterItems.RemoveRange(completedRegisterData.CompletedRegisterItems);

                    context.History.AddRange(completedRegisterData.NewHistoryRecords);

                    foreach (var newSourceHistoryRecord in completedRegisterData.NewSourceHistoryRecords)
                    {
                        newSourceHistoryRecord.HistoryId = newSourceHistoryRecord.HistoryItem.Id;
                        //newSourceHistoryRecord.HistoryItem.BankAccount = null;
                        //newSourceHistoryRecord.HistoryItem = null;
                    }

                    //foreach (var newHistoryRecord in completedRegisterData.NewHistoryRecords)
                    //{
                    //    if (checkDate == null)
                    //    {
                    //        checkDate = newHistoryRecord.Date;
                    //    }
                    //    else
                    //    {
                    //        if (newHistoryRecord.Date < checkDate)
                    //            checkDate = newHistoryRecord.Date;
                    //    }
                    //}

                    context.SourceHistory.AddRange(completedRegisterData.NewSourceHistoryRecords);
                }
            }

            var result = context.DbContext.SaveEfChanges($"Saving Bank Account '{bankAccount.Description}' Source History");

            //var historyItems = context.History.Where(p => p.Date >= (DateTime)checkDate);

            return result;
        }

        private static bool SaveBudgetPeriodRecord(IHomeLogixDbContext context, BudgetPeriodHistory budgetPeriodHistoryRecord)
        {
            if (context.BudgetPeriodHistory.Any(a => a.PeriodType == budgetPeriodHistoryRecord.PeriodType &&
                                                     a.BudgetItemId == budgetPeriodHistoryRecord.BudgetItemId &&
                                                     a.PeriodEndingDate == budgetPeriodHistoryRecord.PeriodEndingDate))
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BudgetPeriodHistory,
                    budgetPeriodHistoryRecord,
                    $"Saving Budget Period Ending '{budgetPeriodHistoryRecord.PeriodEndingDate.ToString(CultureInfo.InvariantCulture)}'")
                )
                    return false;
            }
            else
            {
                if (!context.DbContext.AddNewNoCommitEntity(context.BudgetPeriodHistory,
                    budgetPeriodHistoryRecord,
                    $"Saving Budget Period Ending '{budgetPeriodHistoryRecord.PeriodEndingDate.ToString(CultureInfo.InvariantCulture)}'")
                )
                    return false;
            }

            return true;
        }

        public bool DeleteBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            var bankAccount = context.BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
            return bankAccount != null && context.DbContext.DeleteEntity(context.BankAccounts, bankAccount,
                $"Deleting Bank Account '{bankAccount.Description}'");
        }

        public bool SaveRegisterItem(BankAccountRegisterItem registerItem)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.DbContext.SaveEntity(context.BankAccountRegisterItems, registerItem,
                $"Saving Bank Account Register Item '{registerItem.Description}.'");
        }

        public bool SaveRegisterItems(List<BankAccountRegisterItem> registerItems)
        {
            var context = AppGlobals.GetNewDbContext();
            foreach (var registerItem in registerItems)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccountRegisterItems, registerItem,
                    $"Saving Bank Account Register Item '{registerItem.Description}.'")) 
                    return false;
            }

            return context.DbContext.SaveEfChanges("Saving Bank Register");
        }

        public bool SaveRegisterItem(BankAccountRegisterItem registerItem, List<BankAccountRegisterItemAmountDetail> amountDetails)
        {
            var context = AppGlobals.GetNewDbContext();

            if (!context.DbContext.SaveNoCommitEntity(context.BankAccountRegisterItems, registerItem,
                $"Saving Bank Account Register Item'{registerItem.Description}'"))
                return false;
        
            foreach (var amountDetail in amountDetails)
            {
                amountDetail.RegisterItem = null;
            }
            context.BankAccountRegisterItemAmountDetails.RemoveRange(
                context.BankAccountRegisterItemAmountDetails.Where(w => w.RegisterId == registerItem.Id).ToList());

           context.BankAccountRegisterItemAmountDetails.AddRange(amountDetails);

            return context.DbContext.SaveEfChanges($"Saving Bank Account Register Item '{registerItem.Description}.'");
        }

        public bool DeleteRegisterItems(List<BankAccountRegisterItem> registerItems)
        {
            var context = AppGlobals.GetNewDbContext();
            context.BankAccountRegisterItems.RemoveRange(registerItems);

            return context.DbContext.SaveEfChanges("Deleting Register Items");
        }

        public List<BudgetItem> GetBudgetItemsForBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItems.Where(w => w.BankAccountId == bankAccountId
                                                  || w.TransferToBankAccountId == bankAccountId).ToList();
        }

        public BudgetItem GetBudgetItem(int? budgetItemId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItems
                .Include(i => i.BankAccount)
                .Include(i => i.TransferToBankAccount)
                .FirstOrDefault(f => f.Id == budgetItemId);
        }

        public bool SaveBudgetItem(BudgetItem budgetItem, BankAccount dbBankAccount,
            BankAccount dbTransferToBankAccount, List<BankAccountRegisterItem> newBankRegisterItems,
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

            var addAfterSave = true;
            if (budgetItem.Id != 0)
            {
                addAfterSave = false;
                if (newBankRegisterItems != null)
                    context.BankAccountRegisterItems.AddRange(newBankRegisterItems);

                if (registerItemsToDelete != null)
                    context.BankAccountRegisterItems.RemoveRange(registerItemsToDelete);
            }

            var result = context.DbContext.SaveEfChanges("Saving Budget Item");
            if (!result)
                return false;

            if (addAfterSave && newBankRegisterItems != null)
            {
                foreach (var bankRegisterItem in newBankRegisterItems)
                {
                    bankRegisterItem.BudgetItemId = budgetItem.Id;
                }
                context.BankAccountRegisterItems.AddRange(newBankRegisterItems);
                return context.DbContext.SaveEfChanges("Saving Budget Item");
            }

            return true;
        }

        public bool SaveBudgetItem(BudgetItem budgetItem, List<BudgetPeriodHistory> budgetPeriodHistoryRecords, History newHistoryRecord)
        {
            var context = AppGlobals.GetNewDbContext();
            if (!context.DbContext.SaveNoCommitEntity(context.BudgetItems, budgetItem, "Saving Budget Item"))
                return false;

            foreach (var budgetPeriodHistoryRecord in budgetPeriodHistoryRecords)
            {
                if (!SaveBudgetPeriodRecord(context, budgetPeriodHistoryRecord)) return false;
            }

            context.History.Add(newHistoryRecord);

            if (!context.DbContext.SaveEfChanges($"Saving Budget Item '{budgetItem.Description}'"))
                return false;

            return true;
        }

        public bool DeleteBudgetItem(int budgetItemId, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount)
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

            var budgetItem = context.BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);
            return budgetItem != null && context.DbContext.DeleteEntity(context.BudgetItems, budgetItem, $"Deleting Budget Item '{budgetItem.Description}'");
        }

        public bool SaveGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
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
            
            return result;
        }

        public BankAccountRegisterItem GetBankAccountRegisterItem(int registerId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BankAccountRegisterItems.Include(p => p.BudgetItem)
                .Include(p => p.BankAccount)
                .FirstOrDefault(p => p.Id == registerId);
        }

        public BudgetItemSource GetBudgetItemSource(int sourceId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItemSources.FirstOrDefault(f => f.Id == sourceId);
        }

        public decimal GetSourceTotal(int storeId)
        {
            var context = AppGlobals.GetNewDbContext();
            var amount = context.SourceHistory.Where(p => p.SourceId == storeId).ToList().Sum(p => p.Amount);
            return amount;

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
            return context.BankAccountRegisterItems.Include(p => p.BankAccount)
                .FirstOrDefault(f => f.RegisterGuid == transferGuid);
        }

        public bool SaveNewRegisterItem(BankAccountRegisterItem registerItem, BankAccountRegisterItem transferRegisterItem = null)
        {
            var context = AppGlobals.GetNewDbContext();
            if (!context.DbContext.SaveNoCommitEntity(context.BankAccountRegisterItems, registerItem,
                $"Saving {registerItem.Description}"))
                return false;

            if (transferRegisterItem != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccountRegisterItems, transferRegisterItem,
                    $"Saving {transferRegisterItem.Description}"))
                    return false;
            }

            return context.DbContext.SaveEfChanges($"Saving {registerItem.Description}");
        }

        public BudgetPeriodHistory GetMaxMonthBudgetPeriodHistory()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetPeriodHistory.OrderByDescending(o => o.PeriodEndingDate)
                .FirstOrDefault(f => f.PeriodType == (int)PeriodHistoryTypes.Monthly);
        }

        public BudgetTotals GetBudgetTotals(DateTime monthEndDate, DateTime previousMonthEnding,
            DateTime nextMonthEnding)
        {
            var result = new BudgetTotals();
            var context = AppGlobals.GetNewDbContext();

            result.TotalProjectedMonthlyIncome =
                context.BudgetPeriodHistory
                    .Include(i => i.BudgetItem)
                    .Where(w => w.PeriodType == (int)PeriodHistoryTypes.Monthly &&
                                w.PeriodEndingDate.Year == monthEndDate.Year && w.PeriodEndingDate.Month == monthEndDate.Month &&
                                w.BudgetItem.Type == BudgetItemTypes.Income)
                    .Sum(s => s.ProjectedAmount);

            var registerAmount = context.BankAccountRegisterItems.Include(p => p.BudgetItem)
                .Where(w => (Relationship)decimal.Compare(w.ProjectedAmount, 0) == Relationship.GreaterThan &&
                            w.ItemDate.Month == monthEndDate.Month && w.ItemDate.Year == monthEndDate.Year &&
                            w.BudgetItem.Type == BudgetItemTypes.Income)
                .Sum(s => s.ProjectedAmount);

            result.TotalProjectedMonthlyIncome += registerAmount;

            result.TotalProjectedMonthlyExpenses =
                context.BudgetPeriodHistory
                    .Include(i => i.BudgetItem)
                    .Where(w => w.PeriodType == (int)PeriodHistoryTypes.Monthly &&
                                w.PeriodEndingDate.Year == monthEndDate.Year && w.PeriodEndingDate.Month == monthEndDate.Month &&
                                w.BudgetItem.Type == BudgetItemTypes.Expense)
                    .Sum(s => s.ProjectedAmount);

             registerAmount = context.BankAccountRegisterItems.Include(p => p.BudgetItem)
                .Where(w => (Relationship)decimal.Compare(w.ProjectedAmount, 0) == Relationship.LessThan &&
                            w.ItemDate.Month == monthEndDate.Month && w.ItemDate.Year == monthEndDate.Year &&
                            w.BudgetItem.Type == BudgetItemTypes.Expense)
                .Sum(s => s.ProjectedAmount);

            result.TotalProjectedMonthlyExpenses += Math.Abs(registerAmount);

            result.TotalActualMonthlyIncome = 
                context.BudgetPeriodHistory
                    .Include(i => i.BudgetItem)
                    .Where(w => w.PeriodType == (int) PeriodHistoryTypes.Monthly &&
                                w.PeriodEndingDate.Year == monthEndDate.Year && w.PeriodEndingDate.Month == monthEndDate.Month &&
                                w.BudgetItem.Type == BudgetItemTypes.Income)
                    .Sum(s => s.ActualAmount);

            registerAmount = context.BankAccountRegisterItems.Include(p => p.BudgetItem)
                .Where(w => (Relationship) decimal.Compare(w.ProjectedAmount, 0) == Relationship.GreaterThan &&
                            w.ItemDate.Month == monthEndDate.Month && w.ItemDate.Year == monthEndDate.Year &&
                            w.BudgetItem.Type == BudgetItemTypes.Income)
                .Sum(s => s.ProjectedAmount);

            result.TotalActualMonthlyIncome += registerAmount;

            result.TotalActualMonthlyExpenses =
                context.BudgetPeriodHistory
                    .Include(i => i.BudgetItem)
                    .Where(w => w.PeriodType == (int)PeriodHistoryTypes.Monthly &&
                                w.PeriodEndingDate.Year == monthEndDate.Year && w.PeriodEndingDate.Month == monthEndDate.Month &&
                                w.BudgetItem.Type == BudgetItemTypes.Expense)
                    .Sum(s => s.ActualAmount);

            registerAmount = context.BankAccountRegisterItems.Include(p => p.BudgetItem)
                .Where(w => (Relationship)decimal.Compare(w.ProjectedAmount, 0) == Relationship.LessThan &&
                            w.ItemDate.Month == monthEndDate.Month && w.ItemDate.Year == monthEndDate.Year &&
                            w.BudgetItem.Type == BudgetItemTypes.Expense)
                .Sum(s => s.ProjectedAmount);

            result.TotalActualMonthlyExpenses += Math.Abs(registerAmount);

            var yearEndDate = new DateTime(monthEndDate.Year, 12, 31);
            result.YearToDateIncome =
                context.BudgetPeriodHistory
                    .Include(i => i.BudgetItem)
                    .Where(w => w.PeriodType == (int)PeriodHistoryTypes.Yearly &&
                                w.PeriodEndingDate.Year == yearEndDate.Year && 
                                w.BudgetItem.Type == BudgetItemTypes.Income)
                    .Sum(s => s.ActualAmount);

            registerAmount = context.BankAccountRegisterItems.Include(p => p.BudgetItem)
                .Where(w => (Relationship)decimal.Compare(w.ProjectedAmount, 0) == Relationship.GreaterThan &&
                            w.ItemDate.Year == monthEndDate.Year && w.BudgetItem.Type == BudgetItemTypes.Income)
                .Sum(s => s.ProjectedAmount);

            result.YearToDateIncome += registerAmount;

            result.YearToDateExpenses =
                context.BudgetPeriodHistory
                    .Include(i => i.BudgetItem)
                    .Where(w => w.PeriodType == (int)PeriodHistoryTypes.Yearly &&
                                w.PeriodEndingDate.Year == yearEndDate.Year && w.BudgetItem.Type == BudgetItemTypes.Expense)
                    .Sum(s => s.ActualAmount);

            registerAmount = context.BankAccountRegisterItems.Include(p => p.BudgetItem)
                .Where(w => (Relationship)decimal.Compare(w.ProjectedAmount, 0) == Relationship.LessThan &&
                            w.ItemDate.Year == monthEndDate.Year && w.BudgetItem.Type == BudgetItemTypes.Expense)
                .Sum(s => s.ProjectedAmount);

            result.YearToDateExpenses += Math.Abs(registerAmount);

            result.PreviousMonthHasValues = context.BudgetPeriodHistory.Any(a =>
                a.PeriodType == (int) PeriodHistoryTypes.Monthly &&
                a.PeriodEndingDate.Year == monthEndDate.Year && a.PeriodEndingDate.Month == monthEndDate.Month);

            if (!result.PreviousMonthHasValues)
            {
                result.PreviousMonthHasValues = context.BankAccountRegisterItems.Any(a =>
                    a.ItemDate < previousMonthEnding);
            }


            result.NextMonthHasValues = context.BudgetPeriodHistory.Any(a =>
                a.PeriodType == (int)PeriodHistoryTypes.Monthly && 
                a.PeriodEndingDate.Year == nextMonthEnding.Year && a.PeriodEndingDate.Month == nextMonthEnding.Month);

            if (!result.NextMonthHasValues)
            {
                result.NextMonthHasValues = context.BankAccountRegisterItems.Any(a =>
                    a.ItemDate > nextMonthEnding.AddMonths(-1).AddDays(1));
            }

            return result;
        }

        public BudgetTotals GetBankBudgetTotals(DateTime monthEndDate, int bankAccountId)
        {
            var result =new BudgetTotals();
            var context = AppGlobals.GetNewDbContext();

            //var bankPeriodRecords = 
            //    context.BankAccountPeriodHistory
            //        .Include(i => i.BankAccount)
            //        .Where(w => w.PeriodType == (int)PeriodHistoryTypes.Monthly &&
            //                    w.PeriodEndingDate == monthEndDate && w.BankAccount.Id == bankAccountId);

            var historyExpenseAmount = context.History
                .Include(i => i.BankAccount)
                .Include(p => p.BudgetItem)
                .Where(p => p.BankAccountId == bankAccountId &&
                            p.Date.Year == monthEndDate.Year &&
                            p.Date.Month == monthEndDate.Month &&
                            p.ItemType == (int)DataAccess.Model.BankAccountRegisterItemTypes.BudgetItem &&
                            p.BudgetItem.Type == BudgetItemTypes.Expense).ToList().Sum(p => p.ProjectedAmount);

            var historyIncomeAmount = context.History
                .Include(i => i.BankAccount)
                .Include(p => p.BudgetItem)
                .Where(p => p.BankAccountId == bankAccountId &&
                            p.Date.Year == monthEndDate.Year &&
                            p.Date.Month == monthEndDate.Month &&
                            p.ItemType == (int)DataAccess.Model.BankAccountRegisterItemTypes.BudgetItem &&
                            p.BudgetItem.Type == BudgetItemTypes.Income).ToList().Sum(p => p.ProjectedAmount);

            result.TotalProjectedMonthlyIncome = historyIncomeAmount;
            result.TotalProjectedMonthlyExpenses = historyExpenseAmount;

            return result;
        }

        public History GetHistoryItem(int historyId)
        {
            var context = AppGlobals.GetNewDbContext();
            var result = context.History.Include(p => p.BudgetItem)
                .Include(p => p.BankAccount).
                Include(p => p.Sources).FirstOrDefault(p => p.Id == historyId);
            return result;
        }

        public SourceHistory GetSourceHistory(int historyId, int detailId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.SourceHistory.
                Include(p => p.HistoryItem).
                Include(p => p.Source)
                .FirstOrDefault(p => p.HistoryId == historyId && p.DetailId == detailId);
        }

        public List<BankTransaction> GetBankTransactions(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BankTransactions.Include(p => p.BudgetItems)
                .ThenInclude(p => p.BudgetItem)
                .Include(p => p.BankAccount)
                .Include(p => p.BudgetItem)
                .Include(p => p.Source)
                .Include(p => p.QifMap)
                .Where(p => p.BankAccountId == bankAccountId).ToList();
        }

        public bool SaveBankTransactions(List<BankTransaction> transactions, List<BankTransactionBudget> splits,
            int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            var existingTransactions = context.BankTransactions.Where(p => p.BankAccountId == bankAccountId).ToList();
            var existingSplits = context.BankTransactionBudget.Where(p => p.BankId == bankAccountId).ToList();

            context.BankTransactions.RemoveRange(existingTransactions);
            context.BankTransactionBudget.RemoveRange(existingSplits);

            context.BankTransactions.AddRange(transactions);
            context.BankTransactionBudget.AddRange(splits);

            return context.DbContext.SaveEfChanges("Saving Bank Transactions");
        }

        public bool DeleteTransactions(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            var existingTransactions = context.BankTransactions.Where(p => p.BankAccountId == bankAccountId).ToList();
            var existingSplits = context.BankTransactionBudget.Where(p => p.BankId == bankAccountId).ToList();

            context.BankTransactions.RemoveRange(existingTransactions);
            context.BankTransactionBudget.RemoveRange(existingSplits);

            return context.DbContext.SaveEfChanges("Deleting Bank Transactions");
        }

        public QifMap GetQifMap(int qifMapId)
        {
            var context = AppGlobals.GetNewDbContext();
            var result = context.QifMaps.Include(p => p.BudgetItem)
                .Include(p => p.Source)
                .FirstOrDefault(p => p.Id == qifMapId);

            return result;
        }

        public QifMap GetQifMap(string bankText)
        {
            var context = AppGlobals.GetNewDbContext();
            var result = context.QifMaps.Include(p => p.BudgetItem)
                .Include(p => p.Source)
                .FirstOrDefault(p => p.BankText == bankText);

            return result;
        }

        public bool SaveQifMap(QifMap qifMap)
        {
            var context = AppGlobals.GetNewDbContext();
            context.QifMaps.Add(qifMap);
            return context.DbContext.SaveEfChanges("Saving QifMap");
        }
    }
}
