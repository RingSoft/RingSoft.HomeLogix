using Newtonsoft.Json;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.PhoneModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.App.Interop;
using RingSoft.HomeLogix.MobileInterop;

namespace RingSoft.HomeLogix.Mobile.ViewModels
{
    public interface IComputerSyncView
    {
        void ClosePage();
    }
    public class ComputerSyncViewModel : INotifyPropertyChanged
    {
        private string _userName;

        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName == value)
                {
                    return;
                }
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value)
                {
                    return;
                }
                _password = value;
                OnPropertyChanged();
            }
        }

        IComputerSyncView View { get; set; }

        public RelayCommand SaveCommand { get; set; }

        private List<Login> _logins;

        public ComputerSyncViewModel()
        {
            SaveCommand = new RelayCommand(Save);
        }

        public void Initialize(IComputerSyncView view)
        {
            View = view;
            var loginsText = InteropGlobals.GetWebText("Logins.json");
            _logins = JsonConvert.DeserializeObject<List<Login>>(loginsText);
            UserName = MobileGlobals.GetProperty("UserName");
            Password = MobileGlobals.GetProperty("Password");
        }

        private void Save()
        {
            var login = _logins.FirstOrDefault(p => string.Equals(p .UserName, UserName, StringComparison.CurrentCultureIgnoreCase));
            if (login == null)
            {
                var message = "User Name is incorrect.";
                var caption = "Invalid Entry";
                MobileGlobals.MainViewModel.View.ShowMessage(message, caption);
                return;
            }

            var encryptedPassword = login.Password;
            if (encryptedPassword.Decrypt() != Password)
            {
                var message = "Password is incorrect.";
                var caption = "Invalid Entry";
                MobileGlobals.MainViewModel.View.ShowMessage(message, caption);
                return;
            }

            MobileGlobals.SetProperty("UserName", UserName);
            MobileGlobals.SetProperty("Password", Password);
            MobileGlobals.SetProperty("Guid", login.Guid);
            View.ClosePage();
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
