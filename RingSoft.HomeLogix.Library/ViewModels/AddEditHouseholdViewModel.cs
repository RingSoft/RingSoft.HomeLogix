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
using RingSoft.App.Interop;
using RingSoft.App.Library;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess;
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
        protected override string TestTable => "SystemMaster";
        public AddEditHouseholdViewModel()
        {
            DbName = "HomeLogix";
        }



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
            if (!directory.IsNullOrEmpty() && !directory.EndsWith("\\"))
            {
                directory += "\\";
            }

            SqliteLoginViewModel.FilenamePath = $"{directory}{entity.FileName}";
            SqlServerLoginViewModel.Server = entity.Server;
            SqlServerLoginViewModel.Database = entity.Database;
            if (entity.AuthenticationType != null)
                SqlServerLoginViewModel.SecurityType = (SecurityTypes) entity.AuthenticationType.Value;
            SqlServerLoginViewModel.UserName = entity.Username;
            SqlServerLoginViewModel.Password = entity.Password.DecryptDatabasePassword();
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
            entity.Password = SqlServerLoginViewModel.Password.EncryptDatabasePassword();
        }

        protected override bool PreDataCopy(ref LookupContext context, ref DbDataProcessor destinationProcessor
            , ITwoTierProcedure procedure)
        {
            DbContext destinationDbContext = null;
            IHomeLogixDbContext sourceDbContext = null;
            context = AppGlobals.LookupContext;
            switch (DbPlatform)
            {
                case DbPlatforms.Sqlite:
                    destinationProcessor = AppGlobals.LookupContext.SqliteDataProcessor;
                    LoadDbDataProcessor(destinationProcessor);
                    var sqliteHomeLogixDbContext = new SqliteHomeLogixDbContext();
                    sqliteHomeLogixDbContext.SetLookupContext(AppGlobals.LookupContext);
                    destinationDbContext = sqliteHomeLogixDbContext;
                    break;
                case DbPlatforms.SqlServer:
                    var sqlServerProcessor = AppGlobals.LookupContext.SqlServerDataProcessor;
                    destinationProcessor = sqlServerProcessor;
                    LoadDbDataProcessor(destinationProcessor);
                    var sqlServerContext = new SqlServerHomeLogixDbContext();
                    sqlServerContext.SetLookupContext(AppGlobals.LookupContext);
                    destinationDbContext = sqlServerContext;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }

            AppGlobals.DbPlatform = OriginalDbPlatform;
            switch (OriginalDbPlatform)
            {
                case DbPlatforms.Sqlite:
                    sourceDbContext = new SqliteHomeLogixDbContext();
                    break;
                case DbPlatforms.SqlServer:
                    sourceDbContext = new SqlServerHomeLogixDbContext();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            AppGlobals.LoadDataProcessor(Object, OriginalDbPlatform);
            switch (OriginalDbPlatform)
            {
                case DbPlatforms.Sqlite:
                    if (DbPlatform == DbPlatforms.SqlServer)
                    {
                    }
                    break;
                case DbPlatforms.SqlServer:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var dropResult = destinationProcessor.DropDatabase();
            if (dropResult.ResultCode != GetDataResultCodes.Success)
            {
                procedure.ShowError(dropResult.Message, "Error Dropping Database");
                return false;
            }

            procedure.SetCursor(WindowCursorTypes.Wait);
            sourceDbContext = AppGlobals.GetNewDbContext();
            sourceDbContext.SetLookupContext(AppGlobals.LookupContext);
            AppGlobals.LookupContext.Initialize(sourceDbContext, OriginalDbPlatform);


            var result = AppGlobals.MigrateContext(AppGlobals.GetNewDbContext().DbContext);
            if (!result.IsNullOrEmpty())
            {
                procedure.SetCursor(WindowCursorTypes.Default);
                procedure.ShowError(result, "File Access Error");
                return false;
            }

            result = AppGlobals.MigrateContext(destinationDbContext);
            if (!result.IsNullOrEmpty())
            {
                procedure.SetCursor(WindowCursorTypes.Default);
                procedure.ShowError(result, "File Access Error");
                return false;
            }
            return true;
        }
    }
}
