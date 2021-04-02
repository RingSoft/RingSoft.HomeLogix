using Accessibility;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.HomeLogix.Budget;
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
            return new DataEntryGridDecimalCellProps(Row, ColumnId,
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

            control.CalculatorValueChanged += (_, _) => OnUpdateSource(GetCellValue());
            control.ShowDetailsWindow += (_, _) =>
            {
                ActualAmountCellProps.RegisterGridRow.BankAccountRegisterGridManager.BankAccountViewModel
                    .BankAccountView.ShowActualAmountDetailsWindow(ActualAmountCellProps);

                OnUpdateSource(ActualAmountCellProps);

                control.Value = ActualAmountCellProps.Value;
                control.TextBox.SelectAll();
            };

            base.OnControlLoaded(control, cellProps, cellStyle);
        }
    }
}
