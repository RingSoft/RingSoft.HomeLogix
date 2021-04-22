using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class SourceHistoryConfiguration : IEntityTypeConfiguration<SourceHistory>
    {
        public void Configure(EntityTypeBuilder<SourceHistory> builder)
        {
            builder.Property(p => p.Amount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.Date).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.DetailId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.HistoryId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.SourceId).HasColumnType(SqliteConstants.IntegerColumnType);

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
