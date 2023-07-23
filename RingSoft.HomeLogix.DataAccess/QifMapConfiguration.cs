using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class QifMapConfiguration : IEntityTypeConfiguration<QifMap>
    {
        public void Configure(EntityTypeBuilder<QifMap> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.BankText).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.BudgetId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.SourceId).HasColumnType(DbConstants.IntegerColumnType).IsNullable();

            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.BudgetItem)
                .WithMany(p => p.Maps)
                .HasForeignKey(p => p.BudgetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Source)
                .WithMany(p => p.Maps)
                .HasForeignKey(p => p.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
