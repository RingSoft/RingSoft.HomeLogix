using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RingSoft.App.Library;
using RingSoft.DbLookup.Lookup;
using RingSoft.HomeLogix.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using RingSoft.HomeLogix.DataAccess;

namespace RingSoft.HomeLogix.Library.ViewModels.Main
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        #region Properties

        private LookupDefinition<MainBudgetLookup, MainBudget> _budgetLookupDefinition;

        public LookupDefinition<MainBudgetLookup, MainBudget> BudgetLookupDefinition
        {
            get => _budgetLookupDefinition;
            set
            {
                if (_budgetLookupDefinition == value)
                    return;

                _budgetLookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private LookupDefinition<MainBankLookup, BankAccount> _bankLookupDefinition;

        public LookupDefinition<MainBankLookup, BankAccount> BankLookupDefinition
        {
            get => _bankLookupDefinition;
            set
            {
                if (_bankLookupDefinition == value)
                {
                    return;
                }

                _bankLookupDefinition = value;
                OnPropertyChanged();
            }
        }


        private double _totalProjectedMonthlyIncome;

        public double TotalProjectedMonthlyIncome
        {
            get => _totalProjectedMonthlyIncome;
            set
            {
                if (_totalProjectedMonthlyIncome == value)
                    return;

                _totalProjectedMonthlyIncome = value;
                OnPropertyChanged();
            }
        }

        private double _totalProjectedMonthlyExpenses;

        public double TotalProjectedMonthlyExpenses
        {
            get => _totalProjectedMonthlyExpenses;
            set
            {
                if (_totalProjectedMonthlyExpenses == value)
                    return;

                _totalProjectedMonthlyExpenses = value;
                OnPropertyChanged();
            }
        }

        private double _totalBudgetMonthlyNetIncome;

        public double TotalBudgetMonthlyNetIncome
        {
            get => _totalBudgetMonthlyNetIncome;
            set
            {
                if (_totalBudgetMonthlyNetIncome == value)
                    return;

                _totalBudgetMonthlyNetIncome = value;
                OnPropertyChanged();
            }
        }

        private double _totalActualMonthlyIncome;

        public double TotalActualMonthlyIncome
        {
            get => _totalActualMonthlyIncome;
            set
            {
                if (_totalActualMonthlyIncome == value)
                    return;

                _totalActualMonthlyIncome = value;
                OnPropertyChanged();
            }
        }

        private double _totalActualMonthlyExpenses;

        public double TotalActualMonthlyExpenses
        {
            get => _totalActualMonthlyExpenses;
            set
            {
                if (_totalActualMonthlyExpenses == value)
                    return;

                _totalActualMonthlyExpenses = value;
                OnPropertyChanged();
            }
        }

        private double _totalActualMonthlyNetIncome;

        public double TotalActualMonthlyNetIncome
        {
            get => _totalActualMonthlyNetIncome;
            set
            {
                if (_totalActualMonthlyNetIncome == value)
                    return;

                _totalActualMonthlyNetIncome = value;
                OnPropertyChanged();
            }
        }

        private double _totalMonthlyIncomeDifference;

        public double TotalMonthlyIncomeDifference
        {
            get => _totalMonthlyIncomeDifference;
            set
            {
                if (_totalMonthlyIncomeDifference == value)
                    return;

                _totalMonthlyIncomeDifference = value;
                OnPropertyChanged();
            }
        }

        private double _totalMonthlyExpensesDifference;

        public double TotalMonthlyExpensesDifference
        {
            get => _totalMonthlyExpensesDifference;
            set
            {
                if (_totalMonthlyExpensesDifference == value)
                    return;

                _totalMonthlyExpensesDifference = value;
                OnPropertyChanged();
            }
        }

        private double _totalMonthlyNetIncomeDifference;

        public double TotalMonthlyNetIncomeDifference
        {
            get => _totalMonthlyNetIncomeDifference;
            set
            {
                if (_totalMonthlyNetIncomeDifference == value)
                    return;

                _totalMonthlyNetIncomeDifference = value;
                OnPropertyChanged();
            }
        }

        private double _yearToDateIncome;

        public double YearToDateIncome
        {
            get => _yearToDateIncome;
            set
            {
                if (_yearToDateIncome == value)
                    return;

                _yearToDateIncome = value;
                OnPropertyChanged();
            }
        }

        private double _yearToDateExpenses;

        public double YearToDateExpenses
        {
            get => _yearToDateExpenses;
            set
            {
                if (_yearToDateExpenses == value)
                    return;

                _yearToDateExpenses = value;
                OnPropertyChanged();
            }
        }

        private double _yearToDateNetIncome;

        public double YearToDateNetIncome
        {
            get => _yearToDateNetIncome;
            set
            {
                if (_yearToDateNetIncome == value)
                    return;

                _yearToDateNetIncome = value;
                OnPropertyChanged();
            }
        }

        private ChartData _budgetChartData;
        public ChartData BudgetChartData
        {
            get => _budgetChartData;
            set
            {
                if (_budgetChartData == value)
                {
                    return;
                }
                _budgetChartData = value;
                OnPropertyChanged();
            }
        }

        private ChartData _actualChartData;
        public ChartData ActualChartData
        {
            get => _actualChartData;
            set
            {
                if (_actualChartData == value)
                {
                    return;
                }
                _actualChartData = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private DateTime _currentMonthEnding;
        private ChartData _initialBudgetChartData;
        private ChartData _initialActualChartData;
        private ChartData _activeChartData;
        private List<int> _budgetItems = new List<int>();

        public void OnViewLoaded()
        {
            AppGlobals.MainViewModel.StatsViewModel = this;
            BankLookupDefinition = CreateBankLookupDefinition();
            BudgetLookupDefinition = AppGlobals.LookupContext.MainBudgetLookup.Clone();
            RefreshView();
        }

        private LookupDefinition<MainBankLookup, BankAccount> CreateBankLookupDefinition()
        {
            var bankLookupDefinition =
                new LookupDefinition<MainBankLookup, BankAccount>(AppGlobals.LookupContext.BankAccounts);

            bankLookupDefinition.AddHiddenColumn(p => p.BankId, p => p.Id);

            bankLookupDefinition.AddHiddenColumn(p => p.AccountType, p => p.AccountType);

            bankLookupDefinition.AddVisibleColumnDefinition(p => p.Description,
                p => p.Description);
            bankLookupDefinition.AddVisibleColumnDefinition(p => p.CurrentBalance,
                p => p.CurrentBalance);
            bankLookupDefinition.AddVisibleColumnDefinition(p => p.ProjectedLowestBalance,
                p => p.ProjectedLowestBalanceAmount);
            bankLookupDefinition.AddVisibleColumnDefinition(p => p.ProjectedLowestBalanceDate,
                p => p.ProjectedLowestBalanceDate);
            return bankLookupDefinition;
        }

        public void RefreshView(IAppProcedure? procedure = null)
        {
            _currentMonthEnding = AppGlobals.MainViewModel.CurrentMonthEnding;
            if (AppGlobals.UnitTesting)
            {
                return;
            }
            //BudgetLookupDefinition.HasFromFormula(CreateBudgetLookupDefinitionFormula());

            if (procedure != null)
            {
                procedure.SplashWindow.SetProgress("Setting up main budget lookup.");
            }

            var context = AppGlobals.DataRepository.GetDataContext();
            var mainTable = context.GetTable<MainBudget>();
            context.RemoveRange(mainTable);
            if (context.Commit("Clearing Main Budget Table"))
            {
                var budgetItems = GetBudgetItems(context, procedure);

                context.AddRange(budgetItems);
                context.Commit("Adding Main Budgets");
            }
            BudgetLookupDefinition.SetCommand(new LookupCommand(LookupCommands.Refresh));
            BankLookupDefinition.SetCommand(new LookupCommand(LookupCommands.Refresh));

            var budgetTotals = AppGlobals.DataRepository.GetBudgetTotals(_currentMonthEnding,
                GetPeriodEndDate(_currentMonthEnding.AddMonths(-1)),
                GetPeriodEndDate(_currentMonthEnding.AddMonths(1)));

            TotalProjectedMonthlyIncome = budgetTotals.TotalProjectedMonthlyIncome;
            TotalProjectedMonthlyExpenses = budgetTotals.TotalProjectedMonthlyExpenses;
            TotalBudgetMonthlyNetIncome = TotalProjectedMonthlyIncome - TotalProjectedMonthlyExpenses;

            TotalActualMonthlyIncome = budgetTotals.TotalActualMonthlyIncome;
            TotalActualMonthlyExpenses = budgetTotals.TotalActualMonthlyExpenses;
            TotalActualMonthlyNetIncome = TotalActualMonthlyIncome - TotalActualMonthlyExpenses;

            TotalMonthlyIncomeDifference = TotalActualMonthlyIncome - TotalProjectedMonthlyIncome;
            TotalMonthlyExpensesDifference = TotalProjectedMonthlyExpenses - TotalActualMonthlyExpenses;
            TotalMonthlyNetIncomeDifference = TotalActualMonthlyNetIncome - TotalBudgetMonthlyNetIncome;

            YearToDateIncome = budgetTotals.YearToDateIncome;
            YearToDateExpenses = budgetTotals.YearToDateExpenses;
            YearToDateNetIncome = YearToDateIncome - YearToDateExpenses;


            MakeBudgetChartData();
            MakeActualChartData();

        }

        private static DateTime GetPeriodEndDate(DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }


        public List<MainBudget> GetBudgetItems(IDbContext context, IAppProcedure? procedure = null)
        {
            var budgetItems = new List<MainBudget>();

            var periodHistoryTable = context.GetTable<BudgetPeriodHistory>();
            var periodHistory = periodHistoryTable
                .Include(p => p.BudgetItem)
                .Where(p => p.PeriodType == (byte)PeriodHistoryTypes.Monthly
                            && p.BudgetItem.Type != (byte)BudgetItemTypes.Transfer
                            && p.PeriodEndingDate.Month == _currentMonthEnding.Month
                            && p.PeriodEndingDate.Year == _currentMonthEnding.Year)
                .OrderBy(p => p.BudgetItem.Description);

            var count = periodHistory.Count();
            var index = 0;

            foreach (var budgetPeriodHistory in periodHistory)
            {
                index++;
                if (procedure != null)
                {
                    procedure.SplashWindow.SetProgress($"Processing budget {index} / {count}.");
                }

                var budgetItem = budgetItems
                    .FirstOrDefault(p => p.BudgetItemId == budgetPeriodHistory.BudgetItemId);

                if (budgetItem == null)
                {
                    budgetItem = new MainBudget();
                    budgetItem.BudgetItemId = budgetPeriodHistory.BudgetItemId;
                    budgetItem.ItemType = (byte)budgetPeriodHistory.BudgetItem.Type;
                    budgetItem.BudgetItem = budgetPeriodHistory.BudgetItem;
                    budgetItems.Add(budgetItem);
                }

                budgetItem.BudgetAmount += (double)budgetPeriodHistory.ProjectedAmount;
                budgetItem.ActualAmount += (double)budgetPeriodHistory.ActualAmount;
            }

            var registerTable = context.GetTable<BankAccountRegisterItem>();
            var monthlyRegisterItems = registerTable
                .Include(p => p.BudgetItem)
                .Where(p => p.BudgetItem != null
                            && p.BudgetItem.Type != (byte)BudgetItemTypes.Transfer
                            && p.ItemDate.Month == _currentMonthEnding.Month
                            && p.ItemDate.Year == _currentMonthEnding.Year)
                .OrderBy(p => p.BudgetItem.Description);
            foreach (var bankAccountRegisterItem in monthlyRegisterItems)
            {
                var budgetItem = budgetItems
                    .FirstOrDefault(p => p.BudgetItemId == bankAccountRegisterItem.BudgetItemId);

                if (budgetItem == null)
                {
                    budgetItem = new MainBudget();
                    budgetItem.BudgetItemId = bankAccountRegisterItem.BudgetItemId;
                    budgetItem.ItemType = (byte)bankAccountRegisterItem.BudgetItem.Type;
                    budgetItem.BudgetItem = bankAccountRegisterItem.BudgetItem;
                    budgetItems.Add(budgetItem);
                }

                budgetItem.BudgetAmount += Math.Abs((double)bankAccountRegisterItem.ProjectedAmount);
                budgetItem.ActualAmount += Math.Abs((double)bankAccountRegisterItem.ActualAmount.GetValueOrDefault()
                                                    + (double)bankAccountRegisterItem.ProjectedAmount);
            }

            return budgetItems;
        }

        private void MakeBudgetChartData()
        {
            _initialBudgetChartData = new ChartData();
            _initialBudgetChartData.Title = "Budget Expenses";

            var lookupDefinition = BudgetLookupDefinition.Clone();
            lookupDefinition.InitialSortColumnDefinition =
                lookupDefinition.GetColumnDefinition(p => p.BudgetAmount);

            _activeChartData = _initialBudgetChartData;

            MakeChartData();

            BudgetChartData = _initialBudgetChartData;
        }

        private void MakeActualChartData()
        {
            _initialActualChartData = new ChartData();
            _initialActualChartData.Title = "Actual Expenses";

            var lookupDefinition = BudgetLookupDefinition.Clone();
            lookupDefinition.InitialSortColumnDefinition =
                lookupDefinition.GetColumnDefinition(p => p.ActualAmount);

            _activeChartData = _initialActualChartData;

            MakeChartData();

            ActualChartData = _initialActualChartData;
        }


        private void MakeChartData()
        {
            var context = AppGlobals.DataRepository.GetDataContext();
            var budgetItems = GetBudgetItems(context);

            _budgetItems.Clear();
            MakeChartData(budgetItems);
        }

        private void MakeChartData(List<MainBudget> budgets)
        {
            var sortedBudgets = budgets;
            if (_activeChartData == _initialBudgetChartData)
            {
                sortedBudgets = budgets.OrderByDescending(p => p.BudgetAmount).ToList();
            }
            else
            {
                sortedBudgets = budgets.OrderByDescending(p => p.ActualAmount).ToList();
            }
            foreach (var mainBudget in sortedBudgets)
            {
                var itemType = (BudgetItemTypes)mainBudget.ItemType;
                if (itemType == BudgetItemTypes.Expense)
                {
                    //if (!_budgetItems.Contains(mainLookup.BudgetId))
                    {
                        _budgetItems.Add(mainBudget.BudgetItemId);
                        if (_activeChartData == _initialBudgetChartData)
                        {
                            _activeChartData.Items.Add(new ChartDataItem
                            {
                                Name = mainBudget.BudgetItem.Description,
                                Value = (double)mainBudget.BudgetAmount
                            });
                        }
                        else if (_activeChartData == _initialActualChartData)
                        {
                            _activeChartData.Items.Add(new ChartDataItem
                            {
                                Name = mainBudget.BudgetItem.Description,
                                Value = (double)mainBudget.ActualAmount
                            });
                        }
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
