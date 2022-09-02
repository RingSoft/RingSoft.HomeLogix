using System.Collections.ObjectModel;
using RingSoft.HomeLogix.Budget;
using RingSoft.HomeLogix.Library.ViewModels.Main;
using System.Windows.Input;
using System.Windows.Media;
using RingSoft.App.Library;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using ScottPlot;
using Color = System.Drawing.Color;

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

            Loaded += (sender, args) => { BudgetLookupControl.Focus(); };

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

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //ObservableCollection<PieSegment> pieCollection = new ObservableCollection<PieSegment>();
            //pieCollection.Add(new PieSegment { Color = Colors.Green, Value = 5, Name = "Dogs" });
            //pieCollection.Add(new PieSegment { Color = Colors.Yellow, Value = 12, Name = "Cats" });
            //pieCollection.Add(new PieSegment { Color = Colors.Red, Value = 20, Name = "Mice" });
            //pieCollection.Add(new PieSegment { Color = Colors.DarkCyan, Value = 22, Name = "Lizards" });
            //pie1.Data = pieCollection;
            //chart1.Data = pieCollection;

            //double[] values = {778, 43, 283, 76, 184};
            //WpfPlot.plt.PlotPie(values, showValues: true);

            //WpfPlot.plt.Grid(false);
            //WpfPlot.plt.Frame(false);
            //WpfPlot.plt.Ticks(false, false);
            //WpfPlot.plt.Style(figBg: Color.Transparent, dataBg: Color.Transparent);
            //WpfPlot.plt.Title("Hello");

            //WpfPlot.Render();
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

        public void LaunchAdvancedFind()
        {
            var window = new AdvancedFindWindow();
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
        }
    }
}