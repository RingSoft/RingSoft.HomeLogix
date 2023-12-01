
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.MasterData;
using RingSoft.HomeLogix.Sqlite;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using RingSoft.App.Interop;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;
using RingSoft.HomeLogix.Library.ViewModels.Main;
using RingSoft.HomeLogix.SqlServer;

namespace RingSoft.HomeLogix.Library
{
    public interface IMainViewModel
    {
        void RefreshView();

        DateTime CurrentMonthEnding { get; set; }
    }

    public class AppProgressArgs
    {
        public string ProgressText { get; }

        public AppProgressArgs(string progressText)
        {
            ProgressText = progressText;
        }
    }
    public static class AppGlobals
    {
        public const int BudgetItemIncomeType = (int) BudgetItemTypes.Income;
        public const int BudgetItemExpenseType = (int) BudgetItemTypes.Expense;
        public const int BudgetItemTransferType = (int) BudgetItemTypes.Transfer;

        public const int BudgetItemLineTypeId = (int)BankAccountRegisterItemTypes.BudgetItem;
        public const int MiscellaneousLineTypeId = (int)BankAccountRegisterItemTypes.Miscellaneous;
        public const int TransferToBankAccountLineTypeId = (int)BankAccountRegisterItemTypes.TransferToBankAccount;

        public const int BankTransactionTypeDepositId = 0;
        public const int BankTransactionWithdrawalId = 1;

        public static HomeLogixLookupContext LookupContext { get; private set; }

        public static IDataRepository DataRepository { get; set; }

        public static Household LoggedInHousehold { get; set; }

        public static bool UnitTesting { get; set; }

        public static MainViewModel MainViewModel { get; set; }

        public static DbPlatforms DbPlatform { get; set; }

        public static event EventHandler<AppProgressArgs> AppSplashProgress;

        public static void InitSettings()
        {
            RingSoftAppGlobals.AppTitle = "HomeLogix";
            RingSoftAppGlobals.AppCopyright = "©2023 by Peter Ringering";
            RingSoftAppGlobals.AppGuid = "7b811d4b-e39a-4316-a1dd-ba58eeafede7";
            RingSoftAppGlobals.AppVersion = 213;
            RingSoftAppGlobals.PathToDownloadUpgrade = MasterDbContext.ProgramDataFolder;
            SystemGlobals.ProgramDataFolder = MasterDbContext.ProgramDataFolder;
        }

        public static async void Initialize()
        {
            DataRepository = new DataRepository();
            InitializeLookupContext();
            AppSplashProgress?.Invoke(null, new AppProgressArgs("Initializing Database Structure."));
            SystemGlobals.ValidateDeletedData = false;

            if (UnitTesting)
            {
                var sqliteContext = new SqliteHomeLogixDbContext();
                sqliteContext.IsDesignTime = true;
                LookupContext.LocalDbContext = sqliteContext;
                LookupContext.Initialize(sqliteContext, DbPlatforms.Sqlite);
                //LookupContext.TestInitialize();
                MainViewModel = new MainViewModel();
            }
            else
            {
                AppSplashProgress?.Invoke(null, new AppProgressArgs("Connecting to the Master Database."));

                MasterDbContext.ConnectToMaster();

                var defaultHousehold = MasterDbContext.GetDefaultHousehold();
                if (defaultHousehold != null)
                {
                    if (LoginToHousehold(defaultHousehold).IsNullOrEmpty())
                        LoggedInHousehold = defaultHousehold;
                }

                //var response = RingSoftAppGlobals.GetWebResponse("/HomeLogixData/", WebRequestMethods.Ftp.ListDirectory);
                //using (var stream = response.GetResponseStream())
                //{
                //    using (var reader = new StreamReader(stream, true))
                //    {
                //        var text = reader.ReadToEnd();
                //    }
                //}

                //RingSoftAppGlobals.UploadFile("/HomeLogixData/test.txt", "C:\\Temp\\Test.txt");

                //RingSoftAppGlobals.DownloadFile("/HomeLogixData/test.txt", "C:\\Temp\\Test123.txt");
            }
        }

        private static void InitializeLookupContext()
        {
            LookupContext = new HomeLogixLookupContext();
            LookupContext.SqliteDataProcessor.FilePath = MasterDbContext.ProgramDataFolder;
            LookupContext.SqliteDataProcessor.FileName = MasterDbContext.DemoDataFileName;

        }

