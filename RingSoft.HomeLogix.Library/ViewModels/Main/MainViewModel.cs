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
    public class UpgradeBankData
    {
        public BankAccount BankAccount { get; set; }
        public int PayCCDay { get; set; }
        public BudgetItem PayCCBudgetItem { get; set; }
        public bool DialogResult { get; set; }
        public bool Recalculate { get; set; }
        public BankAccountRegisterItem FirstCCRegisterItem { get; set; }
        public bool Processed { get; set; }
    }
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
                CheckNewStartup();
                //if (StatsDataExists())
                //{
                //    ShowStats(true);
                //}
            }
        }

        private void CheckNewStartup()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<BankAccountRegisterItem>();
            if (!table.Any())
            {
                var message = "Welcome to RingSoft HomeLogix!  Start by creating Budget Items.  When you're done creating all your Budget Items, you can click Manage Bank Accounts and generate bank account future register items from your budget to see what your future bank account balances will be.";
                var caption = "Welcome to RingSoft HomeLogix!";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);
                ManageBudget();
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
                var context = SystemGlobals.DataRepository.GetDataContext();
                var table = context.GetTable<BankAccountRegisterItem>();
                var regItem = table.OrderBy(o => o.ItemDate)
                    .FirstOrDefault();

                DateTime date = DateTime.MinValue;
                if (regItem != null)
                {
                    date = regItem.ItemDate;
                }
                else
                {
                    date = new DateTime(DateTime.Today.Year, DateTime.Today.Month,
                        DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
                }

                ChangeCurrentDate(date);
            }
            else
            {
                SetCurrentMonthEnding(currentBudgetMonth.PeriodEndingDate, false);
                CurrentMonth = currentBudgetMonth.PeriodEndingDate;
                //BankLookupDefinition = CreateBankLookupDefinition();
                //BudgetLookupDefinition = CreateBudgetLookupDefinition(true);
                RefreshView();
            }
            CheckCCUpgrade();
        }

        private async void CheckCCUpgrade()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var regTable = context.GetTable<BankAccountRegisterItem>();

            if (!regTable.Any())
            {
                return;
            }

            var banks = new List<BankAccount>();
            var budgets = new List<BudgetItem>();
            var bankUpgradeDatas = new List<UpgradeBankData>();

            var ccRegItems = regTable
                .Include(p => p.BudgetItem)
                .Include(p => p.BankAccount)
                .OrderBy(p => p.BankAccountId)
                .ThenBy(p => p.ItemDate)
                .Where(p => p.BudgetItem.Type == (byte)BudgetItemTypes.Transfer
                            && p.BudgetItem.Amount == 0
                            && p.BudgetItem.StartingDate != null
                            && p.BankAccount.AccountType == (byte)BankAccountTypes.CreditCard
                            && p.RegisterPayCCType == (byte)RegisterPayCCTypes.None);

            if (ccRegItems.Any())
            {
                var message =
                    "It appears that you have transfer budget items with credit card payment that were created in a previous version of HomeLogix.  " +
                    "To take advantage of the new credit card payment features, these budget items need to be upgraded.  " +
                    "Click Yes to upgrade these budget items now.";

                var caption = "Upgrade Credit Card Budget Items";

                if (await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption) ==
                    MessageBoxButtonsResult.Yes)
                {
                    foreach (var regItem in ccRegItems)
                    {
                        if (!banks.Contains(regItem.BankAccount))
                        {
                            banks.Add(regItem.BankAccount);
                            var upgradeData = new UpgradeBankData
                            {
                                BankAccount = regItem.BankAccount,
                                PayCCDay = regItem.ItemDate.Day,
                                PayCCBudgetItem = regItem.BudgetItem,
                                FirstCCRegisterItem = regItem,
                            };
                            bankUpgradeDatas.Add(upgradeData);
                        }

                        if (!budgets.Contains(regItem.BudgetItem))
                        {
                            budgets.Add(regItem.BudgetItem);
                        }
                    }

                    var processBudgets = true;
                    foreach (var upgradeData in bankUpgradeDatas)
                    {
                        var primaryKey =
                            AppGlobals.LookupContext.BankAccounts.GetPrimaryKeyValueFromEntity(upgradeData.BankAccount);
                        var vmInput = new ViewModelInput
                        {
                            UpgradeBankData = upgradeData,
                        };
                        SystemGlobals.TableRegistry.ShowEditAddOnTheFly(primaryKey, vmInput);
                        if (!upgradeData.DialogResult || !upgradeData.Recalculate)
                        {
                            processBudgets = false;
                        }
                        else
                        {
                            upgradeData.Processed = true;
                        }
                    }

                    if (processBudgets)
                    {
                        foreach (var budgetItem in budgets)
                        {
                            budgetItem.StartingDate = null;
                            context.SaveEntity(budgetItem, "Saving budget item");
                        }
                    }
                }
            }
        }

        public void ChangeCurrentDate(DateTime date)
        {
            SetCurrentMonthEnding(date, false);
            CurrentMonth = date;
            //BudgetLookupDefinition = CreateBudgetLookupDefinition(true);
            RefreshView();
        }


        private void ChangeHousehold()
        {
            //View.ShowChart(false);
            if (View.ChangeHousehold(false))
            {
                StatsViewModel = null;
                SetStartupView();
                CheckNewStartup();
            }
        }

        public void RefreshView(IAppProcedure? procedure = null)
        {
            if (procedure == null && !AppGlobals.UnitTesting)
            {
                procedure = RingSoftAppGlobals.CreateAppProcedure();
                procedure.DoAppProcedure += (sender, args) => { RefreshViewProcedure(procedure); };
                procedure.Start("Loading Main Screen");
                return;
            }
            RefreshViewProcedure(procedure);
        }

        private void RefreshViewProcedure(IAppProcedure? procedure = null)
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
                        StatsViewModel.RefreshView(procedure);
                }
                else
                {
                    View.ShowStatsTab(false, false);
                    StatsViewModel = null;
                }
            }

            if (procedure != null)
            {
                procedure.SplashWindow.SetProgress("Retrieving budget totals.");
            }
            //BudgetLookupDefinition.HasFromFormula(CreateBudgetLookupDefinitionFormula());
            var budgetTotals = AppGlobals.DataRepository.GetBudgetTotals(CurrentMonthEnding,
                GetPeriodEndDate(CurrentMonthEnding.AddMonths(-1)), 
                GetPeriodEndDate(CurrentMonthEnding.AddMonths(1)));


            PreviousMonthCommand.IsEnabled = budgetTotals.PreviousMonthHasValues;
            NextMonthCommand.IsEnabled = budgetTotals.NextMonthHasValues;

            foreach (var bankAccountViewModel in BankAccountViewModels)
            {
                bankAccountViewModel.CalculateTotals(procedure:procedure);
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
