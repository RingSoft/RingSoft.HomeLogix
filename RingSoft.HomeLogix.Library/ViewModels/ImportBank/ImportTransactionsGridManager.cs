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
using RingSoft.App.Library;
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
                        if (row.RegisterItemAutoFillValue != null && row.RegisterItemAutoFillValue.IsValid())
                        {
                            bankTransaction.RegisterId =
                                row.RegisterItemAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt();
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
                                RegisterItemId =
                                    rowBudgetItemSplit.RegisterItemAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt(),
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
                    if (row.RegisterItemAutoFillValue == null || !row.RegisterItemAutoFillValue.IsValid())
                    {
                        var message = "You must select a register item for this transaction.";
                        var caption = "Invalid Register Item";
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption,
                            RsMessageBoxIcons.Exclamation);
                        Grid.GotoCell(row, ImportTransactionGridRow.RegisterItemColumnId);
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

        public bool PostTransactions(ISplashWindow splashWindow)
        {
            var bankBalance = ViewModel.BankViewModel.CurrentBalance;
            var rows = Rows.OfType<ImportTransactionGridRow>().Where(p => p.IsNew == false);

            var count = rows.Count();
            var rowIndex = 0;
            
            rowIndex = 0;
            foreach (var gridRow in rows)
            {
                rowIndex++;
                splashWindow.SetProgress($"Posting Row {rowIndex} of {count}");
                BankAccountRegisterItem registerItem = null;
                BudgetItem budgetItem = null;
                if (gridRow.BudgetItemSplits.Any())
                {
                    foreach (var gridRowBudgetItemSplit in gridRow.BudgetItemSplits)
                    {
                        registerItem = gridRowBudgetItemSplit.RegisterItemAutoFillValue
                            .GetEntity<BankAccountRegisterItem>();
                        registerItem = registerItem.FillOutProperties(true);
                        budgetItem = registerItem.BudgetItem;
                        var amount = ProcessAmount(gridRowBudgetItemSplit.Amount, gridRow, budgetItem);
                        var row = PostRegisterItem(registerItem, gridRowBudgetItemSplit.Amount, gridRow, true);
                        if (row != null)
                            bankBalance = UpdateBankBalance(row, bankBalance, amount);
                    }
                }
                else
                {
                    registerItem = gridRow.RegisterItemAutoFillValue.GetEntity<BankAccountRegisterItem>();
                    registerItem = registerItem.FillOutProperties(true);
                    budgetItem = registerItem.BudgetItem;
                    var amount = ProcessAmount(gridRow.Amount, gridRow, budgetItem);
                    var row = PostRegisterItem(registerItem, amount, gridRow, false);
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

            result = Math.Round(result, 2);
            return result;
        }

        private void SaveQifMaps()
        {
            foreach (var gridRow in Rows.OfType<ImportTransactionGridRow>().Where(p => p.IsNew == false && p.MapTransaction == true && !p.Description.IsNullOrEmpty()))
            {
                if (gridRow.RegisterItemAutoFillValue != null && gridRow.RegisterItemAutoFillValue.IsValid())
                {
                    var qifMap = AppGlobals.DataRepository.GetQifMap(gridRow.Description);
                    if (qifMap == null)
                    {
                        var registerItem = gridRow.RegisterItemAutoFillValue.GetEntity<BankAccountRegisterItem>();
                        registerItem = registerItem.FillOutProperties(true);
                        qifMap = new QifMap();
                        qifMap.BankText = gridRow.Description;
                        qifMap.BudgetId = registerItem.BudgetItemId.GetValueOrDefault();
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
            amount = Math.Abs(amount);
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

            bankBalance = Math.Round(bankBalance, 2);
            return bankBalance;
        }

        private BankAccountRegisterGridRow PostRegisterItem(BankAccountRegisterItem registerItem, double amount, ImportTransactionGridRow gridRow, bool isSplit)
        {
            var registerRow = gridRow
                .Manager
                .ViewModel
                .BankViewModel
                .RegisterGridManager
                .Rows.OfType<BankAccountRegisterGridRow>()
                .FirstOrDefault(p => p.RegisterId == registerItem.Id);

            if (registerRow != null)
            {
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
            return registerRow;
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
            registerRow.ItemDate = importTransactionGridRow.Date;

            registerRow.SaveToEntity(registerItem, 0, registerRow.ActualAmountDetails.ToList());
            if (!AppGlobals.DataRepository.SaveRegisterItem(registerItem, registerRow.ActualAmountDetails))
                return false;
            registerRow.Manager.Grid?.UpdateRow(registerRow);
            return true;
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
                if (bankTransaction.RegisterItem != null)
                {
                    bankRow.RegisterItemAutoFillValue = bankTransaction.RegisterItem.GetAutoFillValue();
                    bankRow.RegisterDate = bankTransaction.RegisterItem.ItemDate;
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
                        RegisterItemAutoFillValue = bankTransactionBudgetItem.RegisterItem.GetAutoFillValue(),
                        RegisterDate = bankTransactionBudgetItem.RegisterItem.ItemDate,
                        Amount = bankTransactionBudgetItem.Amount,
                    });
                }

                AddRow(bankRow);
            }
            Grid?.RefreshGridView();
        }

        public void ImportFromQif(List<ImportTransactionGridRow> rows, ISplashWindow splashWindow)
        {
            var existingRows = Rows.OfType<ImportTransactionGridRow>().Where(p => !p.IsNew).ToList();
            rows.AddRange(existingRows);
            rows = rows.OrderBy(p => p.Date).ToList();
            Grid?.SetBulkInsertMode(true);
            PreLoadGridFromEntity();
            var rowIndex = 0;
            var count = rows.Count;
            var context = SystemGlobals.DataRepository.GetDataContext();
            var bankRegisterTable = context.GetTable<BankAccountRegisterItem>();
            foreach (var importTransactionGridRow in rows)
            {
                rowIndex++;
                splashWindow.SetProgress($"Loading Row {rowIndex} of {count}");
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
                        var checkResult = CheckQuery(filter
                            , text
                            , importTransactionGridRow
                            , out var newFoundMap
                            , bankRegisterTable);
                        foundMap = newFoundMap;
                        if (!checkResult)
                        {
                            break;
                        }
                        spacePos = importTransactionGridRow.Description.IndexOf(" ", spacePos + 1);
                        if (spacePos == -1)
                        {
                            checkResult = CheckQuery(filter
                                , importTransactionGridRow.Description
                                , importTransactionGridRow
                                , out var newCheckFoundMap
                                , bankRegisterTable);
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
            out bool foundMap
            , IQueryable<BankAccountRegisterItem> bankRegisterTable)
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
                    ProcessFoundQif(query.FirstOrDefault(), importTransactionGridRow, bankRegisterTable);
                }
                else if (query.Count() == 0)
                {
                    if (!importTransactionGridRow.RegisterItemAutoFillValue.IsValid())
                    {
                        importTransactionGridRow.MapTransaction = true;
                    }

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

        private static void ProcessFoundQif(QifMap qifMap, ImportTransactionGridRow importTransactionGridRow
            , IQueryable<BankAccountRegisterItem> bankRegisterTable)
        {
            qifMap = AppGlobals.DataRepository.GetQifMap(qifMap.Id);

            importTransactionGridRow.MapTransaction = false;
            importTransactionGridRow.QifMap = qifMap;
            ProcessFoundBudgetItem(qifMap.BudgetItem, importTransactionGridRow, bankRegisterTable);

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

        private static void ProcessFoundBudgetItem(BudgetItem budgetItem, ImportTransactionGridRow importTransactionGridRow
        , IQueryable<BankAccountRegisterItem> bankRegisterTable)
        {
            if (budgetItem != null)
            {
                var importDate = importTransactionGridRow.Date;
                var daysToSplit = 0.0;
                switch ((BudgetItemRecurringTypes)budgetItem.RecurringType)
                {
                    case BudgetItemRecurringTypes.Days:
                        daysToSplit =
                            Math.Floor((double)budgetItem.RecurringPeriod / 2);
                        break;
                    case BudgetItemRecurringTypes.Weeks:
                        daysToSplit =
                            Math.Floor((double)(budgetItem.RecurringPeriod * 7) / 2);
                        break;
                    case BudgetItemRecurringTypes.Months:
                    case BudgetItemRecurringTypes.Years:
                        daysToSplit = 15;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (daysToSplit > 15)
                {
                    daysToSplit = 15;
                }
                var startDate = importDate.AddDays(-daysToSplit);
                var endDate = importDate.AddDays(daysToSplit);
                var registerItemFound = bankRegisterTable
                    .FirstOrDefault(p => p.BudgetItemId == budgetItem.Id
                                         && p.ItemDate >= startDate
                                         && p.ItemDate <= endDate);
                if (registerItemFound != null)
                {
                    importTransactionGridRow.RegisterItemAutoFillValue = registerItemFound.GetAutoFillValue();
                    importTransactionGridRow.RegisterDate = registerItemFound.ItemDate;
                }
            }
        }

        public void SetMapRowsBudget(ImportTransactionGridRow row)
        {
            var refresh = false;

            var rowsToSet =  GetMaps(row);

            if (rowsToSet.Any())
            {
                refresh = true;
                var registerItem = row.RegisterItemAutoFillValue.GetEntity<BankAccountRegisterItem>();
                BudgetItem budgetItem = null;
                if (registerItem != null)
                {
                    registerItem = registerItem.FillOutProperties(true);
                    budgetItem = registerItem.BudgetItem;
                }

                var context = SystemGlobals.DataRepository.GetDataContext();
                var registerTable = context.GetTable<BankAccountRegisterItem>();
                if (budgetItem != null)
                {
                    foreach (var importTransactionGridRow in rowsToSet)
                    {
                        if (importTransactionGridRow != row
                            && !importTransactionGridRow.RegisterItemAutoFillValue.IsValid())
                        {
                            ProcessFoundBudgetItem(budgetItem, importTransactionGridRow, registerTable);
                            importTransactionGridRow.MapTransaction = false;
                        }
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
