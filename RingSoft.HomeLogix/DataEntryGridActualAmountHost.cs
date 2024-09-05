using System.Linq;
using Accessibility;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.HomeLogix.Budget;
using RingSoft.HomeLogix.ImportBank;
using RingSoft.HomeLogix.Library;

namespace RingSoft.HomeLogix
{
    public class DataEntryGridActualAmountHost : DataEntryGridDropDownControlHost<ActualAmountGridControl>
    {
        public ActualAmountCellProps ActualAmountCellProps { get; private set; }

        public DataEntryGridActualAmountHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new ActualAmountCellProps(Row, ColumnId,
                ActualAmountCellProps.NumericEditSetup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            return Control.Value != ActualAmountCellProps.Value;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            ActualAmountCellProps = (ActualAmountCellProps)cellProps;
        }

        protected override void OnControlLoaded(ActualAmountGridControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            ActualAmountCellProps = (ActualAmountCellProps)cellProps;

            control.Setup = ActualAmountCellProps.NumericEditSetup;
            control.Value = ActualAmountCellProps.Value;

            SetAmountMode();

            control.CalculatorValueChanged += (_, _) => OnUpdateSource(GetCellValue());
            control.ShowDetailsWindow += (_, _) =>
            {
                var newCellProps = GetCellValue() as ActualAmountCellProps;
                OnUpdateSource(newCellProps);

                if (ActualAmountCellProps.RegisterGridRow != null)
                {
                    ActualAmountCellProps.RegisterGridRow.Manager.ViewModel
                        .BankAccountView.ShowActualAmountDetailsWindow(newCellProps);
                    ActualAmountCellProps.RegisterGridRow.ActualAmount = newCellProps.Value.GetValueOrDefault();
                    ActualAmountCellProps.RegisterGridRow.Manager.ViewModel.CalculateTotals();
                }

                if (ActualAmountCellProps.ImportTransactionGridRow != null)
                {
                    newCellProps?.ImportTransactionGridRow.Manager.ViewModel.View.ShowImportBankBudgetWindow(
                        ActualAmountCellProps.ImportTransactionGridRow);
                }

                SetAmountMode();
                control.Value = newCellProps.Value;
                control.TextBox.SelectAll();
            };

            base.OnControlLoaded(control, cellProps, cellStyle);
        }

        private void SetAmountMode()
        {
            if (ActualAmountCellProps.RegisterGridRow != null)
            {
                if (ActualAmountCellProps.RegisterGridRow.ActualAmountDetails.Any())
                    Control.AmountMode = ActualAmountMode.Details;
                else
                    Control.AmountMode = ActualAmountMode.Value;
            }

            if (ActualAmountCellProps.ImportTransactionGridRow != null)
            {
                if (ActualAmountCellProps.ImportTransactionGridRow.BudgetItemSplits.Any())
                {
                    Control.AmountMode = ActualAmountMode.Details;
                }
                else
                {
                    Control.AmountMode = ActualAmountMode.Value;
                }
            }
        }
    }
}
