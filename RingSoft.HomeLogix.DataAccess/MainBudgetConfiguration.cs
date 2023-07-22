using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class MainBudgetConfiguration : IEntityTypeConfiguration<MainBudget>
    {
        public void Configure(EntityTypeBuilder<MainBudget> builder)
        {
            builder.Property(p => p.BudgetItemId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ItemType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.BudgetAmount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.ActualAmount).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasOne(p => p.BudgetItem)
                .WithMany(p => p.MainBudgets)
                .HasForeignKey(p => p.BudgetItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
