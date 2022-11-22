using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Mobile.ViewModels;
using Newtonsoft.Json;
using RingSoft.HomeLogix.Library.PhoneModel;

namespace RingSoft.HomeLogix.Mobile.ViewModels
{
    public class BudgetsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BudgetData> _budgetData;

        public ObservableCollection<BudgetData> BudgetData
        {
            get => _budgetData;
            set
            {
                if (_budgetData == value)
                {
                    return;
                }
                _budgetData = value;
                OnPropertyChanged();
            }
        }

        private DateTime _monthEndDate;

        public DateTime MonthEndDate
        {
            get => _monthEndDate;
            set
            {
                if (_monthEndDate == value)
                {
                    return;
                }
                _monthEndDate = value;
                OnPropertyChanged();
            }
        }

        private decimal _budgetIncome;

        public decimal BudgetIncome
        {
            get => _budgetIncome;
            set
            {
                if (_budgetIncome == value)
                {
                    return;
                }
                _budgetIncome = value;
                OnPropertyChanged();
            }
        }

        private decimal _budgetExpenses;

        public decimal BudgetExpenses
        {
            get => _budgetExpenses;
            set
            {
                if (_budgetExpenses == value)
                {
                    return;
                }
                _budgetExpenses = value;
                OnPropertyChanged();
            }
        }

        private decimal _budgetDifference;

        public decimal BudgetDifference
        {
            get => _budgetDifference;
            set
            {
                if (_budgetDifference == value)
                {
                    return;
                }
                _budgetDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _actualIncome;

        public decimal ActualIncome
        {
            get => _actualIncome;
            set
            {
                if (_actualIncome == value)
                {
                    return;
                }
                _actualIncome = value;
                OnPropertyChanged();
            }
        }

        private decimal _actualExpenses;

        public decimal ActualExpenses
        {
            get => _actualExpenses;
            set
            {
                if (_actualExpenses == value)
                {
                    return;
                }
                _actualExpenses = value;
                OnPropertyChanged();
            }
        }

        private decimal _actualDifference;

        public decimal ActualDifference
        {
            get => _actualDifference;
            set
            {
                if (_actualDifference == value)
                {
                    return;
                }
                _actualDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _incomeDifference;

        public decimal IncomeDifference
        {
            get => _incomeDifference;
            set
            {
                if (_incomeDifference == value)
                {
                    return;
                }
                _incomeDifference = value;
                OnPropertyChanged();
            }
        }

        private decimal _expenseDifference;

        public decimal ExpenseDifference
        {
            get => _expenseDifference;
            set
            {
                if (_expenseDifference == value)
                {
                    return;
                }
                _expenseDifference = value;
                OnPropertyChanged();
            }
        }


        private decimal _difference;

        public decimal Difference
        {
            get => _difference;
            set
            {
                if (_difference == value)
                {
                    return;
                }
                _difference = value;
                OnPropertyChanged();
            }
        }

        private decimal _ytdIncome;

        public decimal YtdIncome
        {
            get => _ytdIncome;
            set
            {
                if (_ytdIncome == value)
                {
                    return;
                }
                _ytdIncome = value;
                OnPropertyChanged();
            }
        }

        private decimal _ytdExpenses;

        public decimal YtdExpenses
        {
            get => _ytdExpenses;
            set
            {
                if (_ytdExpenses == value)
                {
                    return;
                }
                _ytdExpenses = value;
                OnPropertyChanged();
            }
        }

        private decimal _ytdDifference;

        public decimal YtdDifference
        {
            get => _ytdDifference;
            set
            {
                if (_ytdDifference == value)
                {
                    return;
                }
                _ytdDifference = value;
                OnPropertyChanged();
            }
        }


        public void Initialize(bool current)
        {
            var file = "CurrentMonthBudget.json";
            if (!current)
            {
                file = "PreviousMonthBudget.json";
            }

            var jsonText = string.Empty;
            if (MainViewModel.DownloadWebText(ref jsonText, file, true))
            {
                var budgetData = new List<BudgetData>();
                budgetData = JsonConvert.DeserializeObject<List<BudgetData>>(jsonText);
                BudgetData = new ObservableCollection<BudgetData>(budgetData);
            }

            file = "BudgetStats.json";
            jsonText = string.Empty;
            if (MainViewModel.DownloadWebText(ref jsonText, file, true))
            {
                var budgetStats = new List<BudgetStatistics>();
                budgetStats = JsonConvert.DeserializeObject<List<BudgetStatistics>>(jsonText);
                var itemStats = budgetStats.FirstOrDefault(p => p.Type == StatisticsType.Current);
                if (!current)
                {
                    itemStats = budgetStats.FirstOrDefault(p => p.Type == StatisticsType.Previous);
                }
                MonthEndDate = itemStats.MonthEnding;

                BudgetIncome = itemStats.BudgetIncome;
                BudgetExpenses = itemStats.BudgetExpenses;
                BudgetDifference = BudgetIncome - BudgetExpenses;

                ActualIncome = itemStats.ActualIncome;
                ActualExpenses = itemStats.ActualExpenses;
                ActualDifference = ActualIncome - ActualExpenses;

                IncomeDifference = ActualIncome - BudgetIncome;
                ExpenseDifference = BudgetExpenses - ActualExpenses;
                Difference = Math.Abs(IncomeDifference) - Math.Abs(ExpenseDifference);

                YtdIncome = itemStats.YtdIncome;
                YtdExpenses = itemStats.YtdExpenses;
                YtdDifference = YtdIncome - YtdExpenses;
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
