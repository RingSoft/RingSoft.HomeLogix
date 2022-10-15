using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.MasterData;
using RingSoft.HomeLogix.Sqlite;
using System;
using System.IO;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;
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
    public class AppGlobals
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

        public static IMainViewModel MainViewModel { get; set; }

        public static DbPlatforms DbPlatform { get; set; }

        public static event EventHandler<AppProgressArgs> AppSplashProgress;

        public static void InitSettings()
        {
            RingSoftAppGlobals.AppTitle = "HomeLogix";
            RingSoftAppGlobals.AppCopyright = "©2022 by Peter Ringering";
            RingSoftAppGlobals.AppVersion = "0.92.00";
        }

        public static void Initialize()
        
        {
            DataRepository ??= new DataRepository();
            InitializeLookupContext();

            AppSplashProgress?.Invoke(null, new AppProgressArgs("Initializing Database Structure."));


            if (!UnitTesting)
            {
                AppSplashProgress?.Invoke(null, new AppProgressArgs("Connecting to the Master Database."));

                MasterDbContext.ConnectToMaster();

                var defaultHousehold = MasterDbContext.GetDefaultHousehold();
                if (defaultHousehold != null)
                {
                    if (LoginToHousehold(defaultHousehold).IsNullOrEmpty())
                        LoggedInHousehold = defaultHousehold;
                }
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
                    return new SqliteHomeLogixDbContext();
                case DbPlatforms.SqlServer:
                    return new SqlServerHomeLogixDbContext();
                    break;
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
                            var message = $"Can't access household file path: {household.FilePath} ";
                            return message;
                        }
                        context.DbContext.Database.Migrate();
                        var systemMaster = context.SystemMaster.FirstOrDefault();
                        household.Name = systemMaster?.HouseholdName;
                    }
                    else
                    {
                        context.DbContext.Database.Migrate();
                        var systemMaster = new SystemMaster { HouseholdName = household.Name };
                        context.DbContext.AddNewEntity(context.SystemMaster, systemMaster, "Saving SystemMaster");

                    }

                    break;
                case DbPlatforms.SqlServer:
                    
                    context.DbContext.Database.Migrate();
                    var databases = RingSoftAppGlobals.GetSqlServerDatabaseList(household.Server);

                    if (databases.IndexOf(household.Database) >= 0)
                    {
                        var systemMaster = context.SystemMaster.FirstOrDefault();
                        household.Name = systemMaster?.HouseholdName;
                    }
                    else
                    {
                        var systemMaster = new SystemMaster { HouseholdName = household.Name };
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
            var selectQuery = new SelectQuery(LookupContext.SystemMaster.TableName);
            LookupContext.SqliteDataProcessor.GetData(selectQuery, false);

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
                    LookupContext.SqlServerDataProcessor.Password = household.Password;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
