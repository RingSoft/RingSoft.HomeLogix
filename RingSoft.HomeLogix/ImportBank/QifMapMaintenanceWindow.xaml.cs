using RingSoft.App.Controls;
using RingSoft.DbMaintenance;

namespace RingSoft.HomeLogix.ImportBank
{
    /// <summary>
    /// Interaction logic for QifMapMaintenance.xaml
    /// </summary>
    public partial class QifMapMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;
        public override string ItemText => "Qif Map";
        public override DbMaintenanceViewModelBase ViewModel => LocalViewModel;
        
        public QifMapMaintenanceWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(BankTextControl);
        }

    }
}
