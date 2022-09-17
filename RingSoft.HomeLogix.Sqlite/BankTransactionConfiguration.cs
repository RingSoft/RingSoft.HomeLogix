using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransaction>
    {
        public void Configure(EntityTypeBuilder<BankTransaction> builder)
        {
            builder.Property(p => p.BankAccountId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TransactionId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TransactionDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.BankTransactionText).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.BudgetId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.SourceId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Amount).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.BankAccountId, p.TransactionId });

            builder.HasOne(p => p.BankAccount)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.BudgetItem)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Source)
                .WithMany(p => p.Transactions)
                .HasForeignKey(p => p.SourceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
