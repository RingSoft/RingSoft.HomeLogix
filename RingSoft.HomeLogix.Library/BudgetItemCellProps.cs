using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library
{
    public class BudgetItemCellProps : DataEntryGridTextCellProps
    {
        public const int BudgetItemControlHostId = 102;

        public override int EditingControlId => BudgetItemControlHostId;

        public new BankAccountRegisterGridBudgetItemRow Row { get; private set; }

        public BudgetItemCellProps(BankAccountRegisterGridBudgetItemRow row, int columnId) : base(row, columnId)
        {
            Row = row;
        }

        public BudgetItemCellProps(BankAccountRegisterGridBudgetItemRow row, int columnId, string text) : base(row, columnId, text)
        {
            Row = row;
        }
    }
}
