using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
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
                    if (!ValidateTransactionRow(row, false)) return false;

                    var bankTransaction = new BankTransaction();
                    if (row != null)
                    {
                        if (row.QifMap != null) bankTransaction.QifMapId = row.QifMap.Id;
                        bankTransaction.BankAccountId = ViewModel.BankViewModel.Id;
                        bankTransaction.TransactionId = rowId;
                        bankTransaction.TransactionDate = row.Date;
                        bankTransaction.BankTransactionText = row.BankText;
                        bankTransaction.Amount = row.Amount;
                        bankTransaction.MapTransaction = row.MapTransaction;
                        bankTransaction.TransactionType = (byte) row.TransactionTypes;
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
                    }

                    transactions.Add(bankTransaction);
                    rowId++;
                }

            }
            SaveQifMaps();
            return AppGlobals.DataRepository.SaveBankTransactions(transactions, splits, ViewModel.BankViewModel.Id);
        }

        private bool ValidateTransactionRow(ImportTransactionGridRow row, bool post)
        {
            if (!row.BudgetItemSplits.Any())
            {
                if (post)
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
            }

            return true;
        }

        public bool PostTransactions()
        {
            var bankBalance = ViewModel.BankViewModel.CurrentBalance;
            var rows = Rows.OfType<ImportTransactionGridRow>().Where(p => p.IsNew == false);

            foreach (var gridRow in rows)
            {
                if (!ValidateTransactionRow(gridRow, true)) return false;
            }

            var count = rows.Count();
            var rowIndex = 0;
            foreach (var gridRow in rows)
            {
                rowIndex++;
                ViewModel.View.UpdateStatus($"Posting Row {rowIndex} of {count}");
                if (gridRow.BudgetItemSplits.Any())
                {
                    foreach (var gridRowBudgetItemSplit in gridRow.BudgetItemSplits)
                    {
                        var budgetItemId = gridRowBudgetItemSplit.BudgetItem.PrimaryKeyValue.KeyValueFields[0].Value
                            .ToInt();
                        var budgetItem = AppGlobals.DataRepository.GetBudgetItem(budgetItemId);
                        var amount = ProcessAmount(gridRowBudgetItemSplit.Amount, gridRow, budgetItem);
                        var row = PostBudgetItem(budgetItem, gridRowBudgetItemSplit.Amount, gridRow);
                        if (row != null)
                            bankBalance = UpdateBankBalance(row, bankBalance, amount);
                    }
                }
                else
                {
                    var budgetItemId = gridRow.BudgetItemAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value
                        .ToInt();
                    var budgetItem = AppGlobals.DataRepository.GetBudgetItem(budgetItemId);
                    var amount = ProcessAmount(gridRow.Amount, gridRow, budgetItem);
                    var row = PostBudgetItem(budgetItem, amount, gridRow);
                    if (row != null) bankBalance = UpdateBankBalance(row, bankBalance, amount);
                }
            }

            if (AppGlobals.DataRepository.DeleteTransactions(ViewModel.BankViewModel.Id))
            {
                if (rows.Any())
                {
                    SaveQifMaps();
                    var lastCompletedDate = rows.Max(p => p.Date);
                    ViewModel.BankViewModel.LastCompleteDate = lastCompletedDate;
                    ViewModel.BankViewModel.CurrentBalance = bankBalance;
                    ViewModel.BankViewModel.RegisterGridManager.CalculateProjectedBalanceData();
                    ViewModel.BankViewModel.RegisterGridManager.Grid?.RefreshGridView();
                    ViewModel.View.CloseWindow(true);
                    ViewModel.BankViewModel.SaveNoPost();
                }
                else
                {
                    ViewModel.View.CloseWindow(true);
                }

                return true;
            }

            return false;
        }

        private decimal ProcessAmount(decimal rowAmount, ImportTransactionGridRow gridRow, BudgetItem budgetItem)
        {
            var result = rowAmount;
            if (gridRow != null)
            {
                switch (budgetItem.Type)
                {
                    case BudgetItemTypes.Income:
                        switch (gridRow.TransactionTypes)
                        {
                            case TransactionTypes.Deposit:
                                break;
                            case TransactionTypes.Withdrawal:
                                result = -rowAmount;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case BudgetItemTypes.Expense:
                        switch (gridRow.TransactionTypes)
                        {
                            case TransactionTypes.Deposit:
                                result = -rowAmount;
                                break;
                            case TransactionTypes.Withdrawal:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                }
            }
            return result;
        }

        private void SaveQifMaps()
        {
            foreach (var gridRow in Rows.OfType<ImportTransactionGridRow>().Where(p => p.IsNew == false && p.MapTransaction == true && !p.BankText.IsNullOrEmpty()))
            {
                if (gridRow.BudgetItemAutoFillValue != null && gridRow.BudgetItemAutoFillValue.IsValid())
                {
                    var qifMap = AppGlobals.DataRepository.GetQifMap(gridRow.BankText);
                    if (qifMap == null)
                    {
                        qifMap = new QifMap();
                        qifMap.BankText = gridRow.BankText;
                        qifMap.BudgetId = AppGlobals.LookupContext.BudgetItems
                            .GetEntityFromPrimaryKeyValue(gridRow.BudgetItemAutoFillValue.PrimaryKeyValue).Id;
                        if (gridRow.SourceAutoFillValue != null && gridRow.SourceAutoFillValue.IsValid())
                        {
                            qifMap.SourceId = AppGlobals.LookupContext.BudgetItemSources
                                .GetEntityFromPrimaryKeyValue(gridRow.SourceAutoFillValue.PrimaryKeyValue).Id;
                        }

                        AppGlobals.DataRepository.SaveQifMap(qifMap);
                    }
                }
            }
        }

        private decimal UpdateBankBalance(BankAccountRegisterGridRow row, decimal bankBalance, decimal amount)
        {
            switch (row.TransactionType)
            {
                case TransactionTypes.Deposit:
                    if (ViewModel.BankViewModel.AccountType == BankAccountTypes.CreditCard)
                    {
                        bankBalance -= amount;
                    }
                    else
                    {
                        bankBalance += amount;
                    }
                    break;
                case TransactionTypes.Withdrawal:
                    if (ViewModel.BankViewModel.AccountType == BankAccountTypes.CreditCard)
                    {
                        bankBalance += amount;
                    }
                    else
                    {
                        bankBalance -= amount;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return bankBalance;
        }

        private BankAccountRegisterGridRow PostBudgetItem(BudgetItem budgetItem, decimal amount, ImportTransactionGridRow gridRow)
        {
            var dateMinValue = gridRow.Date;
            var incompleteRows = ViewModel.BankViewModel.RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>().Where(p => !p.Completed && p.BudgetItemId == budgetItem.Id);
            if (incompleteRows.Any())
            {
                var budgetDate = incompleteRows.Min(p => p.ItemDate);
                if (budgetDate < dateMinValue)
                {
                    dateMinValue = budgetDate;
                }
            }

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
                                                 (p.ItemDate >= dateMinValue && p.ItemDate <= dateMaxValue));

            if (registerRow == null)
            {
                registerRow = registerRows.FirstOrDefault(p => p.BudgetItemId == budgetItem.Id);
            }

            
            if (registerRow == null)
            {
                var transferRows = ViewModel.BankViewModel.RegisterGridManager.Rows
                    .OfType<BankAccountRegisterGridTransferRow>().OrderBy(p => p.ItemDate).ThenBy(p => p.ProjectedAmount);
                var transferRow = transferRows.FirstOrDefault(p => p.BudgetItemId == budgetItem.Id &&(p.ItemDate >= dateMinValue || p.ItemDate <= dateMaxValue));
                registerRow = transferRow;
            }

            if (registerRow != null)
            {
                var registerItem = new BankAccountRegisterItem();
                if (gridRow.SourceAutoFillValue != null)
                {
                    var detailId = 1;
                    if (registerRow.ActualAmountDetails.Any())
                    {
                        detailId = registerRow.ActualAmountDetails.Max(p => p.DetailId) + 1;
                    }

                    registerRow.ActualAmountDetails.Add(new BankAccountRegisterItemAmountDetail()
                    {
                        Amount = amount,
                        Date = gridRow.Date,
                        SourceId = gridRow.SourceAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt(),
                        RegisterId = registerRow.RegisterId,
                        DetailId = detailId,
                        BankText = gridRow.BankText
                    });
                    if (!UpdateRegisterRow(amount, registerRow, registerItem, gridRow)) return null;
                }
                else
                {
                    if (!UpdateRegisterRow(amount, registerRow, registerItem, gridRow)) return null;
                }
                return registerRow;

            }
            else
            {
                var row = AddNewGridRow(budgetItem, amount, gridRow);
                if (row == null) return null;
                return row;
            }
        }

        private static bool UpdateRegisterRow(decimal amount, BankAccountRegisterGridRow registerRow,  
            BankAccountRegisterItem registerItem, ImportTransactionGridRow importTransactionGridRow)
        {
            var actualAmount = (decimal) 0;
            if (registerRow.ActualAmount != null)
            {
                actualAmount = registerRow.ActualAmount.Value;
            }

            registerRow.ActualAmount = actualAmount + amount;
            registerRow.BankText = importTransactionGridRow.BankText;
            registerRow.Completed = true;
            
            registerRow.SaveToEntity(registerItem, 0, registerRow.ActualAmountDetails.ToList());
            if (!AppGlobals.DataRepository.SaveRegisterItem(registerItem, registerRow.ActualAmountDetails))
                return false;
            registerRow.Manager.Grid?.UpdateRow(registerRow);
            return true;
        }

        private BankAccountRegisterGridRow AddNewGridRow(BudgetItem budgetItem, decimal amount, ImportTransactionGridRow gridRow)
        {
            var bankRegisterItem = new BankAccountRegisterItem();
            bankRegisterItem.BankAccountId = gridRow.Manager.ViewModel.BankViewModel.Id;
            bankRegisterItem.ItemDate = gridRow.Date;
            bankRegisterItem.ItemType = (int) budgetItem.Type;
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
            bankRegisterItem.BankText = gridRow.BankText;
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
                    transferToRegisterItem.ItemType = (int) BankAccountRegisterItemTypes.TransferToBankAccount;
                    transferToRegisterItem.BankText = gridRow.BankText;
                    bankRegisterItem.ProjectedAmount = -amount;
                    bankRegisterItem.TransferRegisterGuid = transferToRegisterItem.RegisterGuid;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!AppGlobals.DataRepository.SaveNewRegisterItem(bankRegisterItem, transferToRegisterItem))
                return null;

            ViewModel.BankViewModel.RegisterGridManager.AddGeneratedRegisterItems(new List<BankAccountRegisterItem>()
                {bankRegisterItem});
            var newRow = ViewModel.BankViewModel.RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>()
                .FirstOrDefault(p => p.RegisterId == bankRegisterItem.Id);
            return newRow;
        }

        public void LoadGrid()
        {
            var transactions = AppGlobals.DataRepository.GetBankTransactions(ViewModel.BankViewModel.Id);
            foreach (var bankTransaction in transactions)
            {
                var bankRow = GetNewRow() as ImportTransactionGridRow;
                bankRow.QifMap = bankTransaction.QifMap;
                bankRow.Date = bankTransaction.TransactionDate;
                bankRow.BankText = bankTransaction.BankTransactionText;
                bankRow.MapTransaction = bankTransaction.MapTransaction;
                bankRow.TransactionTypes = (TransactionTypes) bankTransaction.TransactionType;
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
                bankRow.QifMap = bankTransaction.QifMap;
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

        public void ImportFromQif(List<ImportTransactionGridRow> rows)
        {
            var existingRows = Rows.OfType<ImportTransactionGridRow>().Where(p => !p.IsNew).ToList();
            rows.AddRange(existingRows);
            rows = rows.OrderBy(p => p.Date).ToList();
            Grid?.SetBulkInsertMode(true);
            PreLoadGridFromEntity();
            var rowIndex = 0;
            var count = rows.Count;
            foreach (var importTransactionGridRow in rows)
            {
                rowIndex++;
                ViewModel.View.UpdateStatus($"Loading Row {rowIndex} of {count}");
                if (!importTransactionGridRow.BankText.IsNullOrEmpty())
                {
                    var query = new SelectQuery(AppGlobals.LookupContext.QifMaps.TableName);
                    var foundMap = false;
                    var spacePos = importTransactionGridRow.BankText.IndexOf(" ");
                    var text = importTransactionGridRow.BankText;
                    if (spacePos > 0)
                        text = importTransactionGridRow.BankText.MidStr(0, spacePos).Trim();
                    while (foundMap == false)
                    {
                        if (query.WhereItems.Any())
                        {
                            query.RemoveWhereItem(query.WhereItems[0]);
                        }

                        query.AddWhereItem(
                            AppGlobals.LookupContext.QifMaps.GetFieldDefinition(p => p.BankText).FieldName,
                            Conditions.Contains, text);

                        var result = AppGlobals.LookupContext.DataProcessor.GetData(query);
                        if (result.ResultCode == GetDataResultCodes.Success)
                        {
                            var table = result.DataSet.Tables[0];
                            if (table.Rows.Count == 1)
                            {
                                foundMap = true;
                                var qifMapId =
                                    table.Rows[0].GetRowValue(AppGlobals.LookupContext.QifMaps
                                        .GetFieldDefinition(p => p.Id).FieldName).ToInt();

                                var qifMap = AppGlobals.DataRepository.GetQifMap(qifMapId);

                                importTransactionGridRow.QifMap = qifMap;
                                if (qifMap.BudgetItem != null)
                                {
                                    importTransactionGridRow.BudgetItemAutoFillValue =
                                        AppGlobals.LookupContext.OnAutoFillTextRequest(
                                            AppGlobals.LookupContext.BudgetItems,
                                            qifMap.BudgetId.ToString());
                                }

                                if (qifMap.Source != null)
                                {
                                    importTransactionGridRow.SourceAutoFillValue =
                                        AppGlobals.LookupContext.OnAutoFillTextRequest(
                                            AppGlobals.LookupContext.BudgetItemSources,
                                            qifMap.SourceId.ToString());
                                }

                            }
                            else if (table.Rows.Count == 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }

                        spacePos = importTransactionGridRow.BankText.IndexOf(" ", spacePos + 1);
                        if (spacePos == -1)
                        {
                            foundMap = true;
                        }
                        else
                        {
                            text = importTransactionGridRow.BankText.MidStr(0, spacePos).Trim();
                        }

                    }
                }

                AddRow(importTransactionGridRow);
            }
            PostLoadGridFromEntity();
            Grid?.SetBulkInsertMode(false);
            Grid?.RefreshGridView();
        }
    }
}
