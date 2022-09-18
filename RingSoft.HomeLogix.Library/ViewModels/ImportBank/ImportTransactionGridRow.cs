using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public enum ImportColumns
    {
        Date = 0,
        BankText = 1,
        BudgetItem = 2,
        Source = 3,
        Amount = 4,
        Map = 5
    }
    public class ImportTransactionGridRow : DataEntryGridRow
    {
        public const int DateColumnId = (int) ImportColumns.Date;
        public const int BankTextColumnId = (int) ImportColumns.BankText;
        public const int BudgetItemColumnId = (int) ImportColumns.BudgetItem;
        public const int SourceColumnId = (int) ImportColumns.Source;
        public const int AmountColumnId = (int) ImportColumns.Amount;
        public const int MapColumnId = (int) ImportColumns.Map;

        public DateTime Date { get; set; } = DateTime.Today;
        public string BankText { get; set; }
        public AutoFillSetup BudgetItemAutoFillSetup { get; set; }
        public AutoFillValue BudgetItemAutoFillValue { get; set; }
        public AutoFillSetup SourceAutoFillSetup { get; set; }
        public AutoFillValue SourceAutoFillValue { get; set; }
        public decimal Amount { get; set; }
        public bool MapTransaction { get; set; }

        public new ImportTransactionsGridManager Manager { get; set; }

        public ImportTransactionGridRow(ImportTransactionsGridManager manager) : base(manager)
        {
            Manager = manager;
            BudgetItemAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.BankTransactions.GetFieldDefinition(p => p.BudgetId));
            SourceAutoFillSetup =
                new AutoFillSetup(AppGlobals.LookupContext.BankTransactions.GetFieldDefinition(p => p.SourceId));
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ImportColumns) columnId;

            switch (column)
            {
                case ImportColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId,
                        new DateEditControlSetup() {DateFormatType = DateFormatTypes.DateOnly}, Date);
                case ImportColumns.BankText:
                    return new DataEntryGridTextCellProps(this, columnId);
                case ImportColumns.BudgetItem:
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
                case ImportColumns.Date:
                case ImportColumns.BudgetItem:
                case ImportColumns.Source:
                case ImportColumns.Amount:
                    break;
                case ImportColumns.BankText:
                    return new DataEntryGridControlCellStyle() {State = DataEntryGridCellStates.Disabled};
                case ImportColumns.Map:
                    if (BankText.IsNullOrEmpty())
                    {
                        return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }
    }
}
