using Microsoft.EntityFrameworkCore.Design;

namespace RingSoft.HomeLogix.Sqlite
{
    public class ContextDesignTimeFactory : IDesignTimeDbContextFactory<HomeLogixDbContext>
    {
        HomeLogixDbContext IDesignTimeDbContextFactory<HomeLogixDbContext>.CreateDbContext(string[] args)
        {
            return new HomeLogixDbContext{IsDesignTime = true};
        }
    }
}
