﻿using System.Linq;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library
{
    public interface IDataRepository
    {
        [CanBeNull] SystemMaster GetSystemMaster();

        BankAccount GetBankAccount(int bankAccountId);

        bool SaveBankAccount(BankAccount bankAccount);

        bool DeleteBankAccount(int bankAccountId);
    }

    public class DataRepository : IDataRepository
    {
        [CanBeNull]
        public SystemMaster GetSystemMaster()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.SystemMaster.FirstOrDefault();
        }

        public BankAccount GetBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
        }

        public bool SaveBankAccount(BankAccount bankAccount)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.DbContext.SaveEntity(context.BankAccounts, bankAccount, "Saving Bank Account");
        }

        public bool DeleteBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            var bankAccount = context.BankAccounts.FirstOrDefault(f => f.Id == bankAccountId);
            return context.DbContext.DeleteEntity(context.BankAccounts, bankAccount, "Deleting Bank Account");
        }
    }
}
