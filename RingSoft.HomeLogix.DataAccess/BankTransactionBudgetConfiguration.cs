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
            builder.Property(p => p.BudgetItemId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Amount).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.BankId, p.TransactionId, p.RowId });

            builder.HasOne(p => p.BankTransaction)
                .WithMany(p => p.BudgetItems)
                .HasForeignKey(p => new {p.BankId, p.TransactionId})
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.BudgetItem)
                .WithMany(p => p.TransactionBudgets)
                .HasForeignKey(p => p.BudgetItemId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
