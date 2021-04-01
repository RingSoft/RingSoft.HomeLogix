using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library
{
    public class ActualAmountCellProps : DataEntryGridDecimalCellProps
    {
        public const int ActualAmountControlHostId = 101;

        public override int EditingControlId => ActualAmountControlHostId;

        public BankAccountRegisterGridRow RegisterGridRow { get; }

        public ActualAmountCellProps(BankAccountRegisterGridRow row, int columnId, DecimalEditControlSetup setup, decimal? value) : base(row, columnId, setup, value)
        {
            RegisterGridRow = row;
        }
    }
}
