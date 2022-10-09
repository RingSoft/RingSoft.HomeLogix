using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class HistoryConfiguration : IEntityTypeConfiguration<History>
    {
        public void Configure(EntityTypeBuilder<History> builder)
        {
            builder.Property(p => p.ActualAmount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.BankAccountId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.BankText).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.BudgetItemId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Date).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ItemType).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ProjectedAmount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.TransferToBankAccountId).HasColumnType(DbConstants.IntegerColumnType);

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
