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
        bool LoginToHousehold(Household household);

        Household ShowAddHousehold();

        void EditHousehold(Household household);

        string GetHouseholdDataFile();

        void CloseWindow();

        void ShutDownApplication();
    }

    public class LoginListBoxItem
    {
        public string Text { get; set; }

        public Household Household { get; set; }
    }

    public class LoginViewModel : INotifyPropertyChanged
    {
        public ILoginView View { get; private set; }

        private ObservableCollection<LoginListBoxItem> _listBoxItems;

        public ObservableCollection<LoginListBoxItem> Items
        {
            get => _listBoxItems;
            set
            {
                if (_listBoxItems == value)
                    return;

                _listBoxItems = value;
                OnPropertyChanged();
            }
        }

        private LoginListBoxItem _selectedItem;

        public LoginListBoxItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value)
                    return;

                _selectingHousehold = true;

                _selectedItem = value;

                if (SelectedItem == null)
                    IsDefault = false;
                else 
                    IsDefault = SelectedItem.Household.IsDefault;

                _selectingHousehold = false;

                OnPropertyChanged();
            }
        }

        private bool _isDefault;
        public bool IsDefault
        {
            get
            {
                return _isDefault;
            }
            set
            {
                if (_isDefault == value)
                    return;

                _isDefault = value;
                UpdateDefaults();

                OnPropertyChanged();
            }
        }

        public bool DialogResult { get; private set; }

        public RelayCommand AddNewCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ConnectToDataFileCommand { get; }
        public RelayCommand LoginCommand { get; }
        public RelayCommand CancelCommand { get; }

        private readonly bool _initialized;
        private bool _selectingHousehold;

        public LoginViewModel()
        {
            Items = new ObservableCollection<LoginListBoxItem>();
            var dbHouseholds = MasterDbContext.GetHouseholds();

            foreach (var household in dbHouseholds)
            {
                var listBoxItem = new LoginListBoxItem
                {
                    Household = household,
                    Text = household.Name
                };
                Items.Add(listBoxItem);

                if (AppGlobals.LoggedInHousehold != null && AppGlobals.LoggedInHousehold.Id == household.Id)
                {
                    listBoxItem.Text = $"(Active) {listBoxItem.Text}";
                    SelectedItem = listBoxItem;
                    IsDefault = household.IsDefault;
                }
            }

            if (SelectedItem == null && Items.Any())
                SelectedItem = Items[0];

            AddNewCommand = new RelayCommand(AddNewHousehold);
            EditCommand = new RelayCommand(EditHouseHold);
            DeleteCommand = new RelayCommand(DeleteHousehold){IsEnabled = CanDeleteHousehold()};
            ConnectToDataFileCommand = new RelayCommand(ConnectToDataFile);
            LoginCommand = new RelayCommand(Login){IsEnabled = CanLogin()};
            CancelCommand = new RelayCommand(Cancel);

            _initialized = true;
        }

        public void OnViewLoaded(ILoginView loginView) => View = loginView;

        private bool CanLogin() => SelectedItem != null;

        private bool CanDeleteHousehold()
        {
            if (SelectedItem == null)
                return false;

            if (SelectedItem.Household.Id == 1)
                return false;

            if (AppGlobals.LoggedInHousehold != null)
                return AppGlobals.LoggedInHousehold.Id != SelectedItem.Household.Id;

            return true;
        }

        private void AddNewHousehold()
        {
            var newHousehold = View.ShowAddHousehold();
            if (newHousehold != null)
                AddNewHousehold(newHousehold);
        }

        private void EditHouseHold()
        {
            View.EditHousehold(SelectedItem.Household);
        }

        private void AddNewHousehold(Household newHousehold)
        {
            var item = new LoginListBoxItem
            {
                Household = newHousehold,
                Text = newHousehold.Name
            };
            Items.Add(item);
            Items = new ObservableCollection<LoginListBoxItem>(Items.OrderBy(o => o.Text));
            MasterDbContext.SaveHousehold(newHousehold);
            SelectedItem = item;
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

                var systemMaster = AppGlobals.DataRepository.GetSystemMaster();
                if (systemMaster != null)
                {
                    var household = new Household
                    {
                        Name = systemMaster.HouseholdName,
                        FileName = fileInfo.Name,
                        FilePath = fileInfo.DirectoryName
                    };
                    AddNewHousehold(household);
                }

                AppGlobals.LookupContext.SqliteDataProcessor.FilePath = currentFilePath;
                AppGlobals.LookupContext.SqliteDataProcessor.FileName = currentFileName;
            }
        }

        private void DeleteHousehold()
        {
            string message;
            if (SelectedItem.Household.Id == 1)
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
                if (MasterDbContext.DeleteHousehold(SelectedItem.Household.Id))
                {
                    Items.Remove(SelectedItem);
                    SelectedItem = Items[0];
                }
            }
        }

        private void Login()
        {
            if (View.LoginToHousehold(SelectedItem.Household))
            {
                AppGlobals.LoggedInHousehold = SelectedItem.Household;
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

        private void UpdateDefaults()
        {
            if (_selectingHousehold)
                return;

            SelectedItem.Household.IsDefault = IsDefault;
            MasterDbContext.SaveHousehold(SelectedItem.Household);

            if (IsDefault)
            {
                foreach (var item in Items)
                {
                    if (item != SelectedItem && item.Household.IsDefault)
                    {
                        item.Household.IsDefault = false;
                        MasterDbContext.SaveHousehold(item.Household);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (_initialized)
            {
                DeleteCommand.IsEnabled = CanDeleteHousehold();
                EditCommand.IsEnabled = CanDeleteHousehold();
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
                Items.Add(new LoginListBoxItem() { Text = "John and Jane Doe Household Demo" });
            }
        }
    }
}
