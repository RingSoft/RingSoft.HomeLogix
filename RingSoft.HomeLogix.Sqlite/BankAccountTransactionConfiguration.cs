using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class BankAccountTransactionConfiguration : IEntityTypeConfiguration<BankAccountTransaction>
    {
        public void Configure(EntityTypeBuilder<BankAccountTransaction> builder)
        {
            builder.Property(p => p.Amount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.BankAccountId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(SqliteConstants.StringColumnType);
            builder.Property(p => p.TransactionDate).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.TransactionId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.TransactionType).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.TransferToBankAccountId).HasColumnType(SqliteConstants.IntegerColumnType);

            builder.HasOne(p => p.BankAccount)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.BankAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.TransferToBankAccount)
                .WithMany(p => p.BankAccountTransferFromTransactions)
                .HasForeignKey(p => p.TransferToBankAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
