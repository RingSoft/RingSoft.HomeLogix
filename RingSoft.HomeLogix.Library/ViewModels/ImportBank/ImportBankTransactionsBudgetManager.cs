using System.Linq;
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
            var result = new ImportBankTransactionsBudgetsGridRow(this);
            SetLastRowAmount(result);
            return result;
        }

        public void SetLastRowAmount(ImportBankTransactionsBudgetsGridRow gridRow = null)
        {
            var rows = Rows.OfType<ImportBankTransactionsBudgetsGridRow>();
            if (gridRow == null)
            {
                gridRow = rows.LastOrDefault();
            }
            var total = (decimal)0;
            foreach (var row in rows)
            {
                
                if (!(row.IsNew && row.BudgetAmount < 0))
                {
                    total += row.BudgetAmount;
                }
            }

            if (gridRow != null)
            {
                gridRow.BudgetAmount = ViewModel.TransactionAmount - total;
            }
            Grid?.RefreshGridView();
        }

    }
}
