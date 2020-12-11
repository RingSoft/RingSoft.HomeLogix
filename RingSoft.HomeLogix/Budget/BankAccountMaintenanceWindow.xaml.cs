using RingSoft.App.Controls;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BankAccountMaintenanceWindow.xaml
    /// </summary>
    public partial class BankAccountMaintenanceWindow
    {
        public override DbMaintenanceTopHeaderControl DbMaintenanceTopHeaderControl => TopHeaderControl;

        public BankAccountMaintenanceWindow()
        {
            InitializeComponent();

            TopHeaderControl.Loaded += (sender, args) =>
            {
                TopHeaderControl.ButtonsControl.PreviousButton.ToolTipHeader =
                    "Goto Previous Bank Account (Alt + Left Arrow)";
            };
        }
    }
}
