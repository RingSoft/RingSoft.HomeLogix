using System;
using System.Collections.Generic;
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

        public BankAccountViewModel BankAccountViewModel { get; }

        public BankAccountRegisterGridManager(BankAccountViewModel viewModel) : base(viewModel)
        {
            BankAccountViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            throw new System.NotImplementedException();
        }

        protected override DbMaintenanceDataEntryGridRow<BankAccountRegisterItem> ConstructNewRowFromEntity(BankAccountRegisterItem entity)
        {
            var itemType = (BankAccountRegisterItemTypes) entity.ItemType;
            switch (itemType)
            {
                case BankAccountRegisterItemTypes.BudgetItem:
                    return new BankAccountRegisterGridBudgetItemRow(this);
                case BankAccountRegisterItemTypes.Miscellaneous:
                    break;
                case BankAccountRegisterItemTypes.TransferToBankAccount:
                    break;
                case BankAccountRegisterItemTypes.MonthlyEscrow:
                    return new BankAccountRegisterGridEscrowRow(this);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            throw new ArgumentOutOfRangeException();
        }

        public void AddGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> registerItems)
        {
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
        }

        public void CalculateProjectedBalanceData()
        {
            var newBalance = BankAccountViewModel.CurrentBalance - BankAccountViewModel.EscrowBalance.GetValueOrDefault(0);
            var lowestBalance = newBalance;
            var lowestBalanceDate = DateTime.Today;

            var rows = Rows.OfType<BankAccountRegisterGridRow>();
            foreach (var bankAccountRegisterGridRow in rows)
            {
                switch (bankAccountRegisterGridRow.TransactionType)
                {
                    case TransactionTypes.Deposit:
                        newBalance += bankAccountRegisterGridRow.ProjectedAmount;
                        break;
                    case TransactionTypes.Withdrawal:
                        newBalance -= bankAccountRegisterGridRow.ProjectedAmount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                bankAccountRegisterGridRow.Balance = newBalance;
                if (newBalance < lowestBalance)
                {
                    lowestBalance = newBalance;
                    lowestBalanceDate = bankAccountRegisterGridRow.ItemDate;
                }
            }

            BankAccountViewModel.NewProjectedEndingBalance = newBalance;
            BankAccountViewModel.ProjectedLowestBalanceAmount = lowestBalance;
            BankAccountViewModel.ProjectedLowestBalanceDate = lowestBalanceDate;

            Grid?.RefreshDataSource();
        }
    }
}
