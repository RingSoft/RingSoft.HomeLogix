using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public class SystemMasterConfiguration : IEntityTypeConfiguration<SystemMaster>
    {
        public void Configure(EntityTypeBuilder<SystemMaster> builder)
        {
            builder.Property(p => p.HouseholdName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PhoneLogin).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PhonePassword).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.AppGuid).HasColumnType(DbConstants.StringColumnType);
        }
    }
}
