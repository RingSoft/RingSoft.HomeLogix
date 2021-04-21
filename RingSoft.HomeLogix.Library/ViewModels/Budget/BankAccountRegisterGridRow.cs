using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels.Budget
{
    public abstract class BankAccountRegisterGridRow : DbMaintenanceDataEntryGridRow<BankAccountRegisterItem>
    {
        public abstract BankAccountRegisterItemTypes LineType { get; }

        public new BankAccountRegisterGridManager Manager { get; }

        public int RegisterId { get; set; }

        public string RegisterGuid { get; private set; }

        public DateTime ItemDate { get; private set; }

        public abstract string Description { get; }

        public TransactionTypes TransactionType { get; private set; }

        public decimal ProjectedAmount { get; private set; }

        public bool Completed { get; private set; }

        public decimal? Balance { get; set; }

        public decimal? ActualAmount { get; private set; }

        public decimal? Difference
        {
            get
            {
                switch (TransactionType)
                {
                    case TransactionTypes.Deposit:
                        return ActualAmount - ProjectedAmount;
                    case TransactionTypes.Withdrawal:
                        return ProjectedAmount - ActualAmount;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public List<BankAccountRegisterItemAmountDetail> ActualAmountDetails { get; private set; }

        private DecimalEditControlSetup _decimalValueSetup;
        private DateEditControlSetup _dateEditControlSetup;

        protected BankAccountRegisterGridRow(BankAccountRegisterGridManager manager) : base(manager)
        {
            Manager = manager;
            _decimalValueSetup = new DecimalEditControlSetup
            {
                AllowNullValue = false,
                DataEntryMode = DataEntryModes.FormatOnEntry,
                MaximumValue = null,
                MinimumValue = 0,
                NumberFormatString = null,
                FormatType = DecimalEditFormatTypes.Currency,
                Precision = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits
            };

            _dateEditControlSetup = new DateEditControlSetup
            {
                AllowNullValue = false,
                DateFormatType = DateFormatTypes.DateOnly,
            };

            ActualAmountDetails = new List<BankAccountRegisterItemAmountDetail>();
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (BankAccountRegisterGridColumns) columnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.ItemType:
                    return new DataEntryGridCustomControlCellProps(this, columnId, (int) LineType);
                case BankAccountRegisterGridColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId, _dateEditControlSetup, ItemDate);
                case BankAccountRegisterGridColumns.Description:
                    return new DataEntryGridTextCellProps(this, columnId, Description);
                case BankAccountRegisterGridColumns.TransactionType:
                    return new DataEntryGridCustomControlCellProps(this, columnId, (int) TransactionType);
                case BankAccountRegisterGridColumns.Amount:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalValueSetup, ProjectedAmount);
                case BankAccountRegisterGridColumns.Completed:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, Completed);
                case BankAccountRegisterGridColumns.Balance:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalValueSetup, Balance);
                case BankAccountRegisterGridColumns.ActualAmount:
                    return new ActualAmountCellProps(this, columnId, _decimalValueSetup, ActualAmount);
                case BankAccountRegisterGridColumns.Difference:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalValueSetup, Difference);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (BankAccountRegisterGridColumns) columnId;

            switch (column)
            {
                case BankAccountRegisterGridColumns.ItemType:
                case BankAccountRegisterGridColumns.TransactionType:
                case BankAccountRegisterGridColumns.Date:
                    return new DataEntryGridControlCellStyle {State = DataEntryGridCellStates.ReadOnly};
                case BankAccountRegisterGridColumns.Description:
                case BankAccountRegisterGridColumns.Amount:
                case BankAccountRegisterGridColumns.ActualAmount:
                    break;
                case BankAccountRegisterGridColumns.Balance:
                    var style = new DataEntryGridCellStyle {State = DataEntryGridCellStates.Disabled};
                    if (Balance < 0)
                        style.DisplayStyleId = BankAccountRegisterGridManager.NegativeDisplayId;
                    return style;
                case BankAccountRegisterGridColumns.Difference:
                    style = new DataEntryGridCellStyle { State = DataEntryGridCellStates.Disabled };
                    if (Difference < 0)
                        style.DisplayStyleId = BankAccountRegisterGridManager.NegativeDisplayId;
                    return style;
                case BankAccountRegisterGridColumns.Completed:
                    return new DataEntryGridControlCellStyle();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (BankAccountRegisterGridColumns) value.ColumnId;
            switch (column)
            {
                case BankAccountRegisterGridColumns.ItemType:
                    break;
                case BankAccountRegisterGridColumns.Date:
                    break;
                case BankAccountRegisterGridColumns.Description:
                    break;
                case BankAccountRegisterGridColumns.TransactionType:
                    break;
                case BankAccountRegisterGridColumns.Amount:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        ProjectedAmount = decimalCellProps.Value.GetValueOrDefault(0);
                        Manager.ViewModel.CalculateTotals();
                    }
                    break;
                case BankAccountRegisterGridColumns.Completed:
                    if (value is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                    {
                        Completed = checkBoxCellProps.Value;
                        if (Completed && ActualAmount == null)
                            ActualAmount = ProjectedAmount;
                        Manager.ViewModel.CalculateTotals();
                    }
                    break;
                case BankAccountRegisterGridColumns.Balance:
                    break;
                case BankAccountRegisterGridColumns.ActualAmount:
                    var actualAmountCellProps = (DataEntryGridDecimalCellProps) value;
                    if (actualAmountCellProps.Value != null) 
                        ActualAmount = actualAmountCellProps.Value.Value;

                    break;
                case BankAccountRegisterGridColumns.Difference:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            RegisterId = entity.Id;
            RegisterGuid = entity.RegisterGuid;
            ItemDate = entity.ItemDate;
            TransactionType = entity.ProjectedAmount < 0 ? TransactionTypes.Withdrawal : TransactionTypes.Deposit;
            ProjectedAmount = Math.Abs(entity.ProjectedAmount);
            ActualAmount = entity.ActualAmount;
            ActualAmountDetails = entity.AmountDetails.ToList();
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(BankAccountRegisterItem entity, int rowIndex)
        {
            entity.Id = RegisterId;
            entity.BankAccountId = Manager.ViewModel.Id;
            entity.RegisterGuid = RegisterGuid;
            entity.ItemType = (int)LineType;
            entity.ItemDate = ItemDate;
            entity.ActualAmount = ActualAmount;
            switch (TransactionType)
            {
                case TransactionTypes.Deposit:
                    entity.ProjectedAmount = ProjectedAmount;
                    break;
                case TransactionTypes.Withdrawal:
                    entity.ProjectedAmount = -ProjectedAmount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (ActualAmount != null)
            {
                var detailId = 1;
                foreach (var actualAmountDetail in ActualAmountDetails)
                {
                    Manager.ViewModel.RegisterDetails.Add(
                        new BankAccountRegisterItemAmountDetail
                    {
                        RegisterId = RegisterId,
                        DetailId = detailId,
                        Date = actualAmountDetail.Date,
                        SourceId = actualAmountDetail.SourceId,
                        Amount = actualAmountDetail.Amount
                    });
                    detailId++;
                }
            }
        }
    }
}
