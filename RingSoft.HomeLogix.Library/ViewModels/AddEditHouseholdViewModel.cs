using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.HomeLogix.MasterData;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
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

    public enum HouseholdProcesses
    {
        Add = 0,
        Edit = 1,
        Connect = 2
    }

    public interface IAddEditHouseholdView : IDbLoginView
    {
        void SetFocus(SetFocusControls control);

        void SetPlatform();

        Household Household { get; set; }

        HouseholdProcesses HouseholdProcess { get; set; }
    }
    public class AddEditHouseholdViewModel : DbLoginViewModel<Household>
    {
        public new IAddEditHouseholdView View { get; private set; }
        public AddEditHouseholdViewModel()
        {
            DbName = "HomeLogix";
        }



        //public Household Household { get; private set; }

        //public DbPlatforms? OriginalDbPlatform { get; set; }

        //public IAddEditHouseholdView View { get; private set; }

        //public ICommand ShowFileDialogCommand { get; }
        //public ICommand OkCommand { get; }
        //public ICommand CancelCommand { get; }

        //public bool SettingDbProperties { get; set; }

        //public AddEditHouseholdViewModel()
        //{
        //    OkCommand = new RelayCommand(OnOk);
        //    CancelCommand = new RelayCommand(OnCancel);
        //}

        //public void OnViewLoaded(IAddEditHouseholdView addEditHouseholdView)
        //{
        //    View = addEditHouseholdView;
        //    DbPlatform = DbPlatforms.Sqlite;
        //    if (View.Household != null)
        //    {
        //        OriginalDbPlatform = (DbPlatforms) View.Household.Platform;
        //        DbPlatform = (DbPlatforms) View.Household.Platform;
        //        SetPlatformProperties();
        //    }
        //    else
        //    {
        //        SetDefaultDatabaseName();
        //    }
        //}

        //public void SetDefaultDatabaseName()
        //{
        //    var folder = Environment.GetEnvironmentVariable("OneDriveConsumer");
        //    if (folder.IsNullOrEmpty())
        //        folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //    if (SqliteLoginViewModel != null)
        //    {
        //        var fileName = $"{HouseholdName} HomeLogix.sqlite";
        //        if (View.Household != null)
        //        {
        //            if ((DbPlatforms)View.Household.Platform != DbPlatforms.Sqlite)
        //            {
        //                SqliteLoginViewModel.FilenamePath = $"{folder?.Trim()}\\{fileName.Trim()}";
        //            }
        //        }
        //        else
        //        {
        //            SqliteLoginViewModel.FilenamePath = $"{folder?.Trim()}\\{fileName.Trim()}";
        //        }
        //    }

        //    if (SqlServerLoginViewModel != null)
        //    {
        //        if (View.Household != null)
        //        {
        //            if ((DbPlatforms)View.Household.Platform != DbPlatforms.SqlServer)
        //            {
        //                SqlServerLoginViewModel.Database = $"{GetDatabaseName()}HomeLogix";
        //            }
        //        }
        //        else
        //        {
        //            SqlServerLoginViewModel.Database = $"{GetDatabaseName()}HomeLogix";
        //        }
        //    }
        //}

        //private string GetDatabaseName()
        //{
        //    var result = HouseholdName;
        //    if (HouseholdName != null) result = HouseholdName.Replace(" ", "");
        //    return result;
        //}

        //private void OnOk()
        //{
        //    if (HouseholdName.IsNullOrEmpty())
        //    {
        //        var message = "Household Name must have a value";
        //        ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid Household Name", RsMessageBoxIcons.Exclamation);
        //        View.SetFocus(SetFocusControls.HouseholdName);
        //        return;
        //    }

        //    switch (DbPlatform)
        //    {
        //        case DbPlatforms.Sqlite:
        //            if (SqliteLoginViewModel.FilenamePath.IsNullOrEmpty())
        //            {
        //                var message = "File Name must have a value";
        //                ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid File Name", RsMessageBoxIcons.Exclamation);
        //                View.SetFocus(SetFocusControls.FileName);
        //                return;
        //            }
        //            break;
        //        case DbPlatforms.SqlServer:
        //            if (!SqlServerLoginViewModel.TestDatabaseConnection())
        //            {
        //                return;
        //            }
        //            if (SqlServerLoginViewModel.Database.IsNullOrEmpty())
        //            {
        //                var message = "Database must have a value";
        //                ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid File Name", RsMessageBoxIcons.Exclamation);
        //                View.SetFocus(SetFocusControls.FileName);
        //                return;
        //            }
        //            break;
        //        case DbPlatforms.MySql:
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }

        //    Household = new Household();
        //    if (View.Household != null)
        //    {
        //        var originalHousehold = new Household
        //        {
        //            Platform = View.Household.Platform,
        //            FilePath = View.Household.FilePath,
        //            FileName = View.Household.FileName,
        //            Server = View.Household.Server,
        //            Database = View.Household.Database,
        //            AuthenticationType = View.Household.AuthenticationType,
        //            Username = View.Household.Username,
        //            Password = View.Household.Password

        //    };
        //        Household = View.Household;
        //        if (OriginalDbPlatform.HasValue && OriginalDbPlatform != DbPlatform)
        //        {
        //            var message =
        //                $"Do you wish to copy your data from {OriginalDbPlatform.Value.PlatformText()} to {DbPlatform.PlatformText()}?";
        //            var caption = "Copy Data?";
        //            if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, caption, true) == MessageBoxButtonsResult.Yes)
        //            {
        //                //AppGlobals.LoginToHousehold(View.Household);
                        
        //                if (!SetHouseholdProperties())
        //                    return;
        //                SetPropertiesForPlatformFromHousehold(originalHousehold);

        //                DbDataProcessor destinationProcessor = null;
        //                DbContext dbContext = null;
        //                switch (DbPlatform)
        //                {
        //                    case DbPlatforms.Sqlite:
        //                        destinationProcessor = AppGlobals.LookupContext.SqliteDataProcessor;
        //                        var sqliteHomeLogixDbContext = new SqliteHomeLogixDbContext();
        //                        sqliteHomeLogixDbContext.SetLookupContext(AppGlobals.LookupContext);
        //                        dbContext = sqliteHomeLogixDbContext;
        //                        break;
        //                    case DbPlatforms.SqlServer:
        //                        var sqlServerProcessor = AppGlobals.LookupContext.SqlServerDataProcessor;
        //                        destinationProcessor = sqlServerProcessor;
        //                        var sqlServerContext = new SqlServerHomeLogixDbContext();
        //                        sqlServerContext.SetLookupContext(AppGlobals.LookupContext);
        //                        dbContext = sqlServerContext;
        //                        break;
        //                    case DbPlatforms.MySql:
        //                        break;
        //                    default:
        //                        throw new ArgumentOutOfRangeException();

        //                }

        //                if (!destinationProcessor.DropDatabase())
        //                {
        //                    return;
        //                }

        //                AppGlobals.GetNewDbContext().SetLookupContext(AppGlobals.LookupContext);
        //                AppGlobals.LookupContext.Initialize(AppGlobals.GetNewDbContext(), OriginalDbPlatform.Value);

        //                AppGlobals.GetNewDbContext().DbContext.Database.Migrate();

        //                //AppGlobals.LoginToHousehold(originalHousehold);

        //                dbContext.Database.Migrate();

        //                //SystemGlobals.AdvancedFindLookupContext = AppGlobals.LookupContext;
        //                //var configuration = new AdvancedFindLookupConfiguration(SystemGlobals.AdvancedFindLookupContext);
        //                //var context = AppGlobals.GetNewDbContext();
        //                //context.SetLookupContext(AppGlobals.LookupContext);
        //                //configuration.InitializeModel();
        //                //var context = AppGlobals.GetNewDbContext();
        //                //context.SetLookupContext(AppGlobals.LookupContext);
        //                //AppGlobals.LookupContext.Initialize(AppGlobals.GetNewDbContext(), OriginalDbPlatform.Value);
        //                //AppGlobals.LoginToHousehold(View.Household);

        //                if (!RingSoftAppGlobals.CopyData(AppGlobals.LookupContext, destinationProcessor))
        //                {
        //                    return;
        //                }
        //                View.DataCopied = true;
        //            }
        //        }
        //    }

        //    if (!SetHouseholdProperties())
        //        return;
        //    View.Household = Household;

        //    if (View.Household != null)
        //    {
        //        //AppGlobals.LookupContext.DbPlatform = DbPlatform;
        //        //var context = AppGlobals.GetNewDbContext();
        //        //context.SetLookupContext(AppGlobals.LookupContext);
        //        //AppGlobals.LookupContext.Initialize(context, DbPlatform);
        //    }
        //    MasterDbContext.SaveHousehold(Household);
        //    View.CloseWindow();
        //}

        //private bool SetHouseholdProperties()
        //{
        //    Household.Name = HouseholdName;
        //    Household.Platform = (byte) DbPlatform;
        //    switch (DbPlatform)
        //    {
        //        case DbPlatforms.Sqlite:
        //            var fileInfo = new FileInfo(SqliteLoginViewModel.FilenamePath);

        //            Household.FileName = fileInfo.Name;
        //            Household.FilePath = fileInfo.DirectoryName;
        //            break;
        //        case DbPlatforms.SqlServer:
        //            if (!SqlServerLoginViewModel.TestDatabaseConnection())
        //            {
        //                return false;
        //            }

        //            Household.Server = SqlServerLoginViewModel.Server;
        //            Household.Database = SqlServerLoginViewModel.Database;
        //            Household.AuthenticationType = (byte) SqlServerLoginViewModel.SecurityType;
        //            Household.Username = SqlServerLoginViewModel.UserName;
        //            Household.Password = SqlServerLoginViewModel.Password;

        //            break;
        //        case DbPlatforms.MySql:
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //    SetPropertiesForPlatformFromHousehold(Household);

        //    return true;
        //}

        //private void SetPropertiesForPlatformFromHousehold(Household household)
        //{
        //    var platform = (DbPlatforms)household.Platform;
        //    switch (platform)
        //    {
        //        case DbPlatforms.Sqlite:
        //            AppGlobals.LookupContext.SqliteDataProcessor.FilePath = household.FilePath;
        //            AppGlobals.LookupContext.SqliteDataProcessor.FileName = household.FileName;
        //            break;
        //        case DbPlatforms.SqlServer:
        //            AppGlobals.LookupContext.SqlServerDataProcessor.Server = household.Server;
        //            AppGlobals.LookupContext.SqlServerDataProcessor.Database = household.Database;
        //            AppGlobals.LookupContext.SqlServerDataProcessor.SecurityType =
        //                (SecurityTypes) household.AuthenticationType;
        //            AppGlobals.LookupContext.SqlServerDataProcessor.UserName = household.Username;
        //            AppGlobals.LookupContext.SqlServerDataProcessor.Password = household.Password;
        //            break;
        //        case DbPlatforms.MySql:
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
        //    }
        //}

        //private void OnCancel()
        //{
        //    View.CloseWindow();
        //}

        //public void SetPlatformProperties()
        //{
        //    if (SqlServerLoginViewModel == null || SqliteLoginViewModel == null)
        //    {
        //        return;
        //    }

        //    if (View.Household != null)
        //    {
        //        SettingDbProperties = true;
        //        HouseholdName = View.Household.Name;
        //        //DbPlatform = (DbPlatforms) View.Household.Platform;

        //        var path = View.Household.FilePath;
        //        if (!path.EndsWith("\\"))
        //            path += "\\";
        //        SqliteLoginViewModel.FilenamePath = $"{path}{View.Household.FileName}";
        //        SqlServerLoginViewModel.Server = View.Household.Server;
        //        SqlServerLoginViewModel.Database = View.Household.Database;
        //        if (View.Household.AuthenticationType != null)
        //            SqlServerLoginViewModel.SecurityType = (SecurityTypes) View.Household.AuthenticationType;
        //        SqlServerLoginViewModel.UserName = View.Household.Username;
        //        SqlServerLoginViewModel.Password = View.Household.Password;
        //        SettingDbProperties = false;
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public override void Initialize(IDbLoginView view, DbLoginProcesses loginProcess, SqliteLoginViewModel sqliteLoginViewModel,
            SqlServerLoginViewModel sqlServerLoginViewModel, Household entity)
        {
            if (view is IAddEditHouseholdView addEditHouseholdView)
            {
                View = addEditHouseholdView;
            }

            base.Initialize(view, loginProcess, sqliteLoginViewModel, sqlServerLoginViewModel, entity);
        }

        public override void LoadFromEntity(Household entity)
        {
            EntityName = entity.Name;
            DbPlatform = (DbPlatforms) entity.Platform;
            var directory = entity.FilePath;
            if (!directory.EndsWith("\\"))
            {
                directory += "\\";
            }

            SqliteLoginViewModel.FilenamePath = $"{directory}{entity.FileName}";
            SqlServerLoginViewModel.Server = entity.Server;
            SqlServerLoginViewModel.Database = entity.Database;
            if (entity.AuthenticationType != null)
                SqlServerLoginViewModel.SecurityType = (SecurityTypes) entity.AuthenticationType.Value;
            SqlServerLoginViewModel.UserName = entity.Username;
            SqlServerLoginViewModel.Password = entity.Password;
        }

        protected override void ShowEntityNameFailure()
        {
            var message = $"Household Name must have a value";
            ControlsGlobals.UserInterface.ShowMessageBox(message, "Invalid Household Name", RsMessageBoxIcons.Exclamation);
            View.SetFocus(SetFocusControls.HouseholdName);
        }

        protected override void SaveEntity(Household entity)
        {
            if (Object != null)
            {
                entity.Id = Object.Id;
            }
            entity.Name = EntityName;
            entity.FilePath = SqliteLoginViewModel.FilePath;
            entity.FileName = SqliteLoginViewModel.FileName;
            entity.Platform = (byte) DbPlatform;
            entity.Server = SqlServerLoginViewModel.Server;
            entity.Database = SqlServerLoginViewModel.Database;
            entity.AuthenticationType = (byte) SqlServerLoginViewModel.SecurityType;
            entity.Username = SqlServerLoginViewModel.UserName;
            entity.Password = SqlServerLoginViewModel.Password;
        }

        protected override bool PreDataCopy(ref LookupContext context, ref DbDataProcessor destinationProcessor)
        {
            DbContext dbContext = null;
            context = AppGlobals.LookupContext;
            switch (DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    destinationProcessor = AppGlobals.LookupContext.SqliteDataProcessor;
                    LoadDbDataProcessor(destinationProcessor);
                    var sqliteHomeLogixDbContext = new SqliteHomeLogixDbContext();
                    sqliteHomeLogixDbContext.SetLookupContext(AppGlobals.LookupContext);
                    dbContext = sqliteHomeLogixDbContext;
                    break;
                case DbPlatforms.SqlServer:
                    var sqlServerProcessor = AppGlobals.LookupContext.SqlServerDataProcessor;
                    destinationProcessor = sqlServerProcessor;
                    LoadDbDataProcessor(destinationProcessor);
                    var sqlServerContext = new SqlServerHomeLogixDbContext();
                    sqlServerContext.SetLookupContext(AppGlobals.LookupContext);
                    dbContext = sqlServerContext;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }
            AppGlobals.LoadDataProcessor(Object, OriginalDbPlatform);
            if (!destinationProcessor.DropDatabase())
            {
                return false;
            }

            AppGlobals.GetNewDbContext().SetLookupContext(AppGlobals.LookupContext);
            AppGlobals.LookupContext.Initialize(AppGlobals.GetNewDbContext(), OriginalDbPlatform);

            AppGlobals.GetNewDbContext().DbContext.Database.Migrate();

            dbContext.Database.Migrate();
            return true;
        }
    }
}
