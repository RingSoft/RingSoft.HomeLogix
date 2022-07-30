using RingSoft.App.Controls;
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
        }

        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Budget Period History";
        public override DbMaintenanceViewModelBase ViewModel => BudgetPeridHistoryViewModel;
    }
}
