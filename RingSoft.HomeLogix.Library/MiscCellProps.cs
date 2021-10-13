using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library
{
    public class MiscCellProps : DataEntryGridTextCellProps
    {
        public const int MiscControlHostId = 103;

        public override int EditingControlId => MiscControlHostId;

        public new BankAccountRegisterGridRow Row { get; }

        public MiscCellProps(BankAccountRegisterGridRow row, int columnId, string text) : base(row, columnId, text)
        {
            Row = row;
        }
    }
}
