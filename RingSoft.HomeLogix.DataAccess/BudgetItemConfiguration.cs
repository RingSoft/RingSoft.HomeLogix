using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class BudgetItemConfiguration : IEntityTypeConfiguration<BudgetItem>
    {
        public void Configure(EntityTypeBuilder<BudgetItem> builder)
        {
            builder.Property(p => p.Amount).HasColumnType(DbConstants.DecimalColumnType);          
            builder.Property(p => p.BankAccountId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.CurrentMonthAmount).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.CurrentMonthEnding).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.Description).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.EndingDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.LastCompletedDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.MonthOnDay).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.MonthlyAmount).HasColumnType(DbConstants.DecimalColumnType);          
            builder.Property(p => p.Notes).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.PayCCBalance).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.PayCCBalance).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.RecurringPeriod).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.RecurringType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.StartingDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.TransferToBankAccountId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Type).HasColumnType(DbConstants.ByteColumnType);

            builder.HasOne(p => p.BankAccount)
                .WithMany(p => p.BudgetItems)
                .HasForeignKey(p => p.BankAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.TransferToBankAccount)
                .WithMany(p => p.BudgetTransferFromItems)
                .HasForeignKey(p => p.TransferToBankAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.Description).IsUnique();
        }
    }
}
