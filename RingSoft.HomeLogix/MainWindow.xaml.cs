using RingSoft.HomeLogix.Budget;
using RingSoft.HomeLogix.Library.ViewModels.Main;
using System.Windows.Input;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainView
    {
        public MainWindow()
        {
            InitializeComponent();

            ContentRendered += (sender, args) => ViewModel.OnViewLoaded(this);

            PreviewKeyDown += MainWindow_PreviewKeyDown;

            Loaded += (sender, args) =>
            {
                BudgetLookupControl.Focus();
            };

            ChangeHouseholdButton.ToolTip.HeaderText = "Change Household (Alt + T)";
            ChangeHouseholdButton.ToolTip.DescriptionText = "Login to a different household.";

            ManageBudgetButton.ToolTip.HeaderText = "Manage Budget Items (Alt + M)";
            ManageBudgetButton.ToolTip.DescriptionText = "Change budget item properties.";

            ManageBankButton.ToolTip.HeaderText = "Manage Bank Accounts (Alt + B)";
            ManageBankButton.ToolTip.DescriptionText = "Manage and reconcile bank accounts.";

            PreviousMonthButton.ToolTip.HeaderText = "Goto Previous Month (Ctrl + <--)";
            PreviousMonthButton.ToolTip.DescriptionText = "See budget totals for the previous month.";

            NextMonthButton.ToolTip.HeaderText = "Goto Next Month (Ctrl + --)";
            NextMonthButton.ToolTip.DescriptionText = "See budget totals for the ext month.";
        }

        private void MainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Left:
                        if (ViewModel.PreviousMonthCommand.IsEnabled)
                        {
                            ViewModel.PreviousMonthCommand.Execute(null);
                            e.Handled = true;
                        }
                        else
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                        break;
                    case Key.Right:
                        if (ViewModel.NextMonthCommand.IsEnabled)
                        {
                            ViewModel.NextMonthCommand.Execute(null);
                            e.Handled = true;
                        }
                        else
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                        break;
                }
            }
        }

        public bool ChangeHousehold()
        {
            var loginWindow = new LoginWindow { Owner = this };

            var result = false;
            var loginResult = loginWindow.ShowDialog();

            if (loginResult != null)
                result = (bool)loginResult;

            return result;
        }

        public void ManageBudget()
        {
            var budgetItemWindow = new BudgetItemWindow { Owner = this };
            budgetItemWindow.ShowDialog();
        }

        public void ManageBankAccounts()
        {
            var bankAccountMaintenanceWindow = new BankAccountMaintenanceWindow { Owner = this };
            bankAccountMaintenanceWindow.ShowDialog();
        }
    }
}