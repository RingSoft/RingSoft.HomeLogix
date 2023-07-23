using Newtonsoft.Json;
using RingSoft.HomeLogix.Library.PhoneModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        private double _budgetIncome;

        public double BudgetIncome
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

        private double _budgetExpenses;

        public double BudgetExpenses
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

        private double _budgetDifference;

        public double BudgetDifference
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

        private double _actualIncome;

        public double ActualIncome
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

        private double _actualExpenses;

        public double ActualExpenses
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

        private double _actualDifference;

        public double ActualDifference
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

        private double _incomeDifference;

        public double IncomeDifference
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

        private double _expenseDifference;

        public double ExpenseDifference
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


        private double _difference;

        public double Difference
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

        private double _ytdIncome;

        public double YtdIncome
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

        private double _ytdExpenses;

        public double YtdExpenses
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

        private double _ytdDifference;

        public double YtdDifference
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
                Difference = ActualDifference - BudgetDifference;

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
