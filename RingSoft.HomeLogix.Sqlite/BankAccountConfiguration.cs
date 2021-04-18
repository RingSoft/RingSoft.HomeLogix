using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.Property(p => p.CurrentBalance).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.Description).HasColumnType(SqliteConstants.StringColumnType);
            builder.Property(p => p.EscrowBalance).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.EscrowDayOfMonth).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.EscrowToBankAccountId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.Id).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.LastGenerationDate).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.MonthlyBudgetDeposits).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.MonthlyBudgetWithdrawals).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.Notes).HasColumnType(SqliteConstants.MemoColumnType);
            builder.Property(p => p.ProjectedEndingBalance).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.ProjectedLowestBalanceAmount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.ProjectedLowestBalanceDate).HasColumnType(SqliteConstants.DateColumnType);

            builder.HasOne(p => p.EscrowToBankAccount)
                .WithMany(p => p.EscrowFromBankAccounts)
                .HasForeignKey(p => p.EscrowToBankAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
