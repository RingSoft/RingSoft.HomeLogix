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
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library
{
    public interface IMainViewModel
    {
        void RefreshView();
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

        public static event EventHandler<AppProgressArgs> AppSplashProgress;

        public static void InitSettings()
        {
            RingSoftAppGlobals.AppTitle = "HomeLogix";
            RingSoftAppGlobals.AppCopyright = "©2022 by Peter Ringering";
            RingSoftAppGlobals.AppVersion = "0.91.00";
        }

        public static void Initialize()
        
        {
            DataRepository ??= new DataRepository();

            AppSplashProgress?.Invoke(null, new AppProgressArgs("Initializing Database Structure."));

            LookupContext = new HomeLogixLookupContext();
            LookupContext.SqliteDataProcessor.FilePath = MasterDbContext.ProgramDataFolder;
            LookupContext.SqliteDataProcessor.FileName = MasterDbContext.DemoDataFileName;

            LookupContext.Initialize(new HomeLogixDbContext(LookupContext));

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

        public static IHomeLogixDbContext GetNewDbContext()
        {
            return new HomeLogixDbContext();
        }

        public static string LoginToHousehold(Household household)
        {
            AppSplashProgress?.Invoke(null, new AppProgressArgs($"Migrating the {household.Name} Database."));

            if (!household.FilePath.EndsWith('\\'))
                household.FilePath += "\\";

            var newFile = !File.Exists($"{household.FilePath}{household.FileName}");

            LookupContext.SqliteDataProcessor.FilePath = household.FilePath;
            LookupContext.SqliteDataProcessor.FileName = household.FileName;

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
                
            }

            var context = GetNewDbContext();
            context.DbContext.Database.Migrate();

            if (newFile)
            {
                var systemMaster = new SystemMaster {HouseholdName = household.Name};
                context.DbContext.AddNewEntity(context.SystemMaster, systemMaster, "Saving SystemMaster");
            }
            else
            {
                var systemMaster = context.SystemMaster.FirstOrDefault();
                household.Name = systemMaster?.HouseholdName;
            }

            AppSplashProgress?.Invoke(null, new AppProgressArgs($"Connecting to the {household.Name} Database."));

            var selectQuery = new SelectQuery(LookupContext.SystemMaster.TableName);
            LookupContext.SqliteDataProcessor.GetData(selectQuery, false);

            return string.Empty;
        }
    }
}
