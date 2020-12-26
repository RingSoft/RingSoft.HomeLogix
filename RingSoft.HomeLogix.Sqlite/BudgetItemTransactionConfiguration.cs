using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class BudgetItemTransactionConfiguration : IEntityTypeConfiguration<BudgetItemTransaction>
    {
        public void Configure(EntityTypeBuilder<BudgetItemTransaction> builder)
        {
            builder.Property(p => p.Amount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.BudgetItemId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.TransactionDate).HasColumnType(SqliteConstants.DateColumnType);

            builder.HasOne(p => p.BudgetItem)
                .WithMany(p => p.RegisterItems)
                .HasForeignKey(p => p.BudgetItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
