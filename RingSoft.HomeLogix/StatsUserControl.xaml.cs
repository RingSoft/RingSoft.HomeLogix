using System.Collections.Generic;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance;
using RingSoft.HomeLogix.Library.ViewModels.Main;

namespace RingSoft.HomeLogix
{
    /// <summary>
    /// Interaction logic for StatsUserControl.xaml
    /// </summary>
    public partial class StatsUserControl : IMainView
    {
        public StatsUserControl()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                BudgetLookupControl.Focus();
                ShowChart(true);
                ViewModel.OnViewLoaded(this);
            };
        }

        public bool ChangeHousehold()
        {
            return true;
        }

        public void ManageBudget()
        {
            
        }

        public void ManageBankAccounts()
        {
            
        }

        public void LaunchAdvancedFind()
        {
            
        }

        public void CloseApp()
        {
            
        }

        public void ShowChart(bool show = true)
        {
            BudgetChart.Visibility = ActualChart.Visibility = Visibility.Visible;
        }

        public Login ShowPhoneSync(Login input)
        {
            return null;
        }

        public void ShowRichMessageBox(string message, string caption, RsMessageBoxIcons icon, List<HyperlinkData> hyperLinks = null)
        {
            
        }

        public string GetWriteablePath()
        {
            return string.Empty;
        }

        public bool UpgradeVersion()
        {
            return true;
        }

        public void ShowHistoryPrintFilterWindow(HistoryPrintFilterCallBack callBack)
        {
            
        }

        public void ShowAbout()
        {
            
        }

        public void GetChangeDate(ChangeDateData data)
        {
            
        }
    }
}
