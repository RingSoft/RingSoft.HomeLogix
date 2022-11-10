using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.HomeLogix.Library.PhoneModel;

namespace RingSoft.HomeLogix.Mobile.ViewModels
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<HistoryData> _historyDataList;

        public ObservableCollection<HistoryData> HistoryDataList
        {
            get => _historyDataList;
            set
            {
                if (_historyDataList == value)
                {
                    return;
                }
                _historyDataList = value;
                OnPropertyChanged();
            }
        }

        public void Initialize(BudgetData budgetData)
        {
            var historyData = MobileGlobals.MainViewModel.HistoryData
                .Where(p => p.BudgetItemId == budgetData.BudgetItemId)
                .OrderByDescending(p => p.Date);
            HistoryDataList = new ObservableCollection<HistoryData>(historyData);
        }

        public void Initialize(BankData bankData)
        {
            var historyData = MobileGlobals.MainViewModel.HistoryData
                .Where(p => p.BankAccountId == bankData.BankId)
                .OrderByDescending(p => p.Date);
            HistoryDataList = new ObservableCollection<HistoryData>(historyData);
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
