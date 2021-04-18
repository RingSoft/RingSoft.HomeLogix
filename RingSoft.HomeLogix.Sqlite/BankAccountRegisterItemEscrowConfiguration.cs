using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.App.Library;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Sqlite
{
    public class BankAccountRegisterItemEscrowConfiguration
        : IEntityTypeConfiguration<BankAccountRegisterItemEscrow>
    {
        public void Configure(EntityTypeBuilder<BankAccountRegisterItemEscrow> builder)
        {
            builder.Property(p => p.BudgetItemId).HasColumnType(SqliteConstants.IntegerColumnType);
            builder.Property(p => p.RegisterId).HasColumnType(SqliteConstants.IntegerColumnType);

            builder.HasKey(p => new {p.RegisterId, p.BudgetItemId});

            builder.HasOne(p => p.RegisterItem)
                .WithMany(p => p.Escrows)
                .HasForeignKey(p => p.RegisterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.BudgetItem)
                .WithMany(p => p.RegisterEscrows)
                .HasForeignKey(p => p.BudgetItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
