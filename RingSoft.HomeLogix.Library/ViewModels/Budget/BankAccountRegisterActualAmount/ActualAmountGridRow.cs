using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using RingSoft.DbLookup.QueryBuilder;

// ReSharper disable once CheckNamespace
namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class ActualAmountGridRow : DataEntryGridRow
    {
        public new BankAccountRegisterActualAmountGridManager Manager { get; }

        public DateTime Date { get; set; } = DateTime.Today;

        public AutoFillValue Source { get; set; }

        public decimal Amount { get; set; }

        public bool IsIncome { get; set; }

        private DateEditControlSetup _dateSetup;
        private AutoFillSetup _sourceAutoFillSetup;
        private DecimalEditControlSetup _amountSetup;

        public ActualAmountGridRow(BankAccountRegisterActualAmountGridManager manager) : base(manager)
        {
            Manager = manager;
            _dateSetup = new DateEditControlSetup {DateFormatType = DateFormatTypes.DateOnly};

            switch (Manager.ViewModel.ActualAmountCellProps.RegisterGridRow.TransactionType)
            {
                case TransactionTypes.Deposit:
                    IsIncome = true;
                    break;
                case TransactionTypes.Withdrawal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var lookupDefinition = AppGlobals.LookupContext.BudgetItemSourceLookup.Clone();
            lookupDefinition.FilterDefinition.AddFixedFilter(p => p.IsIncome, Conditions.Equals, IsIncome);
            _sourceAutoFillSetup = new AutoFillSetup(lookupDefinition);

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
                case ActualAmountGridColumns.Source:
                    _sourceAutoFillSetup.AddViewParameter = new ViewModelInput() {SourceHistoryIsIncome = IsIncome};
                    return new DataEntryGridAutoFillCellProps(this, columnId, _sourceAutoFillSetup, Source);
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
                case ActualAmountGridColumns.Source:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        //if (autoFillCellProps.AutoFillValue.IsValid())
                        //{
                        //    var source = new BudgetItemSource
                        //    {
                        //        Name = autoFillCellProps.AutoFillValue.Text,
                        //        IsIncome = IsIncome
                        //    };
                        //    if (AppGlobals.DataRepository.SaveBudgetItemSource(source))
                        //    {
                        //        Source = new AutoFillValue(
                        //            AppGlobals.LookupContext.BudgetItemSources.GetPrimaryKeyValueFromEntity(source), source.Name);
                        //    }
                        //}
                        //else 
                        Source = autoFillCellProps.AutoFillValue;
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

        public void LoadFromEntity(BankAccountRegisterItemAmountDetail amountDetail)
        {
            Date = amountDetail.Date;
            Source = new AutoFillValue(AppGlobals.LookupContext.BudgetItemSources.GetPrimaryKeyValueFromEntity(amountDetail.Source),
                amountDetail.Source.Name);
            Amount = amountDetail.Amount;
        }

        public bool ValidateRow()
        {
            return true;
        }

        public void SaveToEntity(BankAccountRegisterItemAmountDetail entity, int rowIndex)
        {
            entity.RegisterId = Manager.ViewModel.ActualAmountCellProps.RegisterGridRow.RegisterId;
            entity.DetailId = rowIndex;
            entity.Date = Date;
            var source = AppGlobals.LookupContext.BudgetItemSources.GetEntityFromPrimaryKeyValue(Source.PrimaryKeyValue);
            entity.SourceId = source.Id;
            entity.Amount = Amount;
        }
    }
}
