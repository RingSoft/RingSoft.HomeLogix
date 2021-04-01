using RingSoft.DataEntryControls.Engine.DataEntryGrid;

// ReSharper disable once CheckNamespace
namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class ActualAmountGridRow : DataEntryGridRow
    {
        private new BankAccountRegisterActualAmountGridManager Manager { get; }
        public ActualAmountGridRow(BankAccountRegisterActualAmountGridManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            return new DataEntryGridTextCellProps(this, columnId);
        }
    }
}
