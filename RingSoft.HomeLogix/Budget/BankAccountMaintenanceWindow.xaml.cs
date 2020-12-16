using RingSoft.App.Controls;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankAccountMaintenanceWindow.xaml
    /// </summary>
    public partial class BankAccountMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Bank Account";

        public BankAccountMaintenanceWindow()
        {
            InitializeComponent();
        }
    }
}
