using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public enum ImportColumns
    {
        Date = 0,
        Description = 1,
        TransactionType = 2,
        BudgetItem = 3,
        Source = 4,
        Amount = 5,
        Map = 6
    }

    public class BudgetSplit
    {
        public AutoFillValue BudgetItem { get; set; }
        public double Amount { get; set; }
    }
    public class ImportTransactionGridRow : DataEntryGridRow
    {
        public const int DateColumnId = (int) ImportColumns.Date;
        public const int DescriptionColumnId = (int) ImportColumns.Description;
        public const int TransactionTypeColumnId = (int) ImportColumns.TransactionType;
        public const int BudgetItemColumnId = (int) ImportColumns.BudgetItem;
        public const int SourceColumnId = (int) ImportColumns.Source;
        public const int AmountColumnId = (int) ImportColumns.Amount;
        public const int MapColumnId = (int) ImportColumns.Map;

        public DateTime Date { get; set; } = DateTime.Today;
        public string Description { get; set; }

        public TransactionTypes TransactionTypes
        {
            get => (Budget.TransactionTypes) TransactionTypeItem.NumericValue;
            set => TransactionTypeItem = TransactionTypeComboBoxControlSetup.GetItem((int) value);
        }
        public TextComboBoxControlSetup TransactionTypeComboBoxControlSetup { get; set; }
        public TextComboBoxItem TransactionTypeItem { get; set; }
        public AutoFillSetup BudgetItemAutoFillSetup { get; set; }
        public AutoFillValue BudgetItemAutoFillValue { get; set; }
        public AutoFillSetup SourceAutoFillSetup { get; set; }
        public AutoFillValue SourceAutoFillValue { get; set; }
        public double Amount { get; set; }
        public bool MapTransaction { get; set; }
        public List<BudgetSplit> BudgetItemSplits { get; set; }

        public new ImportTransactionsGridManager Manager { get; set; }

        public QifMap QifMap { get; set; }

        public bool FromBank { get; set; }

        public ImportTransactionGridRow(ImportTransactionsGridManager manager) : base(manager)
        {
            Manager = manager;
            BudgetItemSplits = new List<BudgetSplit>();
            TransactionTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            TransactionTypeComboBoxControlSetup.LoadFromEnum<TransactionTypes>();
            TransactionTypes = TransactionTypes.Withdrawal;
            BudgetItemAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.BudgetItemsLookup.Clone());
            SourceAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.BankTransactions.GetFieldDefinition(p => p.SourceId));
        }

        public override string ToString()
        {
            return Description;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ImportColumns) columnId;

            switch (column)
            {
                case ImportColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId,
                        new DateEditControlSetup() {DateFormatType = DateFormatTypes.DateOnly}, Date);
                case ImportColumns.Description:
                    return new DataEntryGridTextCellProps(this, columnId, Description);
                case ImportColumns.TransactionType:
                    return new DataEntryGridCustomControlCellProps(this, columnId, (int)TransactionTypes);
                case ImportColumns.BudgetItem:
                    BudgetItemAutoFillSetup.LookupDefinition.FilterDefinition.ClearFixedFilters();
                    if (TransactionTypes == TransactionTypes.Withdrawal)
                    {
                        BudgetItemAutoFillSetup.LookupDefinition.FilterDefinition
                            .AddFixedFilter(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => (int) p.Type),
                                Conditions.Equals, (int) BudgetItemTypes.Expense).SetEndLogic(EndLogics.Or)
                            .SetLeftParenthesesCount(1);
                    }
                    if (TransactionTypes == TransactionTypes.Deposit)
                    {
                        BudgetItemAutoFillSetup.LookupDefinition.FilterDefinition
                            .AddFixedFilter(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => (int)p.Type),
                                Conditions.Equals, (int)BudgetItemTypes.Income).SetEndLogic(EndLogics.Or).SetLeftParenthesesCount(1);
                    }
                    BudgetItemAutoFillSetup.LookupDefinition.FilterDefinition
                        .AddFixedFilter(AppGlobals.LookupContext.BudgetItems.GetFieldDefinition(p => (int)p.Type),
                            Conditions.Equals, (int)BudgetItemTypes.Transfer).SetRightParenthesesCount(1);

                    return new DataEntryGridAutoFillCellProps(this, columnId, BudgetItemAutoFillSetup,
                        BudgetItemAutoFillValue);
                case ImportColumns.Source:
                    return new DataEntryGridAutoFillCellProps(this, columnId, SourceAutoFillSetup,
                        SourceAutoFillValue);
                case ImportColumns.Amount:
                    return new ActualAmountCellProps(this, columnId,
                        new DecimalEditControlSetup {FormatType = DecimalEditFormatTypes.Currency}, Amount);
                case ImportColumns.Map:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, MapTransaction);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ImportColumns)columnId;
            switch (column)
            {
                case ImportColumns.BudgetItem:
                    if (BudgetItemSplits.Any())
                    {
                        return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                    }
                    return new DataEntryGridControlCellStyle();
                case ImportColumns.Source:
                case ImportColumns.Amount:
                    return new DataEntryGridControlCellStyle();
                case ImportColumns.Description:
                    var state = DataEntryGridCellStates.Enabled;
                    if (FromBank)
                    {
                        state |= DataEntryGridCellStates.Disabled;
                    }
                    return new DataEntryGridControlCellStyle() {State = state};
                case ImportColumns.Map:
                    if (!FromBank)
                    {
                        return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                    }
                    return new DataEntryGridControlCellStyle();
                case ImportColumns.Date:
                case ImportColumns.TransactionType:
                    if (FromBank)
                    {
                        return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                    }
                    return new DataEntryGridControlCellStyle();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ImportColumns)value.ColumnId;

            switch (column)
            {
                case ImportColumns.Date:
                    var dateProps = value as DataEntryGridDateCellProps;
                    if (dateProps != null)
                    {
                        if (dateProps.Value != null) 
                            Date = dateProps.Value.Value;
                    }
                    break;
                case ImportColumns.Description:
                    var textProps = value as DataEntryGridTextCellProps;
                    if (textProps != null) Description = textProps.Text;
                    break;
                case ImportColumns.TransactionType:
                    var comboProps = value as DataEntryGridCustomControlCellProps;
                    TransactionTypes = (TransactionTypes) comboProps.SelectedItemId;
                    break;
                case ImportColumns.BudgetItem:
                    var budgetAutoFillCellProps = value as DataEntryGridAutoFillCellProps;
                    if (budgetAutoFillCellProps != null && budgetAutoFillCellProps.AutoFillValue.IsValid())
                    {
                        BudgetItemAutoFillValue = budgetAutoFillCellProps.AutoFillValue;
                        Manager.SetMapRowsBudget(this);
                    }

                    break;
                case ImportColumns.Source:
                    var autoFillCellProps = value as DataEntryGridAutoFillCellProps;
                    if (autoFillCellProps != null)
                    {
                        SourceAutoFillValue = autoFillCellProps.AutoFillValue;
                        Manager.SetMapRowsSource(this);
                    }
                    break;
                case ImportColumns.Amount:
                    var decimalProps = value as ActualAmountCellProps;
                    if (decimalProps != null)
                    {
                        if (decimalProps.Value != null) Amount = (double)decimalProps.Value.Value;
                    }
                    break;
                case ImportColumns.Map:
                    var checkBoxProps = value as DataEntryGridCheckBoxCellProps;
                    MapTransaction = checkBoxProps.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }
    }
}
