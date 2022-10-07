﻿using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class HomeLogixModelBuilder
    {
        public static void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemMaster>().Property(p => p.HouseholdName)
                .HasColumnType(SqliteConstants.StringColumnType);

            modelBuilder.ApplyConfiguration(new BudgetItemConfiguration());
            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
            modelBuilder.ApplyConfiguration(new BankAccountRegisterItemConfiguration());
            modelBuilder.ApplyConfiguration(new BankAccountRegisterItemAmountConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryConfiguration());
            modelBuilder.ApplyConfiguration(new SourceHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetPeriodHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new BankAccountPeriodHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new BankTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new BankTransactionBudgetConfiguration());
            modelBuilder.ApplyConfiguration(new QifMapConfiguration());

            modelBuilder.Entity<BudgetItemSource>().Property(p => p.Id)
                .HasColumnType(SqliteConstants.IntegerColumnType);
            modelBuilder.Entity<BudgetItemSource>().Property(p => p.IsIncome)
                .HasColumnType(SqliteConstants.BoolColumnType);
            modelBuilder.Entity<BudgetItemSource>().Property(p => p.Name)
                .HasColumnType(SqliteConstants.StringColumnType);

            AdvancedFindDataProcessorEfCore.ConfigureAdvancedFind(modelBuilder);



        }
    }
}
