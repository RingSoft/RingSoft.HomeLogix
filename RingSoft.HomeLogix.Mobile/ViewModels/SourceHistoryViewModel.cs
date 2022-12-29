using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.HomeLogix.Library.PhoneModel;

namespace RingSoft.HomeLogix.Mobile.ViewModels
{
    public class SourceHistoryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SourceHistoryData> _historyDataList;

        public ObservableCollection<SourceHistoryData> HistoryDataList
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

        public void Initialize(HistoryData historyData)
        {
            var sourceHistoryData = MobileGlobals.MainViewModel.SourceHistoryData
                .Where(p => p.HistoryId == historyData.HistoryId)
                .OrderByDescending(p => p.Date);
            HistoryDataList = new ObservableCollection<SourceHistoryData>(sourceHistoryData);
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
