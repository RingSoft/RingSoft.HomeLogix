using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

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

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)")
                    .HasMaxLength(50);

                entity.Property(p => p.FilePath)
                    .IsRequired()
                    .HasColumnType("nvarchar(250)")
                    .HasMaxLength(250);

                entity.Property(p => p.FileName)
                    .IsRequired()
                    .HasColumnType("nvarchar(250)")
                    .HasMaxLength(250);
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
    }
}
