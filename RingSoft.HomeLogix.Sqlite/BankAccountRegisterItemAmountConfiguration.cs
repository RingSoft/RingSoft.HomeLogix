using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class BankAccountRegisterItemAmountConfiguration 
        : IEntityTypeConfiguration<BankAccountRegisterItemAmountDetail>
    {
        public void Configure(EntityTypeBuilder<BankAccountRegisterItemAmountDetail> builder)
        {
            builder.Property(p => p.Amount).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.Date).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.DetailId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.RegisterId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.SourceId).HasColumnType(SqliteConstants.IntegerColumnType);

            builder.HasKey(p => new {p.RegisterId, p.DetailId});

            builder.HasOne(p => p.RegisterItem)
                .WithMany(p => p.AmountDetails)
                .HasForeignKey(p => p.RegisterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Source)
                .WithMany(p => p.AmountDetails)
                .HasForeignKey(p => p.SourceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
