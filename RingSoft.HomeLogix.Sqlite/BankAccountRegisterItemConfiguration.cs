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
            builder.Property(p => p.ActualAmount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.BankAccountId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.BudgetItemId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.Description).HasColumnType(SqliteConstants.StringColumnType);
            builder.Property(p => p.Id).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.IsEscrowFrom).HasColumnType(SqliteConstants.BoolColumnType);
            builder.Property(p => p.ItemDate).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.ItemType).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.ProjectedAmount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.RegisterGuid).HasColumnType(SqliteConstants.StringColumnType);
            builder.Property(p => p.TransferRegisterGuid).HasColumnType(SqliteConstants.StringColumnType);

            builder.HasOne(p => p.BankAccount)
                .WithMany(p => p.RegisterItems)
                .HasForeignKey(p => p.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.BudgetItem)
                .WithMany(p => p.RegisterItems)
                .HasForeignKey(p => p.BudgetItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
