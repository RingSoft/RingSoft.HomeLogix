﻿using Microsoft.EntityFrameworkCore;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class HomeLogixDbContext : DbContext, IHomeLogixDbContext
    {
        public const string StringColumnType = "nvarchar";
        public DbContext DbContext => this;

        public virtual DbSet<SystemMaster> SystemMaster { get; set; }

        private static HomeLogixLookupContext _lookupContext;

        public HomeLogixDbContext(HomeLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
        }

        public HomeLogixDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_lookupContext.SqliteDataProcessor.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemMaster>().HasKey(p => p.HouseholdName);
            modelBuilder.Entity<SystemMaster>().Property(p => p.HouseholdName).HasColumnType(StringColumnType);

            base.OnModelCreating(modelBuilder);
        }
    }
}