using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.Property(p => p.AccountType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.CurrentBalance).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.LastCompletedDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.LastGenerationDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.MonthlyBudgetDeposits).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.MonthlyBudgetWithdrawals).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.PendingGeneration).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.ProjectedEndingBalance).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.ProjectedLowestBalanceAmount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.ProjectedLowestBalanceDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.ShowInGraph).HasColumnType(DbConstants.BoolColumnType);
        }
    }
}
