using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class BudgetItemConfiguration : IEntityTypeConfiguration<BudgetItem>
    {
        public void Configure(EntityTypeBuilder<BudgetItem> builder)
        {
            builder.Property(p => p.Amount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.Description).HasColumnType(SqliteConstants.StringColumnType);
            builder.Property(p => p.DoEscrow).HasColumnType(SqliteConstants.BoolColumnType);
            builder.Property(p => p.LastTransactionDate).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.NextTransactionDate).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.Notes).HasColumnType(SqliteConstants.MemoColumnType);
            builder.Property(p => p.RecurringType).HasColumnType(SqliteConstants.ByteColumnType);
            builder.Property(p => p.SpendingDayOfWeek).HasColumnType(SqliteConstants.ByteColumnType);
            builder.Property(p => p.SpendingType).HasColumnType(SqliteConstants.ByteColumnType);
            builder.Property(p => p.StartingDate).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.Type).HasColumnType(SqliteConstants.ByteColumnType);

            builder.HasOne(p => p.BankAccount)
                .WithMany(p => p.BudgetItems)
                .HasForeignKey(p => p.BankAccountId);

            builder.HasOne(p => p.EscrowBankAccount)
                .WithMany(p => p.BudgetEscrowItems)
                .HasForeignKey(p => p.EscrowBankAccountId);
        }
    }
}
