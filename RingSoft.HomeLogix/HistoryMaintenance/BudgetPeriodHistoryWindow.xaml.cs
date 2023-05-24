using System.Windows;
using RingSoft.App.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.HomeLogix.HistoryMaintenance
{
    /// <summary>
    /// Interaction logic for BudgetPeriodHistoryWindow.xaml
    /// </summary>
    public partial class BudgetPeriodHistoryWindow : DbMaintenanceWindow
    {
        public BudgetPeriodHistoryWindow()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                LookupControl.Focus();
                StatusBar.Visibility = Visibility.Collapsed;
            };
        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Budget Period History";
        public override DbMaintenanceViewModelBase ViewModel => BudgetPeridHistoryViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;
    }
}
