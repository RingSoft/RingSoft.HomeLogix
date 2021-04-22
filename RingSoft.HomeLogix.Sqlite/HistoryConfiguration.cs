using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class HistoryConfiguration : IEntityTypeConfiguration<History>
    {
        public void Configure(EntityTypeBuilder<History> builder)
        {
            builder.Property(p => p.ActualAmount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.BankAccountId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.BudgetItemId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.Date).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.Description).HasColumnType(SqliteConstants.StringColumnType);
            builder.Property(p => p.Id).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.ItemType).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.ProjectedAmount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.TransferToBankAccountId).HasColumnType(SqliteConstants.IntegerColumnType);

            builder.HasOne(p => p.BankAccount)
                .WithMany(w => w.History)
                .HasForeignKey(h => h.BankAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.BudgetItem)
                .WithMany(w => w.History)
                .HasForeignKey(h => h.BudgetItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.TransferToBankAccount)
                .WithMany(w => w.TransferToHistory)
                .HasForeignKey(h => h.TransferToBankAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
