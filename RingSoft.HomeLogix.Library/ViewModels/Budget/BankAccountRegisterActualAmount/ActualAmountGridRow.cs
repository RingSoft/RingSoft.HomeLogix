using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;

// ReSharper disable once CheckNamespace
namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class ActualAmountGridRow : DataEntryGridRow
    {
        public new BankAccountRegisterActualAmountGridManager Manager { get; }

        public DateTime Date { get; private set; } = DateTime.Today;

        public AutoFillValue Store { get; private set; }

        public decimal Amount { get; private set; }

        private DateEditControlSetup _dateEditControlSetup;
        private AutoFillSetup _storeAutoFillSetup;
        private DecimalEditControlSetup _decimalEditControlSetup;

        public ActualAmountGridRow(BankAccountRegisterActualAmountGridManager manager) : base(manager)
        {
            Manager = manager;
            _dateEditControlSetup = new DateEditControlSetup {DateFormatType = DateFormatTypes.DateOnly};

            _decimalEditControlSetup = new DecimalEditControlSetup
            {
                FormatType = DecimalEditFormatTypes.Currency,
                AllowNullValue = false
            };
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ActualAmountGridColumns) columnId;

            switch (column)
            {
                case ActualAmountGridColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId, _dateEditControlSetup, Date);
                case ActualAmountGridColumns.Store:
                    break;
                case ActualAmountGridColumns.Amount:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new DataEntryGridTextCellProps(this, columnId);
        }
    }
}
