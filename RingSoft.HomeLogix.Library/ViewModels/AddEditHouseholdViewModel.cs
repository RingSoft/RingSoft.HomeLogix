using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.MasterData;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

        Household Household { get; set; }
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
                //SetFileName();

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

        private SqlServerLoginViewModel _sqlServerLoginViewModel;

        public SqlServerLoginViewModel SqlServerLoginViewModel
        {
            get => _sqlServerLoginViewModel;
            set
            {
                if (_sqlServerLoginViewModel == value)
                {
                    return;
                }
                _sqlServerLoginViewModel = value;
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
                SetFileName();
            }
        }



        public Household Household { get; private set; }

        public DbPlatforms? OriginalDbPlatform { get; set; }

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
            if (View.Household != null)
            {
                OriginalDbPlatform = (DbPlatforms) View.Household.Platform;
            }
            SetFileName();
        }

        public void SetFileName()
        {
            var folder = Environment.GetEnvironmentVariable("OneDriveConsumer");
            if (folder.IsNullOrEmpty())
                folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            switch (DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    if (SqliteLoginViewModel != null)
                    {
                        var fileName = $"{HouseholdName} HomeLogix.sqlite";
                        SqliteLoginViewModel.FilenamePath = $"{folder?.Trim()}\\{fileName.Trim()}";
                    }
                    break;
                case DbPlatforms.SqlServer:
                    if (SqlServerLoginViewModel != null)
                    {
                        SqlServerLoginViewModel.Database = $"{GetDatabaseName()}HomeLogix";
                    }
                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetDatabaseName()
        {
            var result = HouseholdName;
            if (HouseholdName != null) result = HouseholdName.Replace(" ", "");
            return result;
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

            Household = new Household()
            {
                Name = HouseholdName,
                Platform = (byte)DbPlatform
            };

            switch (DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    var fileInfo = new FileInfo(SqliteLoginViewModel.FilenamePath);

                    Household.FileName = fileInfo.Name;
                    Household.FilePath = fileInfo.DirectoryName;
                    break;
                case DbPlatforms.SqlServer:
                    if (!SqlServerLoginViewModel.TestDatabaseConnection())
                    {
                        return;
                    }

                    Household.Server = SqlServerLoginViewModel.Server;
                    Household.Database = SqlServerLoginViewModel.Database;
                    Household.AuthenticationType = (byte) SqlServerLoginViewModel.SecurityType;
                    Household.Username = SqlServerLoginViewModel.UserName;
                    Household.Password = SqlServerLoginViewModel.Password;
                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            View.CloseWindow();
        }

        private void OnCancel()
        {
            View.CloseWindow();
        }

        public void SetPlatformProperties()
        {
            if (SqlServerLoginViewModel == null || SqliteLoginViewModel == null)
            {
                return;
            }
            if (View.Household != null)
            {
                HouseholdName = View.Household.Name;
                DbPlatform = (DbPlatforms) View.Household.Platform;
                switch (DbPlatform)
                {
                    case DbPlatforms.Sqlite:
                        if (SqliteLoginViewModel != null) SqliteLoginViewModel.FilenamePath 
                            = $"{View.Household.FilePath}\\{View.Household.FileName}";
                        break;
                    case DbPlatforms.SqlServer:
                        break;
                    case DbPlatforms.MySql:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
