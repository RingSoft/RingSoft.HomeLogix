using RingSoft.App.Library;
using RingSoft.HomeLogix.MasterData;
using System;
using RingSoft.HomeLogix.DataAccess;

namespace RingSoft.HomeLogix.Library
{
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
        public static HomeLogixLookupContext LookupContext { get; }

        public static Households LoggedInHousehold { get; set; }

        public static event EventHandler<AppProgressArgs> AppSplashProgress;

        public static void InitSettings()
        {
            RingSoftAppGlobals.AppTitle = "HomeLogix";
            RingSoftAppGlobals.AppCopyright = "©2020 by Peter Ringering";
            RingSoftAppGlobals.AppVersion = "0.80.00";
        }

        public static void Initialize()
        
        {
            AppSplashProgress?.Invoke(null, new AppProgressArgs("Initializing Database Structure."));

            DataAccessGlobals.LookupContext = new HomeLogixLookupContext();
            LookupContext.SqliteDataProcessor.FilePath = MasterDbContext.ProgramDataFolder;
            LookupContext.SqliteDataProcessor.FileName = MasterDbContext.DemoDataFileName;

            LookupContext.Initialize();

            AppSplashProgress?.Invoke(null, new AppProgressArgs("Connecting to Master Database."));

            MasterDbContext.ConnectToMaster();
        }
    }
}
