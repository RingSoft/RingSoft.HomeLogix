using System;
using System.Collections.Generic;
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
            switch (entity.ItemType)
            {
                case BankAccountRegisterItemTypes.BudgetItem:
                    return new BankAccountRegisterGridBudgetItemRow(this);
                case BankAccountRegisterItemTypes.Miscellaneous:
                    break;
                case BankAccountRegisterItemTypes.TansferToBankAccount:
                    break;
                case BankAccountRegisterItemTypes.MonthlyEscrow:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            throw new ArgumentOutOfRangeException();
        }

        public void AddGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> registerItems)
        {
            LoadGrid(registerItems);
        }
    }
}
