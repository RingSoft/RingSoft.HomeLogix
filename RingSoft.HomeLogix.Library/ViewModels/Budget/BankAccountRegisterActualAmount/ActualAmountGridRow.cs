﻿using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.Sqlite.Migrations;

// ReSharper disable once CheckNamespace
namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public class ActualAmountGridRow : DataEntryGridRow
    {
        public new BankAccountRegisterActualAmountGridManager Manager { get; }

        public DateTime Date { get; set; } = DateTime.Today;

        public AutoFillValue SourceAutoFillValue { get; set; }

        public double Amount { get; set; }

        public string BankText { get; set; }

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
                    return new DataEntryGridAutoFillCellProps(this, columnId, _sourceAutoFillSetup, SourceAutoFillValue);
                case ActualAmountGridColumns.Amount:
                    return new DataEntryGridDecimalCellProps(this, columnId, _amountSetup, Amount);
                case ActualAmountGridColumns.BankText:
                    return new DataEntryGridTextCellProps(this, columnId, BankText);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ActualAmountGridColumns) columnId;

            switch (column)
            {
                case ActualAmountGridColumns.Date:
                    break;
                case ActualAmountGridColumns.Source:
                    break;
                case ActualAmountGridColumns.Amount:
                    break;
                case ActualAmountGridColumns.BankText:
                    return new DataEntryGridControlCellStyle() {State = DataEntryGridCellStates.Disabled};
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
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
                        SourceAutoFillValue = autoFillCellProps.AutoFillValue;
                    }

                    break;
                case ActualAmountGridColumns.Amount:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        Amount = (double)decimalCellProps.Value.GetValueOrDefault(0);
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
            SourceAutoFillValue = amountDetail.Source.GetAutoFillValue();
            Amount = amountDetail.Amount;
            BankText = amountDetail.BankText;
        }

        public void SaveToEntity(BankAccountRegisterItemAmountDetail entity, int rowIndex)
        {
            entity.RegisterId = Manager.ViewModel.ActualAmountCellProps.RegisterGridRow.RegisterId;
            entity.DetailId = rowIndex;
            entity.Date = Date;
            entity.SourceId = SourceAutoFillValue.GetEntity<BudgetItemSource>().Id;
            entity.Amount = Amount;
            entity.BankText = BankText;
        }
    }
}
