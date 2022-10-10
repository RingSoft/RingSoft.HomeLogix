using Microsoft.EntityFrameworkCore.Design;

namespace RingSoft.HomeLogix.Sqlite
{
    public class ContextDesignTimeFactory : IDesignTimeDbContextFactory<SqliteHomeLogixDbContext>
    {
        SqliteHomeLogixDbContext IDesignTimeDbContextFactory<SqliteHomeLogixDbContext>.CreateDbContext(string[] args)
        {
            return new SqliteHomeLogixDbContext{IsDesignTime = true};
        }
    }
}
