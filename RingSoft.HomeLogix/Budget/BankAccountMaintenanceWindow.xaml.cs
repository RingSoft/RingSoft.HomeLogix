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
        }

        protected override void OnLoaded()
        {
            TopHeaderControl.PreviousButton.ToolTip.HeaderText =
                "Goto Previous Bank Account (Alt + Left Arrow)";

            if (TopHeaderControl.CustomPanel is BankCustomPanel bankCustomPanel)
            {
                bankCustomPanel.Button1.ToolTip.HeaderText = "This is working!";
            }

            base.OnLoaded();
        }
    }
}
