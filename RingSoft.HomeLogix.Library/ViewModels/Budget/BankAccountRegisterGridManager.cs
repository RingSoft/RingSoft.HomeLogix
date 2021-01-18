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
        ProjectedAmount = BankAccountRegisterGridManager.ProjectedAmountColumnId,
        Completed = BankAccountRegisterGridManager.CompletedColumnId,
        Balance = BankAccountRegisterGridManager.BalanceColumnId,
        ActualAmount = BankAccountRegisterGridManager.ActualAmountColumnId,
        Difference = BankAccountRegisterGridManager.DifferenceColumnId
    }

    public class BankAccountRegisterGridManager : DbMaintenanceDataEntryGridManager<BankAccountRegisterItem>
    {
        public const int BudgetItemLineTypeId = (int) BankAccountRegisterItemTypes.BudgetItem;
        public const int MiscellaneousLineTypeId = (int) BankAccountRegisterItemTypes.Miscellaneous;
        public const int TransferToBankAccountLineTypeId = (int) BankAccountRegisterItemTypes.TansferToBankAccount;
        public const int MonthlyEscrowLineTypeId = (int) BankAccountRegisterItemTypes.MonthlyEscrow;

        public const int ItemTypeColumnId = 0;
        public const int DateColumnId = 1;
        public const int DescriptionColumnId = 2;
        public const int TransactionTypeColumnId = 3;
        public const int ProjectedAmountColumnId = 4;
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
            throw new System.NotImplementedException();
        }
    }
}
