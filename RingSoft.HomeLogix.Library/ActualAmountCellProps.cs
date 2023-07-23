using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.Library.ViewModels.ImportBank;

namespace RingSoft.HomeLogix.Library
{
    public class ActualAmountCellProps : DataEntryGridDecimalCellProps
    {
        public const int ActualAmountControlHostId = 101;

        public override int EditingControlId => ActualAmountControlHostId;

        public BankAccountRegisterGridRow RegisterGridRow { get; set; }

        public ImportTransactionGridRow ImportTransactionGridRow { get; set; }

        public ActualAmountCellProps(DataEntryGridRow row, int columnId, 
            DecimalEditControlSetup setup, double? value) : base(row, columnId, setup, (decimal)value)
        {
            RegisterGridRow = row as BankAccountRegisterGridRow;
            ImportTransactionGridRow = row as ImportTransactionGridRow;
        }
    }
}
