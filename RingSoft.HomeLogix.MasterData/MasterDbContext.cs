﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.HomeLogix.MasterData
{
    public class MasterDbContext : DbContext
    {
        //install Microsoft.EntityFrameworkCore.Tools NuGet

        //Restart Visual Studio.

        //Add-Migration <Name>

        public virtual DbSet<Household> Households { get; set; }

        //--------------------------------------------------------------------
        public static string ProgramDataFolder
        {
            get
            {
#if DEBUG
                return RingSoftAppGlobals.AssemblyDirectory;
#else
                return
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\HomeLogix\\";
#endif
            }
        }

        public static string MasterFilePath => $"{ProgramDataFolder}{MasterFileName}";

        public const string MasterFileName = "MasterDb.sqlite";
        public const string DemoDataFileName = "DemoData.sqlite";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={MasterFilePath}");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbConstants.ConstantGenerator = new SqliteDbConstants();
            modelBuilder.Entity<Household>(entity =>
            {
                entity.HasKey(hk => hk.Id);

                entity.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);

                entity.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.FilePath).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.FileName).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.IsDefault).HasColumnType(DbConstants.BoolColumnType);

                entity.Property(p => p.Platform).HasColumnType(DbConstants.ByteColumnType);

                entity.Property(p => p.Server).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.Database).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.AuthenticationType).HasColumnType(DbConstants.ByteColumnType);

                entity.Property(p => p.Username).HasColumnType(DbConstants.StringColumnType);

                entity.Property(p => p.Password).HasColumnType(DbConstants.StringColumnType);
            });

            base.OnModelCreating(modelBuilder);
        }

        public static void ConnectToMaster()
        {
            if (!Directory.Exists(ProgramDataFolder))
                Directory.CreateDirectory(ProgramDataFolder);

            var firstTime = !File.Exists(MasterFilePath);

            var context = new MasterDbContext();
            context.Database.Migrate();

            if (firstTime)
            {
                var filePath = ProgramDataFolder;
#if DEBUG
                var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
                if (directoryInfo != null)
                    if (directoryInfo.Parent != null)
                        filePath = directoryInfo.Parent.FullName;
#endif
                SaveHousehold(new Household
                {
                    Name = "John and Jane Doe Family (Demo)",
                    FilePath = filePath,
                    FileName = DemoDataFileName
                });
            }
        }

        public static IEnumerable<Household> GetHouseholds()
        {
            var context = new MasterDbContext();
            return context.Households.OrderBy(p => p.Name);
        }

        public static Household GetDefaultHousehold()
        {
            var context = new MasterDbContext();
            return context.Households.FirstOrDefault(f => f.IsDefault);
        }

        public static bool SaveHousehold(Household household)
        {
            var context = new MasterDbContext();
            return context.SaveEntity(context.Households, household, "Saving Household");
        }

        public static bool DeleteHousehold(int householdId)
        {
            var context = new MasterDbContext();
            var household = context.Households.FirstOrDefault(f => f.Id == householdId);
            return context.DeleteEntity(context.Households, household, "Deleting Household");
        }
    }
}
