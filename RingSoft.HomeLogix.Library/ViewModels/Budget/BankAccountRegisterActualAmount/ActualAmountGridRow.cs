using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;

// ReSharper disable once CheckNamespace
namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class ActualAmountGridRow : DataEntryGridRow
    {
        public new BankAccountRegisterActualAmountGridManager Manager { get; }

        public DateTime Date { get; private set; } = DateTime.Today;

        public AutoFillValue Store { get; private set; }

        public decimal Amount { get; private set; }

        private DateEditControlSetup _dateSetup;
        private AutoFillSetup _storeAutoFillSetup;
        private DecimalEditControlSetup _amountSetup;

        public ActualAmountGridRow(BankAccountRegisterActualAmountGridManager manager) : base(manager)
        {
            Manager = manager;
            _dateSetup = new DateEditControlSetup {DateFormatType = DateFormatTypes.DateOnly};

            _storeAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.StoresLookup);

            _amountSetup = new DecimalEditControlSetup
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
                    return new DataEntryGridDateCellProps(this, columnId, _dateSetup, Date);
                case ActualAmountGridColumns.Store:
                    return new DataEntryGridAutoFillCellProps(this, columnId, _storeAutoFillSetup, Store);
                case ActualAmountGridColumns.Amount:
                    return new DataEntryGridDecimalCellProps(this, columnId, _amountSetup, Amount);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ActualAmountGridColumns) value.ColumnId;
            switch (column)
            {
                case ActualAmountGridColumns.Date:
                    if (value is DataEntryGridDateCellProps dateCellProps)
                        Date = dateCellProps.Value.GetValueOrDefault(DateTime.Today);
                    break;
                case ActualAmountGridColumns.Store:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        if (!autoFillCellProps.AutoFillValue.IsValid())
                        {
                            var store = new Store {Name = autoFillCellProps.AutoFillValue.Text};
                            if (AppGlobals.DataRepository.SaveStore(store))
                            {
                                Store = new AutoFillValue(
                                    AppGlobals.LookupContext.Stores.GetPrimaryKeyValueFromEntity(store), store.Name);
                            }
                        }
                        else 
                            Store = autoFillCellProps.AutoFillValue;
                    }

                    break;
                case ActualAmountGridColumns.Amount:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        Amount = decimalCellProps.Value.GetValueOrDefault(0);
                        Manager.ViewModel.CalculateTotals();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }
    }
}
