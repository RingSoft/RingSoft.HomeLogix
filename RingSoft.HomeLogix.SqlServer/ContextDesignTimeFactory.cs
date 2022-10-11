using Microsoft.EntityFrameworkCore.Design;

namespace RingSoft.HomeLogix.SqlServer
{
    public class ContextDesignTimeFactory : IDesignTimeDbContextFactory<SqlServerHomeLogixDbContext>
    {
        SqlServerHomeLogixDbContext IDesignTimeDbContextFactory<SqlServerHomeLogixDbContext>.CreateDbContext(string[] args)
        {
            return new SqlServerHomeLogixDbContext() { IsDesignTime = true };
        }
    }
}
