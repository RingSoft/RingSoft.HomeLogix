using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public class ImportExpiredGridRow : DataEntryGridRow
    {
        public enum ImportExpiredColumns
        {
            RemoveItem = 0,
            ClearItem = 1,
            Date = 2,
            Description = 3,
            Amount = 4,
        }

        public const int RemoveItemColumnId = (int)ImportExpiredColumns.RemoveItem;
        public const int ClearColumnId = (int)ImportExpiredColumns.ClearItem;
        public const int DateColumnId = (int)ImportExpiredColumns.Date;
        public const int DescriptionColumnId = (int)ImportExpiredColumns.Description;
        public const int AmountColumnId = (int)ImportExpiredColumns.Amount;

        public new ImportExpiredGridManager Manager { get; private set; }

        public BankAccountRegisterGridRow ExpiredRow { get; private set; }

        public bool RemoveRow { get; private set; } = true;

        public bool ClearRow { get; private set; }

        public DateTime Date { get; private set; }

        public string Description { get; private set; }

        public double Amount { get; private set; }

        public ImportExpiredGridRow(ImportExpiredGridManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ImportExpiredColumns)columnId;
            switch (column)
            {
                case ImportExpiredColumns.RemoveItem:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, RemoveRow);
                case ImportExpiredColumns.ClearItem:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, ClearRow);
                case ImportExpiredColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId,
                        new DateEditControlSetup() { DateFormatType = DateFormatTypes.DateOnly }, Date);
                case ImportExpiredColumns.Description:
                    return new DataEntryGridTextCellProps(this, columnId, Description);
                case ImportExpiredColumns.Amount:
                    return new DataEntryGridDecimalCellProps(this, columnId,
                        new DecimalEditControlSetup()
                        {
                            FormatType = DecimalEditFormatTypes.Currency
                        }, Amount);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (ImportExpiredColumns)columnId;

            switch (column)
            {
                case ImportExpiredColumns.RemoveItem:
                    return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Enabled };
                case ImportExpiredColumns.ClearItem:
                    if (RemoveRow)
                    {
                        return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Disabled };
                    }
                    else
                    {
                        return new DataEntryGridControlCellStyle() { State = DataEntryGridCellStates.Enabled };
                    }
                case ImportExpiredColumns.Date:
                    break;
                case ImportExpiredColumns.Description:
                    break;
                case ImportExpiredColumns.Amount:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new DataEntryGridCellStyle() { State = DataEntryGridCellStates.Disabled };
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ImportExpiredColumns)value.ColumnId;

            switch (column)
            {
                case ImportExpiredColumns.RemoveItem:
                    if (value is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                    {
                        RemoveRow = checkBoxCellProps.Value;
                        if (RemoveRow)
                        {
                            ClearRow = false;
                        }
                    }
                    break;
                case ImportExpiredColumns.ClearItem:
                    if (value is DataEntryGridCheckBoxCellProps clearRowCheckBoxCellProps)
                    {
                        ClearRow = clearRowCheckBoxCellProps.Value;
                    }
                    break;
                case ImportExpiredColumns.Date:
                    break;
                case ImportExpiredColumns.Description:
                    break;
                case ImportExpiredColumns.Amount:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public void LoadGridRow(BankAccountRegisterGridRow row)
        {
            ExpiredRow = row;
            Date = row.ItemDate;
            Description = row.Description;
            Amount = Math.Abs(row.ProjectedAmount);
        }
    }
}
