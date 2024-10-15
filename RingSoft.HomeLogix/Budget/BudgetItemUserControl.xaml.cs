using System.Windows;
using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BudgetItemUserControl.xaml
    /// </summary>
    public partial class BudgetItemUserControl : IBudgetItemView
    {
        public BudgetItemUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(DescriptionControl);

            TopHeaderControl.Loaded += (sender, args) =>
            {
                if (TopHeaderControl.CustomPanel is BudgetCustomPanel budgetCustomPanel)
                {
                    budgetCustomPanel.AddButton.Command = BudgetItemViewModel.AddAdjustmentCommand;
                    budgetCustomPanel.ClearRecurButton.Command = BudgetItemViewModel.ClearRecurringCommand;
                }
            };
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return BudgetItemViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return TopHeaderControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Budget Item";
        }

        public void SetViewType()
        {
            TransferToStackPanel.Visibility =
                BudgetItemViewModel.TransferToBankVisible ? Visibility.Visible : Visibility.Collapsed;

        }

        public void ShowMonthlyStatsControls(bool show = true)
        {
            
        }

        public bool AddAdjustment(BudgetItem budgetItem)
        {
            var win = new BudgetItemAdjustmentWindow(budgetItem);
            win.ShowInTaskbar = false;
            win.Owner = OwnerWindow;
            return win.ShowDialog();
        }
    }
}
