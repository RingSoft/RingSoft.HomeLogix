using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public class ImportBankTransactionsBudgetManager : DataEntryGridManager
    {
        public new ImportBankTransactionsBudgetsViewModel ViewModel { get; set; }

        public ImportBankTransactionsBudgetManager(ImportBankTransactionsBudgetsViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        protected override DataEntryGridRow GetNewRow()
        {
            return new ImportBankTransactionsBudgetsGridRow(this);
        }
    }
}