        public static IHomeLogixDbContext GetNewDbContext(DbPlatforms? platform = null)
        {
            if (platform == null)
            {
                platform = DbPlatform;
            }
            switch (platform)
            {
                case DbPlatforms.Sqlite:
                    var sqliteResult = new SqliteHomeLogixDbContext();
                    sqliteResult.SetLookupContext(AppGlobals.LookupContext);
                    return sqliteResult;
                case DbPlatforms.SqlServer:
                    var result = new SqlServerHomeLogixDbContext();
                    result.SetLookupContext(AppGlobals.LookupContext);
                    return result;
                //case DbPlatforms.MySql:
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string LoginToHousehold(Household household)
        {
            AppSplashProgress?.Invoke(null, new AppProgressArgs($"Migrating the {household.Name} Database."));
            DbPlatform = (DbPlatforms) household.Platform;
            LookupContext.DbPlatform = DbPlatform;
            var context = GetNewDbContext();
            context.SetLookupContext(LookupContext);
            LoadDataProcessor(household);
            SystemMaster systemMaster = null;
            DbContext migrateContext = context.DbContext;
            var migrateResult = string.Empty;
            switch ((DbPlatforms)household.Platform)
            {
                case DbPlatforms.Sqlite:
                    if (!household.FilePath.EndsWith('\\'))
                        household.FilePath += "\\";

                    LookupContext.Initialize(context, DbPlatform);
                    
                    var newFile = !File.Exists($"{household.FilePath}{household.FileName}");

                    if (newFile == false)
                    {
                        try
                        {
                            var file = new FileInfo($"{household.FilePath}{household.FileName}");
                            file.IsReadOnly = false;
                        }
                        catch (Exception e)
                        {
                            var message = $"Can't access household file path: {household.FilePath}.  You must run this program as administrator.";
                            return message;
                        }

                        migrateResult = MigrateContext(migrateContext);
                        if (!migrateResult.IsNullOrEmpty())
                        {
                            return migrateResult;
                        }

                        systemMaster = context.SystemMaster.FirstOrDefault();
                        if (systemMaster != null) household.Name = systemMaster.HouseholdName;
                    }
                    else
                    {
                        migrateResult = MigrateContext(migrateContext);
                        if (!migrateResult.IsNullOrEmpty())
                        {
                            return migrateResult;
                        }
                        systemMaster = new SystemMaster { HouseholdName = household.Name };
                        context.DbContext.AddNewEntity(context.SystemMaster, systemMaster, "Saving SystemMaster");

                    }

                    break;
                case DbPlatforms.SqlServer:

                    migrateResult = MigrateContext(migrateContext);
                    if (!migrateResult.IsNullOrEmpty())
                    {
                        return migrateResult;
                    }
                    var databases = RingSoftAppGlobals.GetSqlServerDatabaseList(household.Server);

                    if (databases.IndexOf(household.Database) >= 0)
                    {
                        systemMaster = context.SystemMaster.FirstOrDefault();
                        if (systemMaster != null) household.Name = systemMaster.HouseholdName;
                    }

                    if (systemMaster == null)
                    {
                        systemMaster = new SystemMaster { HouseholdName = household.Name };
                        context.DbContext.AddNewEntity(context.SystemMaster, systemMaster, "Saving SystemMaster");
                    }
                    LookupContext.Initialize(context, DbPlatform);

                    break;
                case DbPlatforms.MySql:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            AppSplashProgress?.Invoke(null, new AppProgressArgs($"Connecting to the {household.Name} Database."));
            //var selectQuery = new SelectQuery(LookupContext.SystemMaster.TableName);
            //LookupContext.DataProcessor.GetData(selectQuery, false);
            LookupContext.DataProcessor.TestConnection();

            return string.Empty;
        }

        public static string MigrateContext(DbContext migrateContext)
        {
            try
            {
                migrateContext.Database.Migrate();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return string.Empty;
        }

        public static void LoadDataProcessor(Household household, DbPlatforms? platform = null)
        {
            if (platform == null)
            {
                platform = (DbPlatforms) household.Platform;
            }

            switch (platform)
            {
                case DbPlatforms.Sqlite:
                    LookupContext.SqliteDataProcessor.FilePath = household.FilePath;
                    LookupContext.SqliteDataProcessor.FileName = household.FileName;
                    break;
                case DbPlatforms.SqlServer:
                    LookupContext.SqlServerDataProcessor.Server = household.Server;
                    LookupContext.SqlServerDataProcessor.Database = household.Database;
                    LookupContext.SqlServerDataProcessor.SecurityType = (SecurityTypes)household.AuthenticationType;
                    LookupContext.SqlServerDataProcessor.UserName = household.Username;
                    LookupContext.SqlServerDataProcessor.Password = household.Password.DecryptDatabasePassword();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string OpenTextFile(string fileName, string folder = "")
        {
            var result = string.Empty;
            var path = $"{MasterDbContext.ProgramDataFolder}\\";
            if (!folder.IsNullOrEmpty())
            {
                if (!folder.EndsWith("\\"))
                {
                    folder += "\\";
                }

                path = folder;
            }
            var fileNamePath = $"{path}{fileName}";

            try
            {
                var openFile = new StreamReader(fileNamePath);
                result = openFile.ReadToEnd();
                openFile.Close();
                openFile.Dispose();
                GC.Collect();
            }
            catch (Exception e)
            {
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error Opening Text File", RsMessageBoxIcons.Error);
            }

            return result;
        }

        public static void WriteTextFile(string fileName, string text, string folder = "")
        {
            var path = $"{MasterDbContext.ProgramDataFolder}\\";
            if (!folder.IsNullOrEmpty())
            {
                if (!folder.EndsWith("\\"))
                {
                    folder += "\\";
                }

                path = folder;
            }
            var fileNamePath = $"{path}{fileName}";
            try
            {
                var directory = Path.GetDirectoryName(fileNamePath);
                if (!Directory.Exists(directory))
                    if (directory != null)
                        Directory.CreateDirectory(directory);

                WriteTextFile(fileNamePath, text);
            }
            catch (Exception e)
            {
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error Writing Text File", RsMessageBoxIcons.Error);
            }
        }

        public static string WriteTextFile(string fileName, string text)
        {
            var filePath = $"{MasterDbContext.ProgramDataFolder}\\{fileName}";
            try
            {
                File.WriteAllText(filePath, text);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return string.Empty;
        }

        public static void DeleteFile(string fileName)
        {
            //var filePath = $"{MainViewModel.View.GetWriteablePath()}\\{fileName}";
            var filePath = $"{MasterDbContext.ProgramDataFolder}\\{fileName}";
            File.Delete(filePath);
        }

        public static void UploadFile(string fileName, string guid = "")
        {
            var folder = "/public_html/HomeLogixData/";
            if (!guid.IsNullOrEmpty())
            {
                folder += guid + "/";
            }
            RingSoftAppGlobals
                .UploadFile($"{folder}{fileName}", MasterDbContext.ProgramDataFolder + $"\\{fileName}");
        }

        //public static void DownloadFile(string fileName, string guid = "")
        //{
        //    var folder = "/public_html/HomeLogixData/";
        //    if (!guid.IsNullOrEmpty())
        //    {
        //        folder += guid + "/";
        //    }
        //    RingSoftAppGlobals.DownloadFile($"{folder}{fileName}", MasterDbContext.ProgramDataFolder + $"\\{fileName}");
        //}

        public static WebResponse GetWebResponse(string method, string url = "")
        {
            if (url.IsNullOrEmpty())
            {
                url = "/public_html/HomeLogixData/";
            }
            else
            {
                url = $"/public_html/HomeLogixData/{url}";
            }
            return RingSoftAppGlobals.GetWebResponse($"{url}", method);
        }

        public static MobileInterop.PhoneModel.TransactionTypes ToRegisterDataTranType(
            this HomeLogix.Library.ViewModels.Budget.TransactionTypes sourceTransactionType)
        {
            switch (sourceTransactionType)
            {
                case TransactionTypes.Deposit:
                    return MobileInterop.PhoneModel.TransactionTypes.Deposit;
                case TransactionTypes.Withdrawal:
                    return MobileInterop.PhoneModel.TransactionTypes.Withdrawal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sourceTransactionType), sourceTransactionType, null);
            }
        }
    }
}
