using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class BankAccountPeriodHistoryConfiguration : IEntityTypeConfiguration<BankAccountPeriodHistory>
    {
        public void Configure(EntityTypeBuilder<BankAccountPeriodHistory> builder)
        {
            builder.Property(p => p.BankAccountId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.PeriodEndingDate).HasColumnType(SqliteConstants.DateColumnType);
            builder.Property(p => p.PeriodType).HasColumnType(SqliteConstants.ByteColumnType);
            builder.Property(p => p.TotalDeposits).HasColumnType(SqliteConstants.DecimalColumnType);
            builder.Property(p => p.TotalWithdrawals).HasColumnType(SqliteConstants.DecimalColumnType);

            builder.HasKey(hk => new { hk.BankAccountId, hk.PeriodType, hk.PeriodEndingDate });

            builder.HasOne(h => h.BankAccount)
                .WithMany(w => w.PeriodHistory)
                .HasForeignKey(h => h.BankAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
