using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix
{
    public class DataEntryGridMiscHost : DataEntryGridEditingControlHost<RegisterGridBudgetItemAutoFillControl>
    {
        public override bool IsDropDownOpen => false;

        private MiscCellProps _cellProps;

        public DataEntryGridMiscHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return _cellProps;
        }

        public override bool HasDataChanged()
        {
            return false;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
        }

        protected override void OnControlLoaded(RegisterGridBudgetItemAutoFillControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            _cellProps = cellProps as MiscCellProps;
            Control.TextBox.Text = _cellProps?.Text!;
            Control.TextBox.SelectAll();

            Control.ShowBudgetWindow += (sender, args) =>
            {
                var viewModelInput = _cellProps.Row.Manager.ViewModel.ViewModelInput;
                viewModelInput.FromRegisterGrid = true;

                var registerItem = new BankAccountRegisterItem();
                _cellProps.Row.SaveToEntity(registerItem, 0);
                if (_cellProps.Row.Manager.ViewModel.BankAccountView.ShowBankAccountMiscWindow(registerItem,
                    _cellProps.Row.Manager.ViewModel.ViewModelInput))
                {
                    _cellProps.Row.LoadFromEntity(registerItem);
                    Grid.UpdateRow(_cellProps.Row);
                    Grid.GotoCell(_cellProps.Row, _cellProps.ColumnId);
                }
            };

        }
    }
}
