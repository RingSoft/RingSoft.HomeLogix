using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
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
            var isDirty = ViewModel.RecordDirty;

            Grid.SetBulkInsertMode();
            foreach (var bankAccountRegisterItem in registerItems)
            {
                AddRowFromEntity(bankAccountRegisterItem);
            }

            var newList = Rows.OfType<BankAccountRegisterGridRow>().OrderBy(o => o.ItemDate)
                .ThenByDescending(t => t.ProjectedAmount).ToList();
            SetupForNewRecord();
            foreach (var registerGridRow in newList)
            {
                AddRow(registerGridRow);
            }

            Grid.SetBulkInsertMode(false);

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
            foreach (var bankAccountRegisterGridRow in rows)
            {
                if (lowestBalanceDate == null)
                    lowestBalanceDate = bankAccountRegisterGridRow.ItemDate.AddDays(-1);

                if (bankAccountRegisterGridRow.Completed)
                {
                    bankAccountRegisterGridRow.Balance = null;
                }
                else
                {
                    switch (bankAccountRegisterGridRow.TransactionType)
                    {
                        case TransactionTypes.Deposit:
                            switch (ViewModel.AccountType)
                            {
                                case BankAccountTypes.Checking:
                                case BankAccountTypes.Savings:
                                    newBalance += bankAccountRegisterGridRow.ProjectedAmount;
                                    break;
                                case BankAccountTypes.CreditCard:
                                    newBalance -= bankAccountRegisterGridRow.ProjectedAmount;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case TransactionTypes.Withdrawal:
                            switch (ViewModel.AccountType)
                            {
                                case BankAccountTypes.Checking:
                                case BankAccountTypes.Savings:
                                    newBalance -= bankAccountRegisterGridRow.ProjectedAmount;
                                    break;
                                case BankAccountTypes.CreditCard:
                                    newBalance += bankAccountRegisterGridRow.ProjectedAmount;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    bankAccountRegisterGridRow.Balance = newBalance;
                }

                if (newBalance < lowestBalance)
                {
                    lowestBalance = newBalance;
                    lowestBalanceDate = bankAccountRegisterGridRow.ItemDate;
                }
            }

            ViewModel.NewProjectedEndingBalance = newBalance;
            ViewModel.ProjectedLowestBalanceAmount = lowestBalance;
            ViewModel.ProjectedLowestBalanceDate = lowestBalanceDate;

            foreach (var row in Rows)
            {
                Grid?.UpdateRow(row);
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
                var registerItem = new BankAccountRegisterItem();
                bankAccountRegisterGridRow.SaveToEntity(registerItem, 0);
                registerItems.Add(registerItem);
            }

            ViewModel.CalculateTotals();
            AppGlobals.DataRepository.SaveRegisterItems(registerItems);
        }
    }
}
