using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class BankAccountPeriodHistoryConfiguration : IEntityTypeConfiguration<BankAccountPeriodHistory>
    {
        public void Configure(EntityTypeBuilder<BankAccountPeriodHistory> builder)
        {
            builder.Property(p => p.BankAccountId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.PeriodEndingDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.PeriodType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.TotalDeposits).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.TotalWithdrawals).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(hk => new { hk.BankAccountId, hk.PeriodType, hk.PeriodEndingDate });

            builder.HasOne(h => h.BankAccount)
                .WithMany(w => w.PeriodHistory)
                .HasForeignKey(h => h.BankAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
