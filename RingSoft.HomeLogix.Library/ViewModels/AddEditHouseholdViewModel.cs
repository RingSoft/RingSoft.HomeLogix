using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.MasterData;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RingSoft.App.Library;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public enum SetFocusControls
    {
        HouseholdName = 0,
        FileName = 1
    }

    public interface IAddEditHouseholdView
    {
        void CloseWindow();

        void SetFocus(SetFocusControls control);

        void SetPlatform();
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
                SetFileName();

                OnPropertyChanged();
            }
        }

        private SqliteLoginViewModel _sqliteLoginViewModel;

        public SqliteLoginViewModel SqliteLoginViewModel
        {
            get => _sqliteLoginViewModel;
            set
            {
                if (_sqliteLoginViewModel == value)
                {
                    return;
                }
                _sqliteLoginViewModel = value;
                OnPropertyChanged();
            }
        }

        private DbPlatforms _dbPlatform;

        public DbPlatforms DbPlatform
        {
            get => _dbPlatform;
            set
            {
                if (_dbPlatform == value)
                {
                    return;
                }
                _dbPlatform = value;
                OnPropertyChanged();
                View.SetPlatform();
            }
        }



        public Household Household { get; private set; }

        public IAddEditHouseholdView View { get; private set; }

        public ICommand ShowFileDialogCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public AddEditHouseholdViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(OnCancel);
        }

        public void OnViewLoaded(IAddEditHouseholdView addEditHouseholdView)
        {
            View = addEditHouseholdView;
            DbPlatform = DbPlatforms.Sqlite;
            SetFileName();
        }

        public void SetFileName()
        {
            var folder = Environment.GetEnvironmentVariable("OneDriveConsumer");
            if (folder.IsNullOrEmpty())
                folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var fileName = $"{HouseholdName} HomeLogix.sqlite";

            if (SqliteLoginViewModel != null)
            {
                SqliteLoginViewModel.FilenamePath = $"{folder?.Trim()}\\{fileName.Trim()}";
            }
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

            if (SqliteLoginViewModel.FilenamePath.IsNullOrEmpty())
            {
                var message = "File Name must have a value";
                ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid File Name", RsMessageBoxIcons.Exclamation);
                View.SetFocus(SetFocusControls.FileName);
                return;
            }

            var fileInfo = new FileInfo(SqliteLoginViewModel.FilenamePath);

            Household = new Household
            {
                Name = HouseholdName,
                FileName = fileInfo.Name,
                FilePath = fileInfo.DirectoryName
            };

            View.CloseWindow();
        }

        private void OnCancel()
        {
            View.CloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
