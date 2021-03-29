using System;
using System.Globalization;
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

        public BankAccountRegisterGridManager BankAccountRegisterGridManager { get; }

        public string RegisterId { get; private set; }

        public DateTime ItemDate { get; private set; }

        public abstract string Description { get; }

        public TransactionTypes TransactionType { get; private set; }

        public decimal ProjectedAmount { get; private set; }

        public bool Completed { get; private set; }

        public decimal Balance { get; private set; }

        public decimal? ActualAmount { get; private set; }

        private DecimalEditControlSetup _decimalValueSetup;
        private DateEditControlSetup _dateEditControlSetup;

        protected BankAccountRegisterGridRow(BankAccountRegisterGridManager manager) : base(manager)
        {
            BankAccountRegisterGridManager = manager;
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
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalValueSetup, ActualAmount);
                case BankAccountRegisterGridColumns.Difference:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalValueSetup,
                        ProjectedAmount - ActualAmount);
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
                    return new DataEntryGridControlCellStyle {State = DataEntryGridCellStates.ReadOnly};
                case BankAccountRegisterGridColumns.Date:
                case BankAccountRegisterGridColumns.Description:
                case BankAccountRegisterGridColumns.Amount:
                case BankAccountRegisterGridColumns.ActualAmount:
                    break;
                case BankAccountRegisterGridColumns.Balance:
                case BankAccountRegisterGridColumns.Difference:
                    return new DataEntryGridCellStyle {State = DataEntryGridCellStates.Disabled};
                case BankAccountRegisterGridColumns.Completed:
                    return new DataEntryGridControlCellStyle();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }

        public override void LoadFromEntity(BankAccountRegisterItem entity)
        {
            RegisterId = entity.RegisterId;
            ItemDate = entity.ItemDate;
            TransactionType = entity.ProjectedAmount < 0 ? TransactionTypes.Withdrawal : TransactionTypes.Deposit;
            ProjectedAmount = Math.Abs(entity.ProjectedAmount);
            ActualAmount = entity.ActualAmount;
        }
    }
}
