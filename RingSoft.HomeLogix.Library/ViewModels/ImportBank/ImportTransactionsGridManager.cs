using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.TableProcessing;

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
                        bankTransaction.Description = row.Description;
                        bankTransaction.Amount = row.Amount;
                        bankTransaction.MapTransaction = row.MapTransaction;
                        bankTransaction.FromBank = row.FromBank;
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

        private bool BudgetItemFoundInRegister(AutoFillValue budgetAutoFillValue, 
            IEnumerable<BankAccountRegisterGridRow> registerGridRows)
        {
            var budgetItem =
                AppGlobals.LookupContext.BudgetItems.GetEntityFromPrimaryKeyValue(budgetAutoFillValue.PrimaryKeyValue);

            IQueryable<BudgetItem> budgetTable = AppGlobals.DataRepository.GetDataContext().GetTable<BudgetItem>();
            budgetItem = budgetTable.FirstOrDefault(p => p.Id == budgetItem.Id);
            if (budgetItem.BankAccountId != ViewModel.BankViewModel.Id)
            {
                return false;
            }

            if (!registerGridRows.Any(p => p.BudgetItemId == budgetItem.Id))
            {
                return false;
            }

            return true;
        }

        public bool PostTransactions()
        {
            var bankBalance = ViewModel.BankViewModel.CurrentBalance;
            var rows = Rows.OfType<ImportTransactionGridRow>().Where(p => p.IsNew == false);

            var count = rows.Count();
            var rowIndex = 0;
            
            var budgetItemsFound = true;
            var registerRows = ViewModel.BankViewModel.RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>();

            foreach (var gridRow in rows)
            {
                rowIndex++;
                ViewModel.View.UpdateStatus($"Validating Row {rowIndex} of {count}");
                if (!ValidateTransactionRow(gridRow, true)) return false;

                if (gridRow.BudgetItemSplits.Any())
                {
                    foreach (var budgetSplit in gridRow.BudgetItemSplits)
                    {
                        if (registerRows != null && !BudgetItemFoundInRegister(budgetSplit.BudgetItem, registerRows))
                        {
                            budgetItemsFound = false;
                        }
                    }
                }
                else
                {
                    if (registerRows != null && !BudgetItemFoundInRegister(gridRow.BudgetItemAutoFillValue, registerRows))
                    {
                        budgetItemsFound = false;
                    }
                }
            }

            if (!budgetItemsFound)
            {
                var message =
                    "Some Budget Items in these transactions were not found in the Registered Grid of this Bank Account. Are you sure you are importing these transactions into the right Bank Account?";
                var caption = "Bank Account Validation";
                if (!ViewModel.View.ShowYesNoMessage(message, caption))
                {
                    return false;
                }
            }

            rowIndex = 0;
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
                        var row = PostBudgetItem(budgetItem, gridRowBudgetItemSplit.Amount, gridRow, true);
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
                    var row = PostBudgetItem(budgetItem, amount, gridRow, false);
                    if (row != null) 
                        bankBalance = UpdateBankBalance(row, bankBalance, amount);
                }
            }

            var importMaxDate = rows.Max(p => p.Date);
            var registerIncompleteRows = ViewModel.BankViewModel.RegisterGridManager.Rows
                .OfType<BankAccountRegisterGridRow>().Where(p => p.ItemDate < importMaxDate && !p.Completed)
                .ToList();

            if (registerIncompleteRows.Any())
            {
                ViewModel.View.ShowExpiredWindow(registerIncompleteRows);
                //var message = "There are expired budget items in the register grid. Do you wish to delete them?";
                //var caption = "Delete Expired Register Items";
                //if (ViewModel.View.ShowYesNoMessage(message, caption))
                //{
                    foreach (var registerGridRow in registerIncompleteRows)
                    {
                        ViewModel.BankViewModel.RegisterGridManager.RemoveRow(registerGridRow);
                        //registerGridRow.ActualAmount = 0;
                        //registerGridRow.Completed = true;
                        //var registerItem = new BankAccountRegisterItem();
                        //registerGridRow.SaveToEntity(registerItem, 0, registerGridRow.ActualAmountDetails.ToList());
                        //if (!AppGlobals.DataRepository.SaveRegisterItem(registerItem, registerGridRow.ActualAmountDetails))
                        //    return false;
                    }
                //}
            }

            if (AppGlobals.DataRepository.DeleteTransactions(ViewModel.BankViewModel.Id))
            {
                if (rows.Any())
                {
                    SaveQifMaps();
                    //if (ViewModel.BankViewModel.LastCompleteDate.Value.Year != 1980)
                    {
                        ViewModel.BankViewModel.CurrentBalance = bankBalance;
                    }

                    var lastCompletedDate = rows.Max(p => p.Date);
                    ViewModel.BankViewModel.LastCompleteDate = lastCompletedDate;
                    ViewModel.BankViewModel.RegisterGridManager.CalculateProjectedBalanceData();
                    ViewModel.BankViewModel.RegisterGridManager.Grid?.RefreshGridView();
                    ViewModel.View.CloseWindow(true);
                    ViewModel.BankViewModel.SaveNoPost();
                    ViewModel.BankViewModel.RecordDirty = true;
                }
                else
                {
                    ViewModel.View.CloseWindow(true);
                }

                return true;
            }

            return false;
        }

        private double ProcessAmount(double rowAmount, ImportTransactionGridRow gridRow, BudgetItem budgetItem)
        {
            var result = rowAmount;
            if (gridRow != null)
            {
                switch ((BudgetItemTypes)budgetItem.Type)
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
            foreach (var gridRow in Rows.OfType<ImportTransactionGridRow>().Where(p => p.IsNew == false && p.MapTransaction == true && !p.Description.IsNullOrEmpty()))
            {
                if (gridRow.BudgetItemAutoFillValue != null && gridRow.BudgetItemAutoFillValue.IsValid())
                {
                    var qifMap = AppGlobals.DataRepository.GetQifMap(gridRow.Description);
                    if (qifMap == null)
                    {
                        qifMap = new QifMap();
                        qifMap.BankText = gridRow.Description;
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

        private double UpdateBankBalance(BankAccountRegisterGridRow row, double bankBalance, double amount)
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

        private BankAccountRegisterGridRow PostBudgetItem(BudgetItem budgetItem, double amount, ImportTransactionGridRow gridRow, bool isSplit)
        {
            var dateMinValue = gridRow.Date;
            var incompleteRows = ViewModel.BankViewModel.RegisterGridManager.Rows
                .OfType<BankAccountRegisterGridRow>()
                .Where(p => !p.Completed && p.BudgetItemId == budgetItem.Id);
            if (incompleteRows.Any())
            {
                var budgetDate = incompleteRows.Min(p => p.ItemDate);
                if (budgetDate < dateMinValue)
                {
                    dateMinValue = budgetDate;
                }
                if (gridRow.Date > dateMinValue)
                {
                    dateMinValue = budgetDate;
                }
            }

            var dateMaxValue = gridRow.Date;
            switch ((BudgetItemRecurringTypes)budgetItem.RecurringType)
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

            var registerRows 
                = ViewModel.BankViewModel.RegisterGridManager.Rows
                    .OfType<BankAccountRegisterGridRow>()
                    .OrderBy(p => p.ItemDate)
                    .ThenBy(p => p.ProjectedAmount);

            BankAccountRegisterGridRow registerRow = null;
            
            registerRow =
                registerRows.FirstOrDefault(p => p.BudgetItemId == budgetItem.Id &&
                                                 (p.ItemDate >= dateMinValue && p.ItemDate < dateMaxValue)
                                                 && p.Completed == false);

            if (registerRow == null)
            {
                registerRow = registerRows.FirstOrDefault(p => p.BudgetItemId == budgetItem.Id
                                                               && p.ItemDate <= dateMaxValue
                                                               && p.Completed == false);
            }
            
            if (registerRow == null)
            {
                var transferRows = ViewModel.BankViewModel.RegisterGridManager.Rows
                    .OfType<BankAccountRegisterGridTransferRow>().OrderBy(p => p.ItemDate).ThenBy(p => p.ProjectedAmount);
                var transferRow = transferRows.FirstOrDefault(p => 
                    p.BudgetItemId == budgetItem.Id &&(p.ItemDate >= dateMinValue || p.ItemDate < dateMaxValue)
                                                    && p.Completed == false);
             
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
                        BankText = gridRow.Description
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
                var row = AddNewGridRow(budgetItem, amount, gridRow, isSplit);
                if (row == null) return null;
                return row;
            }
        }

        private static bool UpdateRegisterRow(double amount, BankAccountRegisterGridRow registerRow,  
            BankAccountRegisterItem registerItem, ImportTransactionGridRow importTransactionGridRow)
        {
            var actualAmount = (double) 0;
            if (registerRow.ActualAmount != null)
            {
                actualAmount = registerRow.ActualAmount.Value;
            }

            registerRow.ActualAmount = actualAmount + amount;
            registerRow.BankText = importTransactionGridRow.Description;
            registerRow.Completed = true;

            registerRow.SaveToEntity(registerItem, 0, registerRow.ActualAmountDetails.ToList());
            if (!AppGlobals.DataRepository.SaveRegisterItem(registerItem, registerRow.ActualAmountDetails))
                return false;
            registerRow.Manager.Grid?.UpdateRow(registerRow);
            return true;
        }

        private BankAccountRegisterGridRow AddNewGridRow
            (BudgetItem budgetItem, double amount, ImportTransactionGridRow gridRow, bool isSplit)
        {
            var bankRegisterItem = new BankAccountRegisterItem();
            bankRegisterItem.BankAccountId = gridRow.Manager.ViewModel.BankViewModel.Id;
            bankRegisterItem.ItemDate = gridRow.Date;
            bankRegisterItem.ItemType = (int) budgetItem.Type;
            bankRegisterItem.BudgetItemId = budgetItem.Id;
            bankRegisterItem.BudgetItem = budgetItem;
            //bankRegisterItem.Description = $"Increase {budgetItem.Description} ";
            bankRegisterItem.Description = gridRow.Description;


            bankRegisterItem.ItemType = (int) BankAccountRegisterItemTypes.Miscellaneous;
            bankRegisterItem.Completed = true;
            bankRegisterItem.ProjectedAmount = amount;
            bankRegisterItem.ActualAmount = amount;
            bankRegisterItem.BankText = gridRow.Description;
            BankAccountRegisterItem transferToRegisterItem = null;
            var itemToAdd = bankRegisterItem;
            switch ((BudgetItemTypes)budgetItem.Type)
            {
                case BudgetItemTypes.Income:
                    //bankRegisterItem.Description += "Income";
                    break;
                case BudgetItemTypes.Expense:
                    //bankRegisterItem.Description += "Expense";
                    bankRegisterItem.ProjectedAmount = -amount;
                    bankRegisterItem.ActualAmount = amount;
                    break;
                case BudgetItemTypes.Transfer:
                    bankRegisterItem.Description = $"Transfer To {budgetItem.TransferToBankAccount.Description}";
                    bankRegisterItem.RegisterGuid = Guid.NewGuid().ToString();
                    bankRegisterItem.ItemType = (int) BankAccountRegisterItemTypes.TransferToBankAccount;
                    transferToRegisterItem = new BankAccountRegisterItem();

                    transferToRegisterItem.ProjectedAmount = gridRow.Amount;
                    transferToRegisterItem.BudgetItem = budgetItem;
                    transferToRegisterItem.BudgetItemId = budgetItem.Id;
                    transferToRegisterItem.RegisterGuid = Guid.NewGuid().ToString();
                    transferToRegisterItem.TransferRegisterGuid = bankRegisterItem.RegisterGuid;
                    transferToRegisterItem.ItemDate = gridRow.Date;
                    transferToRegisterItem.Description = $"Transfer From {budgetItem.BankAccount.Description}";
                    transferToRegisterItem.ItemType = (int) BankAccountRegisterItemTypes.TransferToBankAccount;
                    transferToRegisterItem.BankText = gridRow.Description;
                    bankRegisterItem.ProjectedAmount = -amount;
                    bankRegisterItem.TransferRegisterGuid = transferToRegisterItem.RegisterGuid;
                    switch (gridRow.TransactionTypes)
                    {
                        case TransactionTypes.Deposit:
                            if (budgetItem.TransferToBankAccountId != null
                                && budgetItem.TransferToBankAccountId.Value == ViewModel.BankViewModel.Id)
                            {
                                transferToRegisterItem.BankAccountId = budgetItem.TransferToBankAccountId.Value;
                                itemToAdd = transferToRegisterItem;
                                transferToRegisterItem.ProjectedAmount = amount;
                                transferToRegisterItem.Completed = true;
                                bankRegisterItem.BankAccountId = budgetItem.BankAccountId;
                                bankRegisterItem.Completed = false;
                            }
                            break;
                        case TransactionTypes.Withdrawal:
                            if (budgetItem.TransferToBankAccountId != null)
                                transferToRegisterItem.BankAccountId = budgetItem.TransferToBankAccountId.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!AppGlobals.DataRepository.SaveNewRegisterItem(bankRegisterItem, transferToRegisterItem))
                return null;

            ViewModel.BankViewModel.RegisterGridManager.AddGeneratedRegisterItems(new List<BankAccountRegisterItem>()
            {
                itemToAdd
            });
            var newRow = ViewModel.BankViewModel.RegisterGridManager.Rows.OfType<BankAccountRegisterGridRow>()
                .FirstOrDefault(p => p.RegisterId == itemToAdd.Id);

            if (gridRow.SourceAutoFillValue != null && gridRow.SourceAutoFillValue.IsValid())
            {
                newRow.ActualAmountDetails.Add(new BankAccountRegisterItemAmountDetail()
                {
                    Amount = gridRow.Amount,
                    BankText = gridRow.Description,
                    Date = gridRow.Date,
                    DetailId = 1,
                    RegisterId = itemToAdd.Id,
                    SourceId = gridRow.SourceAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt(),
                });
                bankRegisterItem.Description += $" {budgetItem.Description} ";
                if (!UpdateRegisterRow(0, newRow, bankRegisterItem, gridRow)) return null;
            }
            return newRow;
        }

        public void LoadGrid()
        {
            var rowIndex = 0;
            var transactions = AppGlobals.DataRepository.GetBankTransactions(ViewModel.BankViewModel.Id);
            foreach (var bankTransaction in transactions)
            {
                rowIndex++;
                var bankRow = GetNewRow() as ImportTransactionGridRow;
                bankRow.FromBank = bankTransaction.FromBank;
                bankRow.QifMap = bankTransaction.QifMap;
                bankRow.Date = bankTransaction.TransactionDate;
                bankRow.Description = bankTransaction.Description;
                bankRow.MapTransaction = bankTransaction.MapTransaction;
                bankRow.TransactionTypes = (TransactionTypes) bankTransaction.TransactionType;
                bankRow.FromBank = bankTransaction.FromBank;
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
                if (!importTransactionGridRow.Description.IsNullOrEmpty())
                {
                    var filter = new TableFilterDefinition<QifMap>(AppGlobals.LookupContext.QifMaps);
                    var foundMap = false;
                    var spacePos = importTransactionGridRow.Description.IndexOf(" ");
                    var text = importTransactionGridRow.Description;
                    if (spacePos > 0)
                        text = importTransactionGridRow.Description.MidStr(0, spacePos).Trim();
                    while (foundMap == false)
                    {
                        var checkResult = CheckQuery(filter, text, importTransactionGridRow, out var newFoundMap);
                        foundMap = newFoundMap;
                        if (!checkResult)
                        {
                            break;
                        }
                        spacePos = importTransactionGridRow.Description.IndexOf(" ", spacePos + 1);
                        if (spacePos == -1)
                        {
                            checkResult = CheckQuery(filter, importTransactionGridRow.Description, importTransactionGridRow, out var newCheckFoundMap);
                            if (!checkResult)
                            {
                                break;
                            }
                            foundMap = true;
                        }
                        else
                        {
                            var nextSpacePos = importTransactionGridRow.Description.IndexOf(" ", spacePos + 1);
                            text = importTransactionGridRow.Description.MidStr(0, spacePos);
                            text = text.Trim();
                        }

                    }
                }

                AddRow(importTransactionGridRow);
            }
            PostLoadGridFromEntity();
            Grid?.SetBulkInsertMode(false);
            Grid?.RefreshGridView();
        }

        private bool CheckQuery(TableFilterDefinition<QifMap> filter, string text, ImportTransactionGridRow importTransactionGridRow,
            out bool foundMap)
        {
            ProcessQifQuery(filter, text);

            var param = GblMethods.GetParameterExpression<QifMap>();
            var filterExpr = filter.GetWhereExpresssion<QifMap>(param);
            var query = AppGlobals
                .LookupContext
                .GetQueryable<QifMap>(AppGlobals
                    .LookupContext
                    .QifMapLookup);
            query = FilterItemDefinition.FilterQuery(query, param, filterExpr);
            foundMap = false;
            //if (result.ResultCode == GetDataResultCodes.Success)
            {
                if (query.Count() == 1)
                {
                    foundMap = true;
                    ProcessFoundQif(query.FirstOrDefault(), importTransactionGridRow);
                }
                else if (query.Count() == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static void ProcessQifQuery(TableFilterDefinition<QifMap> filter, string text)
        {
            if (filter.FixedFilters.Any())
            {
                filter
                    .RemoveFixedFilter(filter.FixedFilters[0]);
            }

            filter.AddFixedFieldFilter(AppGlobals.LookupContext.QifMaps.GetFieldDefinition(p => p.BankText),
                Conditions.Contains, text);
        }

        private static void ProcessFoundQif(QifMap qifMap, ImportTransactionGridRow importTransactionGridRow)
        {
            qifMap = AppGlobals.DataRepository.GetQifMap(qifMap.Id);

            importTransactionGridRow.MapTransaction = false;
            importTransactionGridRow.QifMap = qifMap;
            ProcessFoundBudgetItem(qifMap, importTransactionGridRow);

            ProcessFoundSource(qifMap, importTransactionGridRow);
        }

        private static void ProcessFoundSource(QifMap qifMap, ImportTransactionGridRow importTransactionGridRow)
        {
            if (qifMap.Source != null)
            {
                importTransactionGridRow.SourceAutoFillValue =
                    AppGlobals.LookupContext.OnAutoFillTextRequest(
                        AppGlobals.LookupContext.BudgetItemSources,
                        qifMap.SourceId.ToString());
            }
        }

        private static void ProcessFoundBudgetItem(QifMap qifMap, ImportTransactionGridRow importTransactionGridRow)
        {
            if (qifMap.BudgetItem != null)
            {
                importTransactionGridRow.BudgetItemAutoFillValue =
                    AppGlobals.LookupContext.OnAutoFillTextRequest(
                        AppGlobals.LookupContext.BudgetItems,
                        qifMap.BudgetId.ToString());
            }
        }

        public void SetMapRowsBudget(ImportTransactionGridRow row)
        {
            var refresh = false;

            var rowsToSet =  GetMaps(row);

            if (rowsToSet.Any())
            {
                refresh = true;
                foreach (var importTransactionGridRow in rowsToSet)
                {
                    if (importTransactionGridRow != row
                        && !importTransactionGridRow.BudgetItemAutoFillValue.IsValid())
                    {
                        importTransactionGridRow.BudgetItemAutoFillValue = row.BudgetItemAutoFillValue;
                        importTransactionGridRow.MapTransaction = false;
                    }
                }
            }
            if (refresh)
            {
                Grid?.RefreshGridView();
            }
        }

        private List<ImportTransactionGridRow> GetMaps(ImportTransactionGridRow row)
        {
            var result = new List<ImportTransactionGridRow>();

            var foundMap = false;
            if (row.Description.IsNullOrEmpty())
            {
                return result;
            }
            var spacePos = row.Description.IndexOf(" ");
            var text = row.Description;
            if (spacePos > 0)
                text = row.Description.MidStr(0, spacePos).Trim();
            while (foundMap == false)
            {
                var foundRows = GetRowsContainingText(text);
                if (foundRows.Any())
                {
                    result.AddRange(foundRows);
                    foundMap = true;
                }
                else
                {
                    spacePos = row.Description.IndexOf(" ", spacePos + 1);
                    if (spacePos == -1)
                    {
                        foundMap = true;
                    }
                    else
                    {
                        var nextSpacePos = row.Description.IndexOf(" ", spacePos + 1);
                        text = row.Description.MidStr(0, spacePos);
                        text = text.Trim();
                    }
                }
            }

            return result;
        }

        public List<ImportTransactionGridRow> GetRowsContainingText(string text)
        {
            var rows = Rows.OfType<ImportTransactionGridRow>()
                .Where(p => p.FromBank 
                            && p.Description.Contains(text));

            var result = rows.ToList();
            return result;
        }

        public void SetMapRowsSource(ImportTransactionGridRow row)
        {
            var refresh = false;

            var rowsToSet = GetMaps(row);

            if (rowsToSet.Any())
            {
                refresh = true;
                foreach (var importTransactionGridRow in rowsToSet)
                {
                    if (importTransactionGridRow != row && !importTransactionGridRow.SourceAutoFillValue.IsValid())
                    {
                        importTransactionGridRow.SourceAutoFillValue = row.SourceAutoFillValue;
                    }
                }
            }
            if (refresh)
            {
                Grid?.RefreshGridView();
            }
        }
    }
}
