using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.PhoneModel;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.Library.ViewModels.HistoryMaintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public class MainBudgetData
    {
        public MainBudget BudgetObj { get; set; }

        public string Description { get; set; }
    }

    public class ChangeDateData
    {
        public DateTime NewDate { get; set; }

        public bool DialogResult { get; set; }
    }
    public interface IMainView
    {
        bool ChangeHousehold(bool firstTime);

        void ManageBudget();

        void ManageBankAccounts();

        void LaunchAdvancedFind();

        void CloseApp();

        Login ShowPhoneSync(Login input);

        void ShowRichMessageBox(string message, string caption, RsMessageBoxIcons icon, List<HyperlinkData> hyperLinks = null);

        string GetWriteablePath();

        bool UpgradeVersion();

        void ShowHistoryPrintFilterWindow(HistoryPrintFilterCallBack callBack);

        void ShowAbout();

        void GetChangeDate(ChangeDateData data);

        void ShowStatsTab(bool show, bool setFocus);

        bool StatsTabExists();

        void SetTabDestination(LookupDefinitionBase lookup);
    }

    public class MainViewModel : INotifyPropertyChanged, IMainViewModel, ILookupControl
    {
        #region Properties

        private DateTime _currentMonthEnding;

        public DateTime CurrentMonthEnding
        {
            get => _currentMonthEnding;
            set
            {
                _currentMonthEnding = value;
                if (_setCurrentDateText)
                {
                    CurrentMonthEndingText = CurrentMonthEnding.ToString("MMMM yyyy");
                }
            }
        }

        private string _currentMonthEndingText;

        public string CurrentMonthEndingText
        {
            get => _currentMonthEndingText;
            set
            {
                if (_currentMonthEndingText == value)
                    return;

                _currentMonthEndingText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public IMainView View { get; private set; }

        public RelayCommand CloseAppCommand { get; private set; }
        public RelayCommand PreviousMonthCommand { get; }
        public RelayCommand NextMonthCommand { get; }
        public RelayCommand ChangeHouseholdCommand { get; }
        public RelayCommand ManageBudgetCommand { get; }
        public RelayCommand ManageBankAccountsCommand { get; }
        public RelayCommand SyncPhoneCommand { get; }
        public RelayCommand AdvancedFindCommand { get; }
        public RelayCommand UpgradeCommand { get; }
        public RelayCommand AboutCommand { get; }
        public RelayCommand ChangeDateCommand { get; }

        public void SetLookupIndex(int index)
        {
        }

        public int PageSize { get; } = 20;
        public LookupSearchTypes SearchType { get; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; } = string.Empty;
        public int SelectedIndex { get; }

        public DateTime CurrentMonth { get; private set; }

        public List<BudgetItemViewModel> BudgetItemViewModels { get; } = new List<BudgetItemViewModel>();

        public List<BankAccountViewModel> BankAccountViewModels { get; } = new List<BankAccountViewModel>();

        public StatsViewModel StatsViewModel { get; internal set; }

        private LookupFormulaColumnDefinition _projectedMonthToDateColumnDefinition;
        private LookupFormulaColumnDefinition _actualMonthToDateColumnDefinition;
        private LookupFormulaColumnDefinition _monthlyAmountDifferrenceColumnDefinition;
        private ChartData _initialBudgetChartData;
        private ChartData _initialActualChartData;
        private ChartData _activeChartData;
        private List<int> _budgetItems = new List<int>();
        private bool _setCurrentDateText = true;

        public MainViewModel()
        {
            CurrentMonthEnding = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

            PreviousMonthCommand = new RelayCommand(GotoPreviousMonth);
            NextMonthCommand = new RelayCommand(GotoNextMonth);
            ChangeHouseholdCommand = new RelayCommand(ChangeHousehold);
            ManageBudgetCommand = new RelayCommand(ManageBudget);
            ManageBankAccountsCommand = new RelayCommand(ManageBankAccounts);
            ChangeDateCommand = new RelayCommand(ChangeDate);
            AdvancedFindCommand = new RelayCommand(LaunchAdvancedFind);
            CloseAppCommand = new RelayCommand(CloseApp);
            SyncPhoneCommand = new RelayCommand(SyncPhone);
            UpgradeCommand = new RelayCommand(() =>
            {
                if (!View.UpgradeVersion())
                {
                    var message = "You are already on the latest version.";
                    var caption = "Upgrade Not Necessary";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
                }
            });
            AboutCommand = new RelayCommand((() =>
            {
                View.ShowAbout();
            }));
        }

        public void OnViewLoaded(IMainView view)
        {
            View = view;
            AppGlobals.MainViewModel = this;

            var loadVm = true;
            if (AppGlobals.LoggedInHousehold == null)
            {
                //View.ShowChart(false);
                loadVm = View.ChangeHousehold(true);
            }

            if (loadVm)
            {
                SetStartupView();
                //if (StatsDataExists())
                //{
                //    ShowStats(true);
                //}
            }
        }

        public void ShowStats(bool setFocus)
        {
            var exists = StatsDataExists();
            if (exists)
            {
                View.ShowStatsTab(true, setFocus);
            }
            else
            {
                View.ShowStatsTab(false, false);
                StatsViewModel = null;
            }
        }

        public bool StatsDataExists()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var regTable = context.GetTable<BankAccountRegisterItem>();
            var historyTable = context.GetTable<History>();
            var exists = regTable.Any() || historyTable.Any();
            return exists;
        }

        private void SetStartupView()
        {
            var look = AppGlobals.LookupContext;
            var currentBudgetMonth = AppGlobals.DataRepository.GetMaxMonthBudgetPeriodHistory();
            if (currentBudgetMonth == null)
            {
                var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                    DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));

                SetCurrentMonthEnding(date, false);
                CurrentMonth = date;
                //BudgetLookupDefinition = CreateBudgetLookupDefinition(true);
                RefreshView();
            }
            else
            {
                SetCurrentMonthEnding(currentBudgetMonth.PeriodEndingDate, false);
                CurrentMonth = currentBudgetMonth.PeriodEndingDate;
                //BankLookupDefinition = CreateBankLookupDefinition();
                //BudgetLookupDefinition = CreateBudgetLookupDefinition(true);
                RefreshView();
            }
        }


        private void ChangeHousehold()
        {
            //View.ShowChart(false);
            if (View.ChangeHousehold(false))
            {
                StatsViewModel = null;
                SetStartupView();
            }
        }

        public void RefreshView()
        {
            if (AppGlobals.UnitTesting)
            {
                return;
            }

            if (StatsViewModel == null && !View.StatsTabExists())
            {
                if (StatsDataExists())
                {
                    View.ShowStatsTab(true, false);
                }
            }
            else
            {
                if (StatsDataExists())
                {
                    if (StatsViewModel != null) 
                        StatsViewModel.RefreshView();
                }
                else
                {
                    View.ShowStatsTab(false, false);
                    StatsViewModel = null;
                }
            }
            //BudgetLookupDefinition.HasFromFormula(CreateBudgetLookupDefinitionFormula());
            var budgetTotals = AppGlobals.DataRepository.GetBudgetTotals(CurrentMonthEnding,
                GetPeriodEndDate(CurrentMonthEnding.AddMonths(-1)), 
                GetPeriodEndDate(CurrentMonthEnding.AddMonths(1)));


            PreviousMonthCommand.IsEnabled = budgetTotals.PreviousMonthHasValues;
            NextMonthCommand.IsEnabled = budgetTotals.NextMonthHasValues;

            foreach (var bankAccountViewModel in BankAccountViewModels)
            {
                bankAccountViewModel.CalculateTotals();
            }
        }



        private void ManageBudget()
        {
            View.ManageBudget();
            RefreshView();
        }

        private void ManageBankAccounts()
        {
            View.ManageBankAccounts();
        }

        private void GotoPreviousMonth()
        {
            SetCurrentMonthEnding(CurrentMonthEnding.AddMonths(-1));
        }

        private void GotoNextMonth()
        {
            SetCurrentMonthEnding(CurrentMonthEnding.AddMonths(1));
        }

        private void LaunchAdvancedFind()
        {
            View.LaunchAdvancedFind();
        }

        public void SetCurrentMonthEnding(DateTime value, bool refreshView = true)
        {
            CurrentMonthEnding = GetPeriodEndDate(value);

            if (refreshView)
                RefreshView();
        }

        private void CloseApp()
        {
            View.CloseApp();
        }

        private void SyncPhone()
        {
            var systemMaster = AppGlobals.DataRepository.GetSystemMaster();
            var loginInput = new Login
            {
                UserName = systemMaster.PhoneLogin,
                Password = systemMaster.PhonePassword,
                Guid = Guid.NewGuid().ToString()
            };
            var phoneLogin = View.ShowPhoneSync(loginInput);
            if (phoneLogin != null)
            {
                systemMaster.PhoneLogin = phoneLogin.UserName;
                systemMaster.PhonePassword = phoneLogin.Password;
                AppGlobals.DataRepository.SaveSystemMaster(systemMaster);
            }
        }

        //private List<BudgetData> GetPhoneBudgetData()
        //{

        //}

        private static DateTime GetPeriodEndDate(DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }


        private void ChangeDate()
        {
            var newDate = new DateTime(
                CurrentMonthEnding.Year
                , CurrentMonthEnding.Month
                , CurrentMonthEnding.Day);

            var changeDateData = new ChangeDateData();
            changeDateData.NewDate = newDate;
            View.GetChangeDate(changeDateData);
            if (changeDateData.DialogResult)
            {
                SetCurrentMonthEnding(changeDateData.NewDate);
            }
        }

        public void SetTabDestination(LookupDefinitionBase lookup)
        {
            View.SetTabDestination(lookup);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
