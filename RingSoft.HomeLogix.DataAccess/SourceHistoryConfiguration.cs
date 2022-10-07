using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class SourceHistoryConfiguration : IEntityTypeConfiguration<SourceHistory>
    {
        public void Configure(EntityTypeBuilder<SourceHistory> builder)
        {
            builder.Property(p => p.Amount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Date).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.DetailId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.HistoryId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.SourceId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasKey(p => new { p.HistoryId, p.DetailId });

            builder.HasOne(p => p.HistoryItem)
                .WithMany(w => w.Sources)
                .HasForeignKey(h => h.HistoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Source)
                .WithMany(w => w.History)
                .HasForeignKey(h => h.SourceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
