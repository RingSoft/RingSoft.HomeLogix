using System.Linq;
using System.Windows;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

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


                var currentRowIndex = Grid.CurrentRowIndex;
                lookupDefinition.ShowAddOnTheFlyWindow(_cellProps.Text,
                    _cellProps.Row.Manager.ViewModel.BankAccountView.OwnerWindow, null, viewModelInput, _cellProps.Row.BudgetItemValue.PrimaryKeyValue);

                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                if (window != null)
                {
                    window.Closed += (sender, args) =>
                    {
                        if (currentRowIndex > Grid.Manager.Rows.Count - 1)
                            currentRowIndex = Grid.Manager.Rows.Count - 1;

                        var registerRow = Grid.Manager.Rows.OfType<BankAccountRegisterGridRow>()
                            .FirstOrDefault(f => f.RegisterId == _cellProps.Row.RegisterId);

                        if (registerRow != null)
                            currentRowIndex = Grid.Manager.Rows.IndexOf(registerRow);

                        Grid.GotoCell(Grid.Manager.Rows[currentRowIndex], _cellProps.ColumnId);
                    };
                }
                Grid.DataEntryGridCancelEdit();
            };
        }
    }
}