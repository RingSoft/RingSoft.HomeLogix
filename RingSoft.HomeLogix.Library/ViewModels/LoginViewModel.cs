using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.MasterData;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public interface ILoginView
    {
        bool LoginToHousehold(Households household);

        Households ShowAddHousehold();

        string GetHouseholdDataFile();

        void CloseWindow();

        void ShutDownApplication();
    }

    public class LoginViewModel : INotifyPropertyChanged
    {
        public ILoginView View { get; private set; }

        private ObservableCollection<Households> _households;

        public ObservableCollection<Households> Households
        {
            get => _households;
            set
            {
                if (_households == value)
                    return;

                _households = value;
                OnPropertyChanged();
            }
        }

        private Households _selectedHousehold;

        public Households SelectedHousehold
        {
            get => _selectedHousehold;
            set
            {
                if (_selectedHousehold == value)
                    return;

                _selectedHousehold = value;
                OnPropertyChanged();
            }
        }


        private bool _isDefault;

        public bool IsDefault
        {
            get => _isDefault;
            set
            {
                if (_isDefault == value)
                    return;

                _isDefault = value;
                OnPropertyChanged();
            }
        }

        public bool DialogResult { get; private set; }

        public RelayCommand AddNewCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ConnectToDataFileCommand { get; }
        public RelayCommand LoginCommand { get; }
        public RelayCommand CancelCommand { get; }

        private bool _initialized;

        public LoginViewModel()
        {
            Households = new ObservableCollection<Households>();
            var dbHouseholds = MasterDbContext.GetHouseholds();

            foreach (var household in dbHouseholds)
            {
                Households.Add(household);
                if (AppGlobals.LoggedInHousehold != null && AppGlobals.LoggedInHousehold.Id == household.Id)
                {
                    household.Name = $"(Active) {household.Name}";
                    SelectedHousehold = household;
                }
            }

            if (SelectedHousehold == null && Households.Any())
                SelectedHousehold = Households[0];

            AddNewCommand = new RelayCommand(AddNewHouseHold);
            DeleteCommand = new RelayCommand(DeleteHousehold){IsEnabled = CanDeleteHousehold()};
            ConnectToDataFileCommand = new RelayCommand(ConnectToDataFile);
            LoginCommand = new RelayCommand(Login){IsEnabled = CanLogin()};
            CancelCommand = new RelayCommand(Cancel);

            _initialized = true;
        }

        public void OnViewLoaded(ILoginView loginView) => View = loginView;

        private bool CanLogin() => SelectedHousehold != null;

        private bool CanDeleteHousehold()
        {
            if (SelectedHousehold == null)
                return false;

            if (AppGlobals.LoggedInHousehold != null)
                return AppGlobals.LoggedInHousehold.Id != SelectedHousehold.Id;

            return true;
        }

        private void AddNewHouseHold()
        {
            var newHousehold = View.ShowAddHousehold();
            if (newHousehold != null)
            {
                Households.Add(newHousehold);
                SelectedHousehold = newHousehold;
            }
        }

        private void ConnectToDataFile()
        {
            var fileName = View.GetHouseholdDataFile();
            if (!fileName.IsNullOrEmpty())
            {
                var currentFilePath = AppGlobals.LookupContext.SqliteDataProcessor.FilePath;
                var currentFileName = AppGlobals.LookupContext.SqliteDataProcessor.FileName;

                var fileInfo = new FileInfo(fileName);
                AppGlobals.LookupContext.SqliteDataProcessor.FilePath = fileInfo.DirectoryName;
                AppGlobals.LookupContext.SqliteDataProcessor.FileName = fileInfo.Name;


            }
        }

        private void DeleteHousehold()
        {
            string message;
            if (SelectedHousehold.Id == 1)
            {
                message = "Deleting Demo household is not allowed.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid Operation",
                    RsMessageBoxIcons.Exclamation);
                return;
            }

            message = "Are you sure you wish to delete this household?";
            if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Confirm Delete") ==
                MessageBoxButtonsResult.Yes)
            {
                if (MasterDbContext.DeleteHousehold(SelectedHousehold.Id))
                {
                    Households.Remove(SelectedHousehold);
                    SelectedHousehold = null;
                }
            }
        }

        private void Login()
        {
            if (View.LoginToHousehold(SelectedHousehold))
            {
                AppGlobals.LoggedInHousehold = SelectedHousehold;
                DialogResult = true;
                View.CloseWindow();
            }
        }

        private void Cancel()
        {
            DialogResult = false;
            View.CloseWindow();
        }

        public bool DoCancelClose()
        {
            if (AppGlobals.LoggedInHousehold == null && !DialogResult)
            {
                var message = "Application will shut down if you do not login.  Do you wish to continue?";
                if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Login Failure") ==
                    MessageBoxButtonsResult.Yes)
                {
                    View.ShutDownApplication();
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (_initialized)
            {
                DeleteCommand.IsEnabled = CanDeleteHousehold();
                LoginCommand.IsEnabled = CanLogin();
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class LoginViewModelDesign : LoginViewModel
    {
        public LoginViewModelDesign()
        {
            for (int i = 0; i < 5; i++)
            {
                Households.Add(new Households { Name = "John and Jane Doe Household Demo" });
            }
        }
    }
}
