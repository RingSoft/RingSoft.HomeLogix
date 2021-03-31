using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.HomeLogix.DataAccess.Model;
using System.Linq;

namespace RingSoft.HomeLogix.Library
{
    public interface IDataRepository
    {
        [CanBeNull] SystemMaster GetSystemMaster();

        BankAccount GetBankAccount(int bankAccountId, bool getRelatedEntities = true);

        bool SaveBankAccount(BankAccount bankAccount);

        bool DeleteBankAccount(int bankAccountId);

        List<BudgetItem> GetBudgetItemsForBankAccount(int bankAccountId);

        BudgetItem GetBudgetItem(int budgetItemId);

        bool SaveBudgetItem(BudgetItem budgetItem, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount,
            BankAccount escrowBankAccount, BankAccount dbEscrowBankAccount);

        bool DeleteBudgetItem(int budgetItemId, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount,
            BankAccount dbEscrowBankAccount);

        IEnumerable<BudgetItem> GetEscrowBudgetItems(int bankAccountId);

        bool SaveGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
            IEnumerable<BudgetItem> budgetItems,
            List<BankAccountRegisterItem> registerItemsToDelete = null,
            BankAccount bankAccount = null);
    }

    public class DataRepository : IDataRepository
    {
        [CanBeNull]
        public SystemMaster GetSystemMaster()
        {
            var context = AppGlobals.GetNewDbContext();
            return context.SystemMaster.FirstOrDefault();
        }

        public BankAccount GetBankAccount(int bankAccountId, bool getRelatedEntities = true)
        {
            var context = AppGlobals.GetNewDbContext();

            if (getRelatedEntities)
            {
                return context.BankAccounts.Include(i => i.RegisterItems)
                    .ThenInclude(ti => ti.BudgetItem)
                    .ThenInclude(ti => ti.TransferToBankAccount)
                    .Include(i => i.EscrowToBankAccount)
                    .FirstOrDefault(f => f.Id == bankAccountId);
            }

            return context.BankAccounts.Include(i => i.EscrowToBankAccount)
                .FirstOrDefault(f => f.Id == bankAccountId);
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

        public List<BudgetItem> GetBudgetItemsForBankAccount(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItems.Where(w => w.BankAccountId == bankAccountId
                                                  || w.TransferToBankAccountId == bankAccountId).ToList();
        }

        public BudgetItem GetBudgetItem(int budgetItemId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItems
                .Include(i => i.BankAccount)
                .Include(i => i.TransferToBankAccount)
                .FirstOrDefault(f => f.Id == budgetItemId);
        }

        public bool SaveBudgetItem(BudgetItem budgetItem, BankAccount dbBankAccount,
            BankAccount dbTransferToBankAccount, BankAccount escrowBankAccount, 
            BankAccount dbEscrowBankAccount)
        {
            var context = AppGlobals.GetNewDbContext();
            if (!context.DbContext.SaveNoCommitEntity(context.BudgetItems, budgetItem, "Saving Budget Item"))
                return false;

            if (dbBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbBankAccount, "Saving Bank Account"))
                    return false;
            }

            if (dbTransferToBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbTransferToBankAccount, "Saving Transfer To Bank Account"))
                    return false;
            }

            if (escrowBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, escrowBankAccount,
                    "Saving Escrow Bank Account"))
                    return false;
            }

            if (dbEscrowBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbEscrowBankAccount,
                    "Database Saving Escrow Bank Account"))
                    return false;
            }

            return context.DbContext.SaveEfChanges("Saving Budget Item");
        }

        public bool DeleteBudgetItem(int budgetItemId, BankAccount dbBankAccount, BankAccount dbTransferToBankAccount,
            BankAccount dbEscrowBankAccount)
        {
            var context = AppGlobals.GetNewDbContext();
            if (dbBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbBankAccount, "Saving Bank Account"))
                    return false;
            }

            if (dbTransferToBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbTransferToBankAccount, "Saving Transfer To Bank Account"))
                    return false;
            }

            if (dbEscrowBankAccount != null)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, dbEscrowBankAccount,
                    "Database Saving Escrow Bank Account"))
                    return false;
            }

            var budgetItem = context.BudgetItems.FirstOrDefault(f => f.Id == budgetItemId);
            return context.DbContext.DeleteEntity(context.BudgetItems, budgetItem, "Deleting Budget Item");
        }

        public IEnumerable<BudgetItem> GetEscrowBudgetItems(int bankAccountId)
        {
            var context = AppGlobals.GetNewDbContext();
            return context.BudgetItems.Where(w => w.BankAccountId == bankAccountId && w.DoEscrow);
        }

        public bool SaveGeneratedRegisterItems(IEnumerable<BankAccountRegisterItem> newBankRegisterItems,
            IEnumerable<BudgetItem> budgetItems,
            List<BankAccountRegisterItem> registerItemsToDelete = null,
            BankAccount bankAccount = null)
        {
            var context = AppGlobals.GetNewDbContext();

            if (registerItemsToDelete != null && registerItemsToDelete.Any())
                context.DbContext.RemoveRange(registerItemsToDelete);

            if (bankAccount != null)
                if (!context.DbContext.SaveNoCommitEntity(context.BankAccounts, bankAccount,
                    $"Saving Bank Account '{bankAccount.Description}'"))
                    return false;

            foreach (var budgetItem in budgetItems)
            {
                if (!context.DbContext.SaveNoCommitEntity(context.BudgetItems, budgetItem,
                    $"Saving Budget Item '{budgetItem.Description}'"))
                    return false;
            }

            foreach (var bankAccountRegisterItem in newBankRegisterItems)
            {
                context.BankAccountRegisterItems.Add(bankAccountRegisterItem);
            }

            return context.DbContext.SaveEfChanges("Saving generated Bank Account Register Items");
        }
    }
}
