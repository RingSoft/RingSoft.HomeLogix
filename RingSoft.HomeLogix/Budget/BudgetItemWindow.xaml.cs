using System.Windows;
using RingSoft.App.Controls;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BudgetItemWindow.xaml
    /// </summary>
    public partial class BudgetItemWindow : IBudgetItemView
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Budget Item";
        public override DbMaintenanceViewModelBase ViewModel => BudgetItemViewModel;

        public BudgetItemWindow()
        {
            InitializeComponent();
        }

        public void SetViewType(RecurringViewTypes viewType)
        {
            
        }
    }
}
