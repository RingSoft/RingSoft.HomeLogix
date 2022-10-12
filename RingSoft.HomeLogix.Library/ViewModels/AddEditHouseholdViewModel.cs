using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.MasterData;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.Sqlite;
using RingSoft.HomeLogix.SqlServer;

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

                //if (!_settingDbProperties)

                //SetDefaultDatabaseName();
                //    SetPlatformProperties();
                //}
            }
        }



        public Household Household { get; private set; }

        public DbPlatforms? OriginalDbPlatform { get; set; }

        public IAddEditHouseholdView View { get; private set; }

        public ICommand ShowFileDialogCommand { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public bool SettingDbProperties { get; set; }

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
                DbPlatform = (DbPlatforms) View.Household.Platform;
                SetPlatformProperties();
            }
            else
            {
                SetDefaultDatabaseName();
            }
        }

        public void SetDefaultDatabaseName()
        {
            var folder = Environment.GetEnvironmentVariable("OneDriveConsumer");
            if (folder.IsNullOrEmpty())
                folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (SqliteLoginViewModel != null)
            {
                var fileName = $"{HouseholdName} HomeLogix.sqlite";
                if (View.Household != null)
                {
                    if ((DbPlatforms)View.Household.Platform != DbPlatforms.Sqlite)
                    {
                        SqliteLoginViewModel.FilenamePath = $"{folder?.Trim()}\\{fileName.Trim()}";
                    }
                }
                else
                {
                    SqliteLoginViewModel.FilenamePath = $"{folder?.Trim()}\\{fileName.Trim()}";
                }
            }

            if (SqlServerLoginViewModel != null)
            {
                if (View.Household != null)
                {
                    if ((DbPlatforms)View.Household.Platform != DbPlatforms.SqlServer)
                    {
                        SqlServerLoginViewModel.Database = $"{GetDatabaseName()}HomeLogix";
                    }
                }
                else
                {
                    SqlServerLoginViewModel.Database = $"{GetDatabaseName()}HomeLogix";
                }
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

            if (View.Household == null)
            {
                Household = new Household();
            }
            else
            {
                if (OriginalDbPlatform.HasValue && OriginalDbPlatform != DbPlatform)
                {
                    var message =
                        $"Do you wish to copy your data from {OriginalDbPlatform.Value.PlatformText()} to {DbPlatform.PlatformText()}?";
                    var caption = "Copy Data?";
                    if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption, true) == MessageBoxButtonsResult.Yes)
                    {
                        Household = View.Household;
                        AppGlobals.LoginToHousehold(View.Household);
                        

                        if (!SetHouseholdProperties())
                            return;

                        DbDataProcessor destinationProcessor = null;
                        switch (DbPlatform)
                        {
                            case DbPlatforms.Sqlite:
                                destinationProcessor = AppGlobals.LookupContext.SqliteDataProcessor;
                                break;
                            case DbPlatforms.SqlServer:
                                destinationProcessor = AppGlobals.LookupContext.SqlServerDataProcessor;
                                break;
                            case DbPlatforms.MySql:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();

                        }
                        //SystemGlobals.AdvancedFindLookupContext = AppGlobals.LookupContext;
                        //var configuration = new AdvancedFindLookupConfiguration(SystemGlobals.AdvancedFindLookupContext);
                        //var context = AppGlobals.GetNewDbContext();
                        //context.SetLookupContext(AppGlobals.LookupContext);
                        //configuration.InitializeModel();
                        //var context = AppGlobals.GetNewDbContext();
                        //context.SetLookupContext(AppGlobals.LookupContext);
                        //AppGlobals.LookupContext.Initialize(AppGlobals.GetNewDbContext(), OriginalDbPlatform.Value);
                        //AppGlobals.LoginToHousehold(View.Household);
                        if (!RingSoftAppGlobals.CopyData(AppGlobals.LookupContext, destinationProcessor))
                        {
                            return;
                        }

                    }
                }
            }
            Household.Platform = (byte)DbPlatform;
            if (!SetHouseholdProperties())
                return;

            if (View.Household != null)
            {
                AppGlobals.LookupContext.DbPlatform = DbPlatform;
                var context = AppGlobals.GetNewDbContext();
                context.SetLookupContext(AppGlobals.LookupContext);
            }
            View.CloseWindow();
        }

        private bool SetHouseholdProperties()
        {
            Household.Name = HouseholdName;

            switch (DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    var fileInfo = new FileInfo(SqliteLoginViewModel.FilenamePath);

                    Household.FileName = fileInfo.Name;
                    Household.FilePath = fileInfo.DirectoryName;
                    AppGlobals.LookupContext.SqliteDataProcessor.FilePath = Household.FilePath;
                    AppGlobals.LookupContext.SqliteDataProcessor.FileName = Household.FileName;
                    break;
                case DbPlatforms.SqlServer:
                    if (!SqlServerLoginViewModel.TestDatabaseConnection())
                    {
                        return false;
                    }

                    Household.Server = SqlServerLoginViewModel.Server;
                    Household.Database = SqlServerLoginViewModel.Database;
                    Household.AuthenticationType = (byte) SqlServerLoginViewModel.SecurityType;
                    Household.Username = SqlServerLoginViewModel.UserName;
                    Household.Password = SqlServerLoginViewModel.Password;

                    AppGlobals.LookupContext.SqlServerDataProcessor.Server = SqlServerLoginViewModel.Server;
                    AppGlobals.LookupContext.SqlServerDataProcessor.Database = SqlServerLoginViewModel.Database;
                    AppGlobals.LookupContext.SqlServerDataProcessor.SecurityType = SqlServerLoginViewModel.SecurityType;
                    AppGlobals.LookupContext.SqlServerDataProcessor.UserName = SqlServerLoginViewModel.UserName;
                    AppGlobals.LookupContext.SqlServerDataProcessor.Password = SqlServerLoginViewModel.Password;
                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
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
                SettingDbProperties = true;
                HouseholdName = View.Household.Name;
                //DbPlatform = (DbPlatforms) View.Household.Platform;

                SqliteLoginViewModel.FilenamePath = $"{View.Household.FilePath}\\{View.Household.FileName}";
                SqlServerLoginViewModel.Server = View.Household.Server;
                SqlServerLoginViewModel.Database = View.Household.Database;
                SqlServerLoginViewModel.SecurityType = (SecurityTypes) View.Household.AuthenticationType;
                SqlServerLoginViewModel.UserName = View.Household.Username;
                SqlServerLoginViewModel.Password = View.Household.Password;
                SettingDbProperties = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
