using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.Library.Annotations;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public interface ILoginView
    {
        bool ShowAddEditHousehold(Households household);

        string GetHouseholdDataFile();

        void CloseWindow(bool dialogResult);
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

        public ICommand AddNewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ConnectToDataFileCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand CancelCommand { get; }

        public LoginViewModel()
        {
            Households = new ObservableCollection<Households>();
            var dbHouseholds = MasterDbContext.GetHouseholds();

            foreach (var household in dbHouseholds)
            {
                Households.Add(household);
            }

            if (Households.Any())
                SelectedHousehold = Households[0];

            AddNewCommand = new RelayCommand(AddNewHouseHold);
            EditCommand = new RelayCommand(EditHousehold){IsEnabled = CanLogin()};
            DeleteCommand = new RelayCommand(DeleteHousehold){IsEnabled = CanDeleteHousehold()};
            ConnectToDataFileCommand = new RelayCommand(ConnectToDataFile);
            LoginCommand = new RelayCommand(Login){IsEnabled = CanLogin()};
            CancelCommand = new RelayCommand(Cancel);
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
            var newHousehold = new Households();
            if (View.ShowAddEditHousehold(newHousehold))
            {
                if (MasterDbContext.SaveHousehold(newHousehold))
                {
                    Households.Add(newHousehold);
                    SelectedHousehold = newHousehold;
                }
            }
        }

        private void EditHousehold()
        {
            if (View.ShowAddEditHousehold(SelectedHousehold))
            {
                MasterDbContext.SaveHousehold(SelectedHousehold);
            }
        }

        private void ConnectToDataFile()
        {
            throw new NotImplementedException();
        }

        private void DeleteHousehold()
        {
            var message = "Are you sure you wish to delete this household?";
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
            AppGlobals.LoggedInHousehold = SelectedHousehold;

            View.CloseWindow(true);
        }

        private void Cancel()
        {
            if (AppGlobals.LoggedInHousehold == null)
            {
                var message = "Login failure.  Application will shut down.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Login Failure", RsMessageBoxIcons.Information);
            }

            View.CloseWindow(false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
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
