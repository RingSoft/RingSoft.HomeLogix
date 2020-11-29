using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.HomeLogix.MasterData
{
    public class MasterDbContext : DbContext
    {
        //install Microsoft.EntityFrameworkCore.Tools NuGet

        //Restart Visual Studio.

        //Add-Migration <Name>

        public virtual DbSet<Households> Households { get; set; }

        public static string ProgramDataFolder => $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\RingSoft\\HomeLogix";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var fileName = $"{ProgramDataFolder}\\MasterDb.sqlite";
                optionsBuilder.UseSqlite($"Data Source={fileName}");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Households>(entity =>
            {
                entity.HasKey(hk => hk.Id);

                entity.Property(p => p.Id).HasColumnType("integer");

                entity.Property(p => p.Name).HasColumnType("nvarchar");

                entity.Property(p => p.FilePath).HasColumnType("nvarchar");

                entity.Property(p => p.FileName).HasColumnType("nvarchar");

                entity.Property(p => p.IsDefault).HasColumnType("bit");
            });

            base.OnModelCreating(modelBuilder);
        }

        public static void ConnectToMaster()
        {
            if (!Directory.Exists(ProgramDataFolder))
                Directory.CreateDirectory(ProgramDataFolder);

            var context = new MasterDbContext();
            context.Database.Migrate();
        }

        public static IEnumerable<Households> GetHouseholds()
        {
            var context = new MasterDbContext();
            return context.Households.OrderBy(p => p.Name);
        }

        public static bool SaveHousehold(Households household)
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
