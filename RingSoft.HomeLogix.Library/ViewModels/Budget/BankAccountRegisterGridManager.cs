using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public enum BankAccountRegisterGridColumns
    {
        ItemType = BankAccountRegisterGridManager.ItemTypeColumnId,
        Date = BankAccountRegisterGridManager.DateColumnId,
        Description = BankAccountRegisterGridManager.DescriptionColumnId,
        TransactionType = BankAccountRegisterGridManager.TransactionTypeColumnId,
        Amount = BankAccountRegisterGridManager.AmountColumnId,
        Completed = BankAccountRegisterGridManager.CompletedColumnId,
        Balance = BankAccountRegisterGridManager.BalanceColumnId,
        ActualAmount = BankAccountRegisterGridManager.ActualAmountColumnId,
        Difference = BankAccountRegisterGridManager.DifferenceColumnId
    }

    public enum TransactionTypes
    {
        Deposit = AppGlobals.BankTransactionTypeDepositId,
        Withdrawal = AppGlobals.BankTransactionWithdrawalId
    }

    public class BankAccountRegisterGridManager : DbMaintenanceDataEntryGridManager<BankAccountRegisterItem>
    {
        public const int ItemTypeColumnId = 0;
        public const int DateColumnId = 1;
        public const int DescriptionColumnId = 2;
        public const int TransactionTypeColumnId = 3;
        public const int AmountColumnId = 4;
        public const int CompletedColumnId = 5;
        public const int BalanceColumnId = 6;
        public const int ActualAmountColumnId = 7;
        public const int DifferenceColumnId = 8;

        public const int NegativeDisplayId = 101;
        public const int PositiveDisplayId = 102;

        public new BankAccountViewModel ViewModel { get; }
        public double MonthlyBudgetDeposits { get; set; }
        public double MonthlyBudgetWithdrawals { get; set; }


        private bool _noRemoveRowFromDb;

        public BankAccountRegisterGridManager(BankAccountViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }

        protected override DbMaintenanceDataEntryGridRow<BankAccountRegisterItem> ConstructNewRowFromEntity(
            BankAccountRegisterItem entity)
        {
            var itemType = (BankAccountRegisterItemTypes) entity.ItemType;
            switch (itemType)
            {
                case BankAccountRegisterItemTypes.BudgetItem:
                    return new BankAccountRegisterGridBudgetItemRow(this);
                case BankAccountRegisterItemTypes.Miscellaneous:
                    return new BankAccountRegisterGridMiscRow(this);
                case BankAccountRegisterItemTypes.TransferToBankAccount:
                    return new BankAccountRegisterGridTransferRow(this);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new ArgumentOutOfRangeException();
        }

        public void AddGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> registerItems)
        {
            Grid?.SetBulkInsertMode();
            foreach (var bankAccountRegisterItem in registerItems)
            {
                AddRowFromEntity(bankAccountRegisterItem);
            }

            ReSortGrid();
        }

        public void ReSortGrid()
        {
            var isDirty = ViewModel.RecordDirty;
            Grid?.SetBulkInsertMode();

            var newList = Rows.OfType<BankAccountRegisterGridRow>().OrderBy(o => o.ItemDate)
                .ThenByDescending(t => t.ProjectedAmount).ToList();
            SetupForNewRecord();
            foreach (var registerGridRow in newList)
            {
                AddRow(registerGridRow);
            }

            Grid?.SetBulkInsertMode(false);

            if (!isDirty)
                ViewModel.RecordDirty = false;
        }

        public new List<BankAccountRegisterItem> GetEntityList()
        {
            var result = new List<BankAccountRegisterItem>();
            var rows = Rows.OfType<BankAccountRegisterGridRow>().Where(w => w.Completed == false);
            var rowIndex = 1;
            foreach (var registerGridRow in rows)
            {
                var registerItem = new BankAccountRegisterItem();
                registerGridRow.SaveToEntity(registerItem, rowIndex, ViewModel.RegisterDetails);
                result.Add(registerItem);
                rowIndex++;
            }

            return result;
        }

        public void CalculateProjectedBalanceData()
        {
            var newBalance = ViewModel.CurrentBalance;
            var lowestBalance = newBalance;
            DateTime? lowestBalanceDate = null;

            var rows = Rows.OfType<BankAccountRegisterGridRow>();

            //09/30/2024
            //var bankTotals =
            //    AppGlobals.DataRepository.GetBankBudgetTotals(AppGlobals.MainViewModel.CurrentMonthEnding,
            //        ViewModel.Id, true);
            var banksToRefresh = new List<int>();
            var context = SystemGlobals.DataRepository.GetDataContext();

            foreach (var bankAccountRegisterGridRow in rows)
            {
                if ( bankAccountRegisterGridRow is BankAccountRegisterGridTransferRow transferRow1
                    && bankAccountRegisterGridRow.RegisterPayCCType == RegisterPayCCTypes.ToCC)
                {
                    var statementDate = bankAccountRegisterGridRow.ItemDate.AddMonths(-1);
                    statementDate = new DateTime(statementDate.Year, statementDate.Month,
                        ViewModel.StatementDayOfMonth);

                    var balanceRow = Rows.OfType<BankAccountRegisterGridRow>()
                        .OrderBy(p => p.ItemDate)
                        .ThenByDescending(p => p.ProjectedAmount)
                        .LastOrDefault(p => p.ItemDate <= statementDate && p.Completed == false);

                    var projectedAmount = 0.0;
                    if (balanceRow == null)
                    {
                        if (bankAccountRegisterGridRow.ProjectedAmount == 0)
                        {
                            projectedAmount = ViewModel.CurrentBalance;
                        }
                        else
                        {
                            projectedAmount = bankAccountRegisterGridRow.ProjectedAmount;
                        }
                        bankAccountRegisterGridRow.PayCCAllowEdit = true;
                    }
                    else
                    {
                        bankAccountRegisterGridRow.PayCCAllowEdit = false;
                        if (balanceRow.Balance != null)
                        {
                            projectedAmount = balanceRow.Balance.Value;
                        }
                    }

                    var deposits = Rows.OfType<BankAccountRegisterGridRow>()
                        .Where(p => p.RegisterPayCCType == RegisterPayCCTypes.None
                            && p.ItemDate >= statementDate
                        && p.ItemDate < bankAccountRegisterGridRow.ItemDate
                        && p.TransactionType == TransactionTypes.Deposit)
                        .Sum(p => p.ProjectedAmount > 0 ? p.ProjectedAmount : 0);

                    projectedAmount -= deposits;
                    bankAccountRegisterGridRow.ProjectedAmount = projectedAmount;

                    bankAccountRegisterGridRow.SaveToDbOnTheFly();
                }
                var registerData = bankAccountRegisterGridRow.GetRegisterData();
                newBalance = BankAccountViewModel.CalcNewBalance(ViewModel.AccountType, registerData, newBalance);

                if (registerData.ProjectedAmount.CompareTo(bankAccountRegisterGridRow.ProjectedAmount) != 0
                    || bankAccountRegisterGridRow.RegisterPayCCType == RegisterPayCCTypes.ToCC)
                {
                    switch (bankAccountRegisterGridRow.LineType)
                    {
                        case BankAccountRegisterItemTypes.BudgetItem:
                            break;
                        case BankAccountRegisterItemTypes.Miscellaneous:
                            break;
                        case BankAccountRegisterItemTypes.TransferToBankAccount:
                            if (bankAccountRegisterGridRow 
                                is BankAccountRegisterGridTransferRow transferRow)
                            {
                                var registerTable
                                    = context.GetTable<BankAccountRegisterItem>()
                                        .Include(p => p.BankAccount);
                                var fromBankRegisterItem =
                                    registerTable.FirstOrDefault(p => p.RegisterGuid
                                                                      == transferRow.TransferRegisterGuid);

                                if (fromBankRegisterItem != null)
                                {
                                    fromBankRegisterItem.ProjectedAmount = -bankAccountRegisterGridRow.ProjectedAmount;
                                    context.SaveEntity(fromBankRegisterItem, "");

                                    if (!banksToRefresh.Contains(fromBankRegisterItem.BankAccountId))
                                    {
                                        banksToRefresh.Add(fromBankRegisterItem.BankAccountId);
                                    }
                                }
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (lowestBalanceDate == null)
                    lowestBalanceDate = bankAccountRegisterGridRow.ItemDate.AddDays(-1);

                if (bankAccountRegisterGridRow.Completed)
                {
                    bankAccountRegisterGridRow.Balance = null;
                }
                else
                {
                    bankAccountRegisterGridRow.Balance = newBalance;
                }

                if (newBalance < lowestBalance)
                {
                    lowestBalance = newBalance;
                    lowestBalanceDate = bankAccountRegisterGridRow.ItemDate;
                }
            }

            var bankTotals = new BudgetTotals();
            var table = context.GetTable<BankAccountPeriodHistory>();
            var periodTotals = table.FirstOrDefault(
                p => p.BankAccountId == ViewModel.Id
                     && p.PeriodEndingDate.Year == AppGlobals.MainViewModel.CurrentMonthEnding.Year
                     && p.PeriodEndingDate.Month == AppGlobals.MainViewModel.CurrentMonthEnding.Month
                     && p.PeriodType == (byte)PeriodHistoryTypes.Monthly);
            if (periodTotals != null)
            {
                bankTotals.TotalProjectedMonthlyIncome = periodTotals.TotalDeposits;
                bankTotals.TotalProjectedMonthlyExpenses = periodTotals.TotalWithdrawals;
            }

            var monthBudgetDeposits = rows
                .Where(w => w.ProjectedAmount > 0 &&
                            w.ItemDate.Month == AppGlobals.MainViewModel.CurrentMonthEnding.Month && w.ItemDate.Year == AppGlobals.MainViewModel.CurrentMonthEnding.Year && w.TransactionType == TransactionTypes.Deposit)
                .Sum(s => s.ProjectedAmount);

            monthBudgetDeposits += bankTotals.TotalProjectedMonthlyIncome;

            var monthBudgetWithdrawals = rows
                .Where(w => w.ProjectedAmount > 0 &&
                            w.ItemDate.Month == AppGlobals.MainViewModel.CurrentMonthEnding.Month && w.ItemDate.Year == AppGlobals.MainViewModel.CurrentMonthEnding.Year && w.TransactionType == TransactionTypes.Withdrawal)
                .Sum(s => s.ProjectedAmount);

            monthBudgetWithdrawals += bankTotals.TotalProjectedMonthlyExpenses;

            MonthlyBudgetDeposits = monthBudgetDeposits;
            MonthlyBudgetWithdrawals = monthBudgetWithdrawals;


            ViewModel.NewProjectedEndingBalance = newBalance;
            ViewModel.ProjectedLowestBalanceAmount = lowestBalance;
            ViewModel.ProjectedLowestBalanceDate = lowestBalanceDate;
            ViewModel.MonthlyBudgetDeposits = MonthlyBudgetDeposits;
            ViewModel.MonthlyBudgetWithdrawals = MonthlyBudgetWithdrawals;

            foreach (var row in Rows)
            {
                Grid?.UpdateRow(row);
            }

            if (banksToRefresh.Any())
            {
                foreach (var bankId in banksToRefresh)
                {
                    foreach (var bankAccountViewModel in AppGlobals.MainViewModel.BankAccountViewModels)
                    {
                        if (bankAccountViewModel.Id == bankId)
                        {
                            bankAccountViewModel.RefreshAfterBudgetItemSave();
                        }
                    }
                }
            }
        }

        public void InternalRemoveRow(DataEntryGridRow row)
        {
            _noRemoveRowFromDb = true;
            RemoveRow(row);
            _noRemoveRowFromDb = false;
        }

        protected override void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    if (!_noRemoveRowFromDb)
                    {
                        var rowsToDelete = e.OldItems.OfType<BankAccountRegisterGridRow>();
                        var registersToDelete = new List<BankAccountRegisterItem>();
                        foreach (var registerGridRow in rowsToDelete)
                        {
                            var registerToDelete = new BankAccountRegisterItem();
                            registerGridRow.SaveToEntity(registerToDelete, 0);
                            registersToDelete.Add(registerToDelete);
                        }

                        AppGlobals.DataRepository.DeleteRegisterItems(registersToDelete);
                    }

                    ViewModel.CalculateTotals();
                    break;
            }

            base.OnRowsChanged(e);
        }

        public void CompleteAll(bool value)
        {
            var registerItems = new List<BankAccountRegisterItem>();
            var rows = Rows.OfType<BankAccountRegisterGridRow>();
            foreach (var bankAccountRegisterGridRow in rows)
            {
                bankAccountRegisterGridRow.SetComplete(value);
                if (!value)
                {
                    bankAccountRegisterGridRow.ActualAmount = null;
                }
                var registerItem = new BankAccountRegisterItem();
                bankAccountRegisterGridRow.SaveToEntity(registerItem, 0);
                registerItems.Add(registerItem);
            }

            ViewModel.CalculateTotals();
            AppGlobals.DataRepository.SaveRegisterItems(registerItems);
        }

        public override void LoadGrid(IEnumerable<BankAccountRegisterItem> entityList)
        {
            var listToLoad = new List<BankAccountRegisterItem>(
                entityList.OrderBy(p => p.ItemDate)
                    .ThenByDescending(p => p.ProjectedAmount));
            base.LoadGrid(listToLoad);
            if (ViewModel.InitRegisterId > 0)
            {
                var registerRows = Rows.OfType<BankAccountRegisterGridRow>();
                if (registerRows != null)
                {
                    var registerRow = registerRows.FirstOrDefault(
                        p => p.RegisterId == ViewModel.InitRegisterId);
                    if (registerRow != null)
                    {
                        ViewModel.BankAccountView.SetInitGridFocus(registerRow, (int)BankAccountRegisterGridColumns.Description);
                    }
                }
            }
        }
    }
}
