﻿using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.MasterData;
using RingSoft.HomeLogix.Sqlite;
using System;
using System.IO;
using System.Linq;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

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
        public static HomeLogixLookupContext LookupContext { get; private set; }

        public static IDataRepository DataRepository { get; set; }

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
            DataRepository = new DataRepository();

            AppSplashProgress?.Invoke(null, new AppProgressArgs("Initializing Database Structure."));

            LookupContext = new HomeLogixLookupContext();
            LookupContext.SqliteDataProcessor.FilePath = MasterDbContext.ProgramDataFolder;
            LookupContext.SqliteDataProcessor.FileName = MasterDbContext.DemoDataFileName;

            LookupContext.Initialize(new HomeLogixDbContext(LookupContext));

            AppSplashProgress?.Invoke(null, new AppProgressArgs("Connecting to the Master Database."));

            MasterDbContext.ConnectToMaster();
        }

        public static IHomeLogixDbContext GetNewDbContext()
        {
            return new HomeLogixDbContext();
        }

        public static bool LoginToHousehold(Households household)
        {
            AppSplashProgress?.Invoke(null, new AppProgressArgs($"Migrating the {household.Name} Database."));

            if (!household.FilePath.EndsWith('\\'))
                household.FilePath += "\\";

            var newFile = !File.Exists($"{household.FilePath}{household.FileName}");

            LookupContext.SqliteDataProcessor.FilePath = household.FilePath;
            LookupContext.SqliteDataProcessor.FileName = household.FileName;

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

            return true;
        }
    }
}
