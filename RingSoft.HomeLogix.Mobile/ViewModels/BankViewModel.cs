using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.PhoneModel;

namespace RingSoft.HomeLogix.Mobile.ViewModels
{
    public class BankViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BankData> _bankData;

        public ObservableCollection<BankData> BankData
        {
            get => _bankData;
            set
            {
                if (_bankData == value)
                {
                    return;
                }
                _bankData = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<BankData> RegisterCommand { get; set; }

        public BankViewModel()
        {
            RegisterCommand = new RelayCommand<BankData>(OnRegisterClicked);
        }

        public void Initialize()
        {
            var file = "BankData.json";
            var jsonText = string.Empty;
            if (MainViewModel.DownloadWebText(ref jsonText, file, true))
            {
                var bankData = new List<BankData>();
                bankData = JsonConvert.DeserializeObject<List<BankData>>(jsonText);
                BankData = new ObservableCollection<BankData>(bankData);
            }

        }

        private void OnRegisterClicked(BankData bankData)
        {

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
