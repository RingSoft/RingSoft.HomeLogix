
using RingSoft.App.Controls;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.HomeLogix.HistoryMaintenance;
using RingSoft.HomeLogix.Library;
using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance;
using RingSoft.HomeLogix.Library.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using RingSoft.DbLookup.Lookup;
using System.ComponentModel;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMainView, ICheckVersionView
    {
        public MainWindow()
        {
            InitializeComponent();
            LookupControlsGlobals.SetTabSwitcherWindow(this, TabControl);
            TabControl.SetDestionationAsFirstTab = false;
            ContentRendered += (sender, args) =>
            {
                ViewModel.OnViewLoaded(this);
            };

            //PreviewKeyDown += MainWindow_PreviewKeyDown;

            Loaded += (sender, args) =>
            {
                //BudgetLookupControl.Focus();
                //ShowChart(false);
                //ViewModel.OnViewLoaded(this);
            };

            ChangeHouseholdButton.ToolTip.HeaderText = "Change Household (Alt + T)";
            ChangeHouseholdButton.ToolTip.DescriptionText = "Login to a different household.";

            ManageBudgetButton.ToolTip.HeaderText = "Manage Budget Items (Alt + M)";
            ManageBudgetButton.ToolTip.DescriptionText = "Change budget item properties.";

            ManageBankButton.ToolTip.HeaderText = "Manage Bank Accounts (Alt + B)";
            ManageBankButton.ToolTip.DescriptionText = "Manage and reconcile bank accounts.";

            PreviousMonthButton.ToolTip.HeaderText = "Goto Previous Month";
            PreviousMonthButton.ToolTip.DescriptionText = "See budget totals for the previous month.";

            NextMonthButton.ToolTip.HeaderText = "Goto Next Month";
            NextMonthButton.ToolTip.DescriptionText = "See budget totals for the next month.";

            ChangeDateButton.ToolTip.HeaderText = "Change Budget Date (Alt + D)";
            ChangeDateButton.ToolTip.DescriptionText = "See budget totals for an explicit month.";

            //SyncPhoneButton.ToolTip.HeaderText = "Sync With Mobile Device (Alt + S)";
            //SyncPhoneButton.ToolTip.DescriptionText = "Copy data to mobile device.";

            AdvancedFindButton.ToolTip.HeaderText = "Advanced Find (Alt + A)";
            AdvancedFindButton.ToolTip.DescriptionText = "Show the advanced find window.";

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

        //private void MainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
        //    {
        //        switch (e.Key)
        //        {
        //            case Key.Left:
        //                if (ViewModel.PreviousMonthCommand.IsEnabled)
        //                {
        //                    ViewModel.PreviousMonthCommand.Execute(null);
        //                    e.Handled = true;
        //                }
        //                else
        //                {
        //                    System.Media.SystemSounds.Exclamation.Play();
        //                }
        //                break;
        //            case Key.Right:
        //                if (ViewModel.NextMonthCommand.IsEnabled)
        //                {
        //                    ViewModel.NextMonthCommand.Execute(null);
        //                    e.Handled = true;
        //                }
        //                else
        //                {
        //                    System.Media.SystemSounds.Exclamation.Play();
        //                }
        //                break;
        //        }
        //    }
        //}

        public bool ChangeHousehold(bool firstTime)
        {
            if (!TabControl.CloseAllTabs())
            {
                return false;
            }
            var loginWindow = new LoginWindow { Owner = this };

            var result = false;
            var loginResult = loginWindow.ShowDialog();
            
            if (loginResult != null && loginResult.Value == true)
                result = (bool)loginResult;


            if (result && !firstTime)
            {

            }
            return result;
        }

        public void ManageBudget()
        {
            //LookupControlsGlobals.WindowRegistry.ShowDbMaintenanceWindow(AppGlobals.LookupContext.BudgetItems);
            TabControl.ShowTableControl(AppGlobals.LookupContext.BudgetItems, false);
        }

        public void ManageBankAccounts()
        {
            //LookupControlsGlobals.WindowRegistry.ShowDbMaintenanceWindow(AppGlobals.LookupContext.BankAccounts);
            TabControl.ShowTableControl(AppGlobals.LookupContext.BankAccounts, false);
        }

        public void LaunchAdvancedFind()
        {
            var window = new AdvancedFindWindow();
            //window.Owner = this;
            //window.Closed += (sender, args) => Activate();
            
            window.ShowDialog();
        }

        public void CloseApp()
        {
            Close();
        }

        public Login ShowPhoneSync(Login input)
        {
            var phoneSyncWindow = new PhoneSyncWindow(input)
            {
                Owner = this,
                ShowInTaskbar = false,
            };
            phoneSyncWindow.ShowDialog();
            return phoneSyncWindow.ViewModel.DialogResult;
        }

        public void ShowRichMessageBox(string message, string caption, RsMessageBoxIcons icon, List<HyperlinkData> hyperLinks = null)
        {
            var richMessageBox = new RichMessageBox(message, caption, icon, hyperLinks);
            richMessageBox.Owner = this;
            richMessageBox.ShowInTaskbar = false;
            richMessageBox.ShowDialog();
        }

        public string GetWriteablePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public bool UpgradeVersion()
        {
            return AppStart.CheckVersion(this, true);
        }

        public void ShowHistoryPrintFilterWindow(HistoryPrintFilterCallBack callback)
        {
            var window = new HistoryPrintFilterWindow(callback);
            window.Owner = WPFControlsGlobals.ActiveWindow;
            window.ShowInTaskbar = false;
            window.ShowDialog();
        }

        public void ShowAbout()
        {
            var splashWindow = new AppSplashWindow(false);
            splashWindow.Title = "About HomeLogix";
            splashWindow.Owner = this;
            splashWindow.ShowInTaskbar = false;
            splashWindow.ShowDialog();
        }

        public void GetChangeDate(ChangeDateData data)
        {
            var win = new GetNewDateWindow(data);
            win.Owner = this;
            win.ShowInTaskbar = false;
            win.ShowDialog();
        }

        public void ShowStatsTab(bool show, bool setFocus)
        {
            if (show)
            {
                if (TabControl.Items.Count == 0)
                {
                    setFocus = true;
                }

                var statsUserControl = new StatsUserControl();
                TabControl.ShowUserControl(
                    statsUserControl
                    , "Statistics and Graphs"
                    , true
                    , setFocus);
            }
            else
            {
                var userControlTabItem = GetStatsTabItem();
                if (userControlTabItem != null)
                {
                    userControlTabItem.CloseTab(TabControl);
                }
            }
        }

        public void ShutDownApp()
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        public bool StatsTabExists()
        {
            var result = false;

            if (GetStatsTabItem() != null)
            {
                result = true;
            }
            return result;
        }

        public void SetTabDestination(LookupDefinitionBase lookup)
        {
            lookup.Destination = TabControl;
        }

        public UserControlTabItem GetStatsTabItem()
        {
            UserControlTabItem result = null;
            var exists = TabControl.Items.Count > 0;
            if (exists)
            {
                if (TabControl.Items[0] is UserControlTabItem userControlTabItem)
                {
                    if (userControlTabItem.UserControl is StatsUserControl)
                    {
                        result = userControlTabItem;
                    }
                }
            }
            return result;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!TabControl.CloseAllTabs())
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }
    }
}