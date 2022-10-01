using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.HomeLogix.Library.ViewModels.ImportBank
{
    public enum ImportBudgetsColumn
    {
        BudgetItem = 0,
        Amount = 1,
    }
    public class ImportBankTransactionsBudgetsGridRow : DataEntryGridRow
    {
        public const int BudgetColumnId = (int)ImportBudgetsColumn.BudgetItem;
        public const int AmountColumnId = (int)ImportBudgetsColumn.Amount;

        public ImportBankTransactionsBudgetManager Manager { get; set; }
        public AutoFillSetup BudgetAutoFillSetup { get; set; }
        public AutoFillValue BudgetAutoFillValue { get; set; }
        public decimal BudgetAmount { get; set; }

        public ImportBankTransactionsBudgetsGridRow(ImportBankTransactionsBudgetManager manager) : base(manager)
        {
            Manager = manager;
            BudgetAutoFillSetup = new AutoFillSetup(AppGlobals.LookupContext.BudgetItemsLookup)
                {AddViewParameter = Manager.ViewModel.Row.Manager.ViewModel.BankViewModel.ViewModelInput};
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (ImportBudgetsColumn) columnId;

            switch (column)
            {
                case ImportBudgetsColumn.BudgetItem:
                    return new DataEntryGridAutoFillCellProps(this, columnId, BudgetAutoFillSetup, BudgetAutoFillValue);
                case ImportBudgetsColumn.Amount:
                    return new DataEntryGridDecimalCellProps(this, columnId,
                        new DecimalEditControlSetup
                        {
                            FormatType = DecimalEditFormatTypes.Currency,
                        }, BudgetAmount);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (ImportBudgetsColumn)value.ColumnId;

            switch (column)
            {
                case ImportBudgetsColumn.BudgetItem:
                    var autoFillCellProps = value as DataEntryGridAutoFillCellProps;
                    if (autoFillCellProps != null)
                    {
                        BudgetAutoFillValue = autoFillCellProps.AutoFillValue;
                    }
                    break;
                case ImportBudgetsColumn.Amount:
                    var amountCellProps = value as DataEntryGridDecimalCellProps;
                    if (amountCellProps != null)
                    {
                        if (amountCellProps.Value != null) BudgetAmount = amountCellProps.Value.Value;
                        Manager.SetLastRowAmount();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void Dispose()
        {
            BudgetAmount = 0;
            Manager.SetLastRowAmount();
            base.Dispose();
        }
    }
}
