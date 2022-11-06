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
    public interface IBudgetsPageView
    {
        void ShowMessage(string message, string caption);
    }

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
                //OnPropertyChanged();
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


        public IBudgetsPageView View { get; set; }

        public void Initialize(IBudgetsPageView view, bool current)
        {
            View = view;
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
