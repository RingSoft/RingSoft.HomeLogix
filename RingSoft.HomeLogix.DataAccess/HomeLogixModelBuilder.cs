using Microsoft.EntityFrameworkCore;
using RingSoft.App.Library;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;
using System.Collections.Generic;

namespace RingSoft.HomeLogix.DataAccess
{
    public class HomeLogixModelBuilder
    {
        public static DbContext DbContext { get; set; }

        public static void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SystemMasterConfiguration());
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
                .HasColumnType(DbConstants.IntegerColumnType);
            modelBuilder.Entity<BudgetItemSource>().Property(p => p.IsIncome)
                .HasColumnType(DbConstants.BoolColumnType);
            modelBuilder.Entity<BudgetItemSource>().Property(p => p.Name)
                .HasColumnType(DbConstants.StringColumnType);

            AdvancedFindDataProcessorEfCore.ConfigureAdvancedFind(modelBuilder);
        }

        public static bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            if (!DbContext.SaveNoCommitEntity(DbContext.Set<TEntity>(), entity, message))
                return false;

            return true;
        }

        public static bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DbContext.SaveEntity(DbContext.Set<TEntity>(), entity, message);
        }

        public static bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DbContext.DeleteEntity(DbContext.Set<TEntity>(), entity, message);
        }

        public static bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DbContext.AddNewNoCommitEntity(DbContext.Set<TEntity>(), entity, message);
        }

        public static bool Commit(string message)
        {
            var result = DbContext.SaveEfChanges(message);

            return result;
        }

        public static void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class
        {
            var dbSet = DbContext.Set<TEntity>();

            dbSet.RemoveRange(listToRemove);
        }

        public static void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class
        {
            var dbSet = DbContext.Set<TEntity>();

            dbSet.AddRange(listToAdd);
        }

        public static bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return DbContext.DeleteNoCommitEntity(DbContext.Set<TEntity>(), entity, message);
        }

    }
}
