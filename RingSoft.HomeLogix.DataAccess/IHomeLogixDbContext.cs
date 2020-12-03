using Microsoft.EntityFrameworkCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public interface IHomeLogixDbContext
    {
        DbContext DbContext { get; }

        DbSet<SystemMaster> SystemMaster { get; set; }
        DbSet<BudgetItem> BudgetItems { get; set; }
        DbSet<BankAccount> BankAccounts { get; set; }

    }
}
