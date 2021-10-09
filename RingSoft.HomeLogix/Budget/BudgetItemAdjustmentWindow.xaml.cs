using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BudgetItemAdjustmentWindow.xaml
    /// </summary>
    public partial class BudgetItemAdjustmentWindow : IBudgetItemAdjustmentView
    {
        public BudgetItemAdjustmentWindow(BudgetItem budgetItem)
        {
            InitializeComponent();
            ViewModel.OnViewLoaded(this, budgetItem);
            CancelButton.Click += (sender, args) => Close();
        }

        public new bool ShowDialog()
        {
            base.ShowDialog();
            return ViewModel.DialogResult;
        }

        public void OnOkButtonCloseWindow()
        {
            Close();
        }
    }
}
