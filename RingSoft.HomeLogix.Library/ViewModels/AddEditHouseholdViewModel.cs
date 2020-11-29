using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.MasterData;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public enum SetFocusControls
    {
        HouseholdName = 0,
        FileName = 1
    }

    public interface IAddEditHouseholdView
    {
        Households Household { get; }

        void CloseWindow(bool dialogResult);

        string ShowFileDialog();

        void SetFocus(SetFocusControls control);
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
            var fileName = View.ShowFileDialog();
            if (!fileName.IsNullOrEmpty())
                FileName = fileName;
        }

        private void OnOk()
        {
            if (HouseholdName.IsNullOrEmpty())
            {
                var message = "Household Name must have a value";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid Household Name", RsMessageBoxIcons.Exclamation);
                View.SetFocus(SetFocusControls.HouseholdName);
                return;
            }

            if (FileName.IsNullOrEmpty())
            {
                var message = "File Name must have a value";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid File Name", RsMessageBoxIcons.Exclamation);
                View.SetFocus(SetFocusControls.FileName);
                return;
            }

            View.Household.Name = HouseholdName;
            var fileInfo = new FileInfo(FileName);
            View.Household.FileName = fileInfo.Name;
            View.Household.FilePath = fileInfo.DirectoryName;

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
