using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

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

        public bool PostTransactions()
        {
            var bankBalance = ViewModel.BankViewModel.CurrentBalance;
            var rows = Rows.OfType<ImportTransactionGridRow>();
            foreach (var gridRow in rows)
            {
                if (!gridRow.IsNew)
                {
                    if (gridRow.BudgetItemSplits.Any())
                    {
                        foreach (var gridRowBudgetItemSplit in gridRow.BudgetItemSplits)
                        {
                            var budgetItemId = gridRowBudgetItemSplit.BudgetItem.PrimaryKeyValue.KeyValueFields[0].Value
                                .ToInt();
                            var budgetItem = AppGlobals.DataRepository.GetBudgetItem(budgetItemId);
                            var row = PostBudgetItem(budgetItem, gridRowBudgetItemSplit.Amount, gridRow);
                            if (row != null)
                                bankBalance = UpdateBankBalance(row, bankBalance, gridRowBudgetItemSplit.Amount);
                        }
                    }
                    else
                    {
                        var budgetItemId = gridRow.BudgetItemAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value
                            .ToInt();
                        var budgetItem = AppGlobals.DataRepository.GetBudgetItem(budgetItemId);
                        var row = PostBudgetItem(budgetItem, gridRow.Amount, gridRow);
                        if (row != null) bankBalance = UpdateBankBalance(row, bankBalance, gridRow.Amount);
                    }
                }
            }

            if (AppGlobals.DataRepository.DeleteTransactions(ViewModel.BankViewModel.Id))
            {
                ViewModel.BankViewModel.CurrentBalance = bankBalance;
                ViewModel.View.CloseWindow(true);
                ViewModel.BankViewModel.DoSave();
                return true;
            }

            return false;
        }

        private static decimal UpdateBankBalance(BankAccountRegisterGridRow row, decimal bankBalance, decimal amount)
        {
            switch (row.TransactionType)
            {
                case TransactionTypes.Deposit:
                    bankBalance += amount;
                    break;
                case TransactionTypes.Withdrawal:
                    bankBalance -= amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return bankBalance;
        }

        private BankAccountRegisterGridRow PostBudgetItem(BudgetItem budgetItem, decimal amount, ImportTransactionGridRow gridRow)
        {
            var dateMinValue = gridRow.Date;
            var dateMaxValue = gridRow.Date;
            switch (budgetItem.RecurringType)
            {
                case BudgetItemRecurringTypes.Days:
                    dateMaxValue = dateMaxValue.AddDays(budgetItem.RecurringPeriod);
                    break;
                case BudgetItemRecurringTypes.Weeks:
                    dateMaxValue = dateMaxValue.AddDays(budgetItem.RecurringPeriod * 7);
                    break;
                case BudgetItemRecurringTypes.Months:
                    dateMaxValue = dateMaxValue.AddMonths(budgetItem.RecurringPeriod);
                    break;
                case BudgetItemRecurringTypes.Years:
                    dateMaxValue = dateMaxValue.AddYears(budgetItem.RecurringPeriod);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var registerRows = ViewModel.BankViewModel.RegisterGridManager.Rows.OfType<BankAccountRegisterGridBudgetItemRow>().OrderBy(p => p.ItemDate).ThenBy(p => p.ProjectedAmount);

            BankAccountRegisterGridRow registerRow = null;
            
            registerRow =
                registerRows.FirstOrDefault(p => p.BudgetItemId == budgetItem.Id &&
                                                 (p.ItemDate >= dateMinValue || p.ItemDate <= dateMaxValue));

            
            if (registerRow == null)
            {
                var transferRows = ViewModel.BankViewModel.RegisterGridManager.Rows
                    .OfType<BankAccountRegisterGridTransferRow>().OrderBy(p => p.ItemDate).ThenBy(p => p.ProjectedAmount);
                var transferRow = transferRows.FirstOrDefault(p => p.BudgetItemId == budgetItem.Id &&(p.ItemDate >= dateMinValue || p.ItemDate <= dateMaxValue));
                registerRow = transferRow;
            }

            if (registerRow != null)
            {
                if (gridRow.SourceAutoFillValue != null)
                {
                    var detailId = 1;
                    if (registerRow.ActualAmountDetails.Any())
                    {
                        detailId = registerRow.ActualAmountDetails.Max(p => p.DetailId) + 1;
                    }
                    registerRow.ActualAmountDetails.Add( new BankAccountRegisterItemAmountDetail()
                    {
                        Amount = amount,
                        Date = gridRow.Date,
                        SourceId = gridRow.SourceAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt(),
                        RegisterId = registerRow.RegisterId,
                        DetailId = detailId
                    });
                }

                var actualAmount = (decimal) 0;
                if (registerRow.ActualAmount != null)
                {
                    actualAmount = registerRow.ActualAmount.Value;
                    
                }
                registerRow.ActualAmount = actualAmount + amount;
                registerRow.Completed = true;
                var registerItem = new BankAccountRegisterItem();
                registerRow.SaveToEntity(registerItem, 0, registerRow.ActualAmountDetails.ToList());
                if (!AppGlobals.DataRepository.SaveRegisterItem(registerItem, registerRow.ActualAmountDetails))
                    return null;
                registerRow.Manager.Grid?.UpdateRow(registerRow);
                registerRow.Manager.CalculateProjectedBalanceData();
                return registerRow;
            }
            else
            {
                var bankRegisterItem = new BankAccountRegisterItem();
                bankRegisterItem.BankAccountId = gridRow.Manager.ViewModel.BankViewModel.Id;
                bankRegisterItem.ItemDate = gridRow.Date;
                bankRegisterItem.ItemType = (int)budgetItem.Type;
                bankRegisterItem.BudgetItemId = budgetItem.Id;
                bankRegisterItem.BudgetItem = budgetItem;
                bankRegisterItem.Description = $"Increase {budgetItem.Description} ";
                if (gridRow.SourceAutoFillValue != null && gridRow.SourceAutoFillValue.IsValid())
                {
                    bankRegisterItem.Description = gridRow.SourceAutoFillValue.Text;
                    bankRegisterItem.Description += $" {budgetItem.Description} ";
                }
                bankRegisterItem.ItemType = (int) BankAccountRegisterItemTypes.Miscellaneous;
                bankRegisterItem.Completed = true;
                bankRegisterItem.ProjectedAmount = amount;
                bankRegisterItem.ActualAmount = amount;
                BankAccountRegisterItem transferToRegisterItem = null;
                switch (budgetItem.Type)
                {
                    case BudgetItemTypes.Income:
                        bankRegisterItem.Description += "Income";
                        break;
                    case BudgetItemTypes.Expense:
                        bankRegisterItem.Description += "Expense";
                        bankRegisterItem.ProjectedAmount = -amount;
                        bankRegisterItem.ActualAmount = amount;
                        break;
                    case BudgetItemTypes.Transfer:
                        bankRegisterItem.Description = $"Transfer To {budgetItem.TransferToBankAccount.Description}";
                        bankRegisterItem.RegisterGuid = Guid.NewGuid().ToString();
                        bankRegisterItem.ItemType = (int) BankAccountRegisterItemTypes.TransferToBankAccount;
                        transferToRegisterItem = new BankAccountRegisterItem();

                        if (budgetItem.TransferToBankAccountId != null)
                            transferToRegisterItem.BankAccountId = budgetItem.TransferToBankAccountId.Value;
                        transferToRegisterItem.ProjectedAmount = gridRow.Amount;
                        transferToRegisterItem.BudgetItem = budgetItem;
                        transferToRegisterItem.BudgetItemId = budgetItem.Id;
                        transferToRegisterItem.RegisterGuid = Guid.NewGuid().ToString();
                        transferToRegisterItem.TransferRegisterGuid = bankRegisterItem.RegisterGuid;
                        transferToRegisterItem.ItemDate = gridRow.Date;
                        transferToRegisterItem.Description = $"Transfer From {budgetItem.BankAccount.Description}";
                        transferToRegisterItem.ItemType = (int)BankAccountRegisterItemTypes.TransferToBankAccount;
                        bankRegisterItem.ProjectedAmount = -amount;
                        bankRegisterItem.TransferRegisterGuid = transferToRegisterItem.RegisterGuid;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (!AppGlobals.DataRepository.SaveNewRegisterItem(bankRegisterItem, transferToRegisterItem))
                    return null;

                ViewModel.BankViewModel.RegisterGridManager.AddGeneratedRegisterItems(new List<BankAccountRegisterItem>() {bankRegisterItem});
                var newRow = ViewModel.BankViewModel.RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>()
                    .FirstOrDefault(p => p.RegisterId == bankRegisterItem.Id);
                gridRow.Manager.ViewModel.BankViewModel.RegisterGridManager.Grid?.RefreshGridView();
                gridRow.Manager.ViewModel.BankViewModel.RegisterGridManager.CalculateProjectedBalanceData();
                return newRow;
            }
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
