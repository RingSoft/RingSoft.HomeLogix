using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.HomeLogix.Library.Annotations;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Households> _households;

        public ObservableCollection<Households> Households
        {
            get { return _households; }
            set
            {
                if (_households == value)
                    return;

                _households = value;
                OnPropertyChanged();
            }
        }

        private bool _isDefault;

        public bool IsDefault
        {
            get { return _isDefault; }
            set
            {
                if (_isDefault == value)
                    return;

                _isDefault = value;
                OnPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
