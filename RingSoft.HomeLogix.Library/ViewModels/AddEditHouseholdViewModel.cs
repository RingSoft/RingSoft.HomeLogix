using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.MasterData;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public interface IAddEditHouseholdView
    {
        Households Household { get; }

        void CloseWindow(bool dialogResult);

        string ShowFileDialog();
    }
    public class AddEditHouseholdViewModel : INotifyPropertyChanged
    {
        private string _householdName;

        public string HouseholdName
        {
            get => _householdName;
            set
            {
                if (_householdName == value)
                    return;

                _householdName = value;
                OnPropertyChanged();
            }
        }

        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName == value)
                    return;
                
                _fileName = value;
                OnPropertyChanged();
            }
        }

        public IAddEditHouseholdView View { get; private set; }

        public ICommand ShowFileDialogCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public AddEditHouseholdViewModel()
        {
            ShowFileDialogCommand = new RelayCommand(ShowFileDialog);
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public void OnViewLoaded(IAddEditHouseholdView addEditHouseholdView) => View = addEditHouseholdView;

        private void ShowFileDialog()
        {
            throw new NotImplementedException();
        }

        private void OnOk()
        {
            View.CloseWindow(true);
        }

        private void OnCancel()
        {
            View.CloseWindow(false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
