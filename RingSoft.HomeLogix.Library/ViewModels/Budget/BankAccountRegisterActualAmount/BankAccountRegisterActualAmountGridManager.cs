using RingSoft.DataEntryControls.Engine.DataEntryGrid;

// ReSharper disable once CheckNamespace
namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public enum ActualAmountGridColumns
    {
        Date = BankAccountRegisterActualAmountGridManager.DateColumnId,
        Store = BankAccountRegisterActualAmountGridManager.StoreColumnId,
        Amount = BankAccountRegisterActualAmountGridManager.AmountColumnId
    }

    public class BankAccountRegisterActualAmountGridManager : DataEntryGridManager
    {
        public const int DateColumnId = 1;
        public const int StoreColumnId = 2;
        public const int AmountColumnId = 3;

        public BankAccountRegisterActualAmountViewModel ViewModel { get; }

        public BankAccountRegisterActualAmountGridManager(BankAccountRegisterActualAmountViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ActualAmountGridRow(this);
        }
    }
}
