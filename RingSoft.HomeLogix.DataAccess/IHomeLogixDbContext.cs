﻿using Microsoft.EntityFrameworkCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.DataAccess
{
    public interface IHomeLogixDbContext : DbLookup.IDbContext, IDbContext
    {
        DbContext DbContext { get; }

        DbSet<SystemMaster> SystemMaster { get; set; }
        DbSet<BudgetItem> BudgetItems { get; set; }
        DbSet<BankAccount> BankAccounts { get; set; }
        DbSet<BankAccountRegisterItem> BankAccountRegisterItems { get; set; }
        DbSet<BankAccountRegisterItemAmountDetail> BankAccountRegisterItemAmountDetails { get; set; }
        DbSet<BudgetItemSource> BudgetItemSources { get; set; }
        DbSet<History> History { get; set; }
        DbSet<SourceHistory> SourceHistory { get; set; }
        DbSet<BudgetPeriodHistory> BudgetPeriodHistory { get; set; }
        DbSet<BankAccountPeriodHistory> BankAccountPeriodHistory { get; set; }
        DbSet<BankTransaction> BankTransactions { get; set; }
        DbSet<BankTransactionBudget> BankTransactionBudget { get; set; }
        DbSet<QifMap> QifMaps { get; set; }
        DbSet<MainBudget> MainBudget { get; set; }

        void SetLookupContext(HomeLogixLookupContext lookupContext);
    }
}
