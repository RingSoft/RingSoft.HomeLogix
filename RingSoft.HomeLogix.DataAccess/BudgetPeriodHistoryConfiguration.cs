using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class BudgetPeriodHistoryConfiguration : IEntityTypeConfiguration<BudgetPeriodHistory>
    {
        public void Configure(EntityTypeBuilder<BudgetPeriodHistory> builder)
        {
            builder.Property(p => p.ActualAmount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.BudgetItemId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.PeriodEndingDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.PeriodType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.ProjectedAmount).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(hk => new {hk.BudgetItemId, hk.PeriodType, hk.PeriodEndingDate});

            builder.HasOne(p => p.BudgetItem)
                .WithMany(w => w.PeriodHistory)
                .HasForeignKey(h => h.BudgetItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
