using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;

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

        public List<BudgetSplit> SaveData()
        {
            var total = (decimal)0;
            var rows = Rows.OfType<ImportBankTransactionsBudgetsGridRow>();
            foreach (var importBankTransactionsBudgetsGridRow in rows)
            {
                if (importBankTransactionsBudgetsGridRow.BudgetAmount < 0)
                {
                    var message = "Row amount cannot be less than zero.";
                    var caption = "Invalid Row Amount";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return null;
                }

                if (importBankTransactionsBudgetsGridRow.BudgetAutoFillValue == null || !importBankTransactionsBudgetsGridRow.BudgetAutoFillValue.IsValid())
                {
                    if (importBankTransactionsBudgetsGridRow.BudgetAmount != 0)
                    {
                        var message = "Row budget item cannot be empty.";
                        var caption = "Invalid Row Budget Item";
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                        return null;
                    }
                }
                total += importBankTransactionsBudgetsGridRow.BudgetAmount;
            }

            if (total != ViewModel.TransactionAmount)
            {
                var message = "Total grid amount must equal the transaction amount.";
                var caption = "Invalid Amount";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return null;
            }

            var budgetsSplit = new List<BudgetSplit>();
            foreach (var importBankTransactionsBudgetsGridRow in rows)
            {
                if (!importBankTransactionsBudgetsGridRow.IsNew)
                {
                    budgetsSplit.Add(new BudgetSplit
                    {
                        BudgetItem = importBankTransactionsBudgetsGridRow.BudgetAutoFillValue,
                        Amount = importBankTransactionsBudgetsGridRow.BudgetAmount
                    });
                }
            }
            return budgetsSplit;
        }

        public void LoadGrid(List<BudgetSplit> splits)
        {
            if (splits == null || !splits.Any())
            {
                if (ViewModel.Row.BudgetItemAutoFillValue != null && ViewModel.Row.BudgetItemAutoFillValue.IsValid())
                {
                    var row = GetNewRow() as ImportBankTransactionsBudgetsGridRow;
                    row.BudgetAutoFillValue = ViewModel.Row.BudgetItemAutoFillValue;
                    row.BudgetAmount = ViewModel.Row.Amount;
                    AddRow(row);
                }
            }
            else
            {
                foreach (var budgetSplit in splits)
                {
                    var row = GetNewRow() as ImportBankTransactionsBudgetsGridRow;
                    row.BudgetAutoFillValue = budgetSplit.BudgetItem;
                    row.BudgetAmount = budgetSplit.Amount;
                    AddRow(row);
                }
            }

            Grid?.RefreshGridView();
        }
    }
}
