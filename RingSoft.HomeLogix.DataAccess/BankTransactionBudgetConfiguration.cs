using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.HomeLogix.DataAccess
{
    public class BankTransactionBudgetConfiguration : IEntityTypeConfiguration<RingSoft.HomeLogix.DataAccess.Model.BankTransactionBudget>
    {
        public void Configure(EntityTypeBuilder<RingSoft.HomeLogix.DataAccess.Model.BankTransactionBudget> builder)
        {
            builder.Property(p => p.BankId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TransactionId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.RowId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.RegisterItemId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Amount).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.BankId, p.TransactionId, p.RowId });

            builder.HasOne(p => p.RegisterItem)
                .WithMany(p => p.BankTransactionBudgets)
                .HasForeignKey(p => p.RegisterItemId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
