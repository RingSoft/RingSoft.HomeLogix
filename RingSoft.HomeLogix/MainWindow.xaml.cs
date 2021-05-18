using System.Windows.Input;
using RingSoft.App.Controls;
using RingSoft.HomeLogix.Budget;
using RingSoft.HomeLogix.Library.ViewModels.Main;

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

            TestButton.Click += (sender, args) =>
            {
                var testWindow = new TestWindow { Owner = this };
                testWindow.ShowDialog();
            };

            PreviewKeyDown += MainWindow_PreviewKeyDown;

            Loaded += (sender, args) =>
            {
                BudgetLookupControl.Focus();
            };
        }

        private void MainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Left:
                        ViewModel.PreviousMonthCommand.Execute(null);
                        e.Handled = true;
                        break;
                    case Key.Right:
                        ViewModel.NextMonthCommand.Execute(null);
                        e.Handled = true;
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