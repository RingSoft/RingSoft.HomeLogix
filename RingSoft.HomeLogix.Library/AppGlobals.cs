using System;
using System.Threading;
using RingSoft.App.Library;
using RingSoft.HomeLogix.MasterData;

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

            //Thread.Sleep(3000);

            AppSplashProgress?.Invoke(null, new AppProgressArgs("Connecting to Master Database."));

            MasterDbContext.ConnectToMaster();
        }
    }
}
