using RingSoft.DataEntryControls.Engine.DataEntryGrid;

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

        public new ImportTransactionsGridManager Manager { get; set; }

        public ImportTransactionGridRow(ImportTransactionsGridManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId);
        }
    }
}
