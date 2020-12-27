using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class BankAccountRegisterItemConfiguration : IEntityTypeConfiguration<BankAccountRegisterItem>
    {
        public void Configure(EntityTypeBuilder<BankAccountRegisterItem> builder)
        {
            builder.Property(p => p.Amount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.BankAccountId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.BudgetItemId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(SqliteConstants.StringColumnType);
            builder.Property(p => p.RegisterId).HasColumnType(SqliteConstants.StringColumnType);
            builder.Property(p => p.TransactionDate).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.TransactionType).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.TransferToBankAccountId).HasColumnType(SqliteConstants.IntegerColumnType);

            builder.HasOne(p => p.BankAccount)
                .WithMany(p => p.RegisterItems)
                .HasForeignKey(p => p.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.BudgetItem)
                .WithMany(p => p.RegisterItems)
                .HasForeignKey(p => p.BudgetItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.TransferToBankAccount)
                .WithMany(p => p.BankAccountTransferFromRegisterItems)
                .HasForeignKey(p => p.TransferToBankAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
