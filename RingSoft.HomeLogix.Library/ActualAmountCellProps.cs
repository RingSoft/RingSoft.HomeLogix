using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.HomeLogix.Library
{
    public class ActualAmountCellProps : DataEntryGridDecimalCellProps
    {
        public const int ActualAmountControlHostId = 101;

        public override int EditingControlId => ActualAmountControlHostId;

        public ActualAmountCellProps(DataEntryGridRow row, int columnId, DecimalEditControlSetup setup, decimal? value) : base(row, columnId, setup, value)
        {
        }
    }
}
