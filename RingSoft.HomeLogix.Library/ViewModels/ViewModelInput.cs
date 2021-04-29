using System.Collections.Generic;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public class ViewModelInput
    {
        public List<BudgetItemViewModel> BudgetItemViewModels { get; } = new List<BudgetItemViewModel>();

        public List<BankAccountViewModel> BankAccountViewModels { get; } = new List<BankAccountViewModel>();

        public bool FromRegisterGrid { get; set; }

        public int LockBudgetBankAccountId { get; set; }

        public PrimaryKeyValue SelectBudgetPrimaryKeyValue { get; set; }
    }
}
