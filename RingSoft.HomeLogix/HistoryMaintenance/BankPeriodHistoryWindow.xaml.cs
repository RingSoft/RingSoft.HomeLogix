using System.Windows;
using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.HistoryMaintenance
{
    /// <summary>
    /// Interaction logic for BankPeriodHistoryWindow.xaml
    /// </summary>
    public partial class BankPeriodHistoryWindow : DbMaintenanceWindow
    {
        public BankPeriodHistoryWindow()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                LookupControl.Focus();
                StatusBar.Visibility = Visibility.Collapsed;
            };
        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Bank Period History";
        public override DbMaintenanceViewModelBase ViewModel => BankPeridHistoryViewModel;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;
    }
}
