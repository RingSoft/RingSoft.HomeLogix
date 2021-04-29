using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;

namespace RingSoft.HomeLogix
{
    public class DataEntryGridBudgetItemHost : DataEntryGridEditingControlHost<RegisterGridBudgetItemAutoFillControl>
    {
        public override bool IsDropDownOpen => false;

        private BudgetItemCellProps _cellProps;

        public DataEntryGridBudgetItemHost(DataEntryGrid grid) : base(grid)
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
            _cellProps = cellProps as BudgetItemCellProps;
            Control.TextBox.Text = _cellProps?.Text!;
            Control.TextBox.SelectAll();

            Control.ShowBudgetWindow += (sender, args) =>
            {
                var viewModelInput = _cellProps.Row.Manager.ViewModel.ViewModelInput;
                viewModelInput.FromRegisterGrid = true;

                var budgetItem =
                    AppGlobals.DataRepository.GetBudgetItem(_cellProps.Row.BudgetItemId);
                
                var lookupDefinition = AppGlobals.LookupContext.BudgetItemsLookup.Clone();
                lookupDefinition.FilterDefinition.AddFixedFilter(p => p.BankAccountId, Conditions.Equals,
                    budgetItem.BankAccountId);

                lookupDefinition.ShowAddOnTheFlyWindow(_cellProps.Text,
                    _cellProps.Row.Manager.ViewModel.BankAccountView.OwnerWindow, null, viewModelInput);
            };
        }
    }
}
