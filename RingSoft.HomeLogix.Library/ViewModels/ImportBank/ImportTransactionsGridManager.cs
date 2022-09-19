using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public class ImportTransactionsGridManager : DataEntryGridManager
    {
        public new ImportBankTransactionsViewModel ViewModel { get; set; }

        public ImportTransactionsGridManager(ImportBankTransactionsViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new ImportTransactionGridRow(this);
        }

        public bool SaveTransactions()
        {
            var rows = Rows.OfType<ImportTransactionGridRow>();
            var rowId = 1;
            var transactions = new List<BankTransaction>();
            var splits = new List<BankTransactionBudget>();
            foreach (var row in rows)
            {
                if (!row.IsNew)
                {
                    if (!row.BudgetItemSplits.Any())
                    {
                        if (row.BudgetItemAutoFillValue == null || !row.BudgetItemAutoFillValue.IsValid())
                        {
                            var message = "You must select a budget item for this transaction.";
                            var caption = "Invalid Budget Item";
                            ControlsGlobals.UserInterface.ShowMessageBox(message, caption,
                                RsMessageBoxIcons.Exclamation);
                            Grid.GotoCell(row, ImportTransactionGridRow.BudgetItemColumnId);
                            return false;
                        }
                    }

                    var bankTransaction = new BankTransaction();
                    bankTransaction.BankAccountId = ViewModel.BankViewModel.Id;
                    bankTransaction.TransactionId = rowId;
                    bankTransaction.TransactionDate = row.Date;
                    bankTransaction.BankTransactionText = row.BankText;
                    bankTransaction.Amount = row.Amount;
                    if (row.BudgetItemAutoFillValue != null && row.BudgetItemAutoFillValue.IsValid())
                    {
                        bankTransaction.BudgetId =
                            row.BudgetItemAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt();
                    }

                    if (row.SourceAutoFillValue != null && row.SourceAutoFillValue.IsValid())
                    {
                        bankTransaction.SourceId =
                            row.SourceAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt();
                    }

                    var budgetRowId = 1;
                    foreach (var rowBudgetItemSplit in row.BudgetItemSplits)
                    {
                        splits.Add(new BankTransactionBudget
                        {
                            BankId = bankTransaction.BankAccountId,
                            TransactionId = bankTransaction.TransactionId,
                            RowId = budgetRowId,
                            BudgetItemId =
                                rowBudgetItemSplit.BudgetItem.PrimaryKeyValue.KeyValueFields[0].Value.ToInt(),
                            Amount = rowBudgetItemSplit.Amount
                        });
                        budgetRowId++;
                    }
                    transactions.Add(bankTransaction);
                    rowId++;
                }

            }
            return AppGlobals.DataRepository.SaveBankTransactions(transactions, splits, ViewModel.BankViewModel.Id);
        }

        public void LoadGrid()
        {
            var transactions = AppGlobals.DataRepository.GetBankTransactions(ViewModel.BankViewModel.Id);
            foreach (var bankTransaction in transactions)
            {
                var bankRow = GetNewRow() as ImportTransactionGridRow;
                bankRow.Date = bankTransaction.TransactionDate;
                bankRow.BankText = bankTransaction.BankTransactionText;
                if (bankTransaction.BudgetItem != null)
                {
                    bankRow.BudgetItemAutoFillValue =
                        AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.BudgetItems,
                            bankTransaction.BudgetId.ToString());
                }
                if (bankTransaction.Source != null)
                {
                    bankRow.SourceAutoFillValue =
                        AppGlobals.LookupContext.OnAutoFillTextRequest(AppGlobals.LookupContext.BudgetItemSources,
                            bankTransaction.SourceId.ToString());
                }
                bankRow.Amount = bankTransaction.Amount;
                foreach (var bankTransactionBudgetItem in bankTransaction.BudgetItems)
                {
                    bankRow.BudgetItemSplits.Add(new BudgetSplit
                    {
                        BudgetItem = AppGlobals.LookupContext.OnAutoFillTextRequest(
                            AppGlobals.LookupContext.BudgetItems,
                            bankTransactionBudgetItem.BudgetItemId.ToString()),
                        Amount = bankTransactionBudgetItem.Amount,
                    });
                }
                AddRow(bankRow);
            }
            Grid?.RefreshGridView();
        }
    }
}
