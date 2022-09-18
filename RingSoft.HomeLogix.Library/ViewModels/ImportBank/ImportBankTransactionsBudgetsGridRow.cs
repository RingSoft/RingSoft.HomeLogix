using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public enum ImportBudgetsColumn
    {
        BudgetItem = 0,
        Amount = 1,
    }
    public class ImportBankTransactionsBudgetsGridRow : DataEntryGridRow
    {
        public const int BudgetColumnId = (int)ImportBudgetsColumn.BudgetItem;
        public const int AmountColumnId = (int)ImportBudgetsColumn.Amount;

        public ImportBankTransactionsBudgetManager Manager { get; set; }
        public ImportBankTransactionsBudgetsGridRow(ImportBankTransactionsBudgetManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId);
        }
    }
}
