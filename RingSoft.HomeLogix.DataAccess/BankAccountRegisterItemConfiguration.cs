using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class BankAccountRegisterItemConfiguration : IEntityTypeConfiguration<BankAccountRegisterItem>
    {
        public void Configure(EntityTypeBuilder<BankAccountRegisterItem> builder)
        {
            builder.Property(p => p.ActualAmount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.BankAccountId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.BankText).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.BudgetItemId).HasColumnType(DbConstants.IntegerColumnType).IsNullable();
            builder.Property(p => p.Completed).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.IsNegative).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.ItemDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.ItemType).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ProjectedAmount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.RegisterGuid).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.TransferRegisterGuid).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.IsTransferMisc).HasColumnType(DbConstants.BoolColumnType);

            builder.HasOne(p => p.BankAccount)
                .WithMany(p => p.RegisterItems)
                .HasForeignKey(p => p.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.ItemDate);

            builder.HasIndex(p => p.Description);
        }
    }
}
