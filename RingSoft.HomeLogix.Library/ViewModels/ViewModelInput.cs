using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System.Collections.Generic;
using RingSoft.HomeLogix.DataAccess.Model;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public class ViewModelInput
    {
        public List<BudgetItemViewModel> BudgetItemViewModels { get; } = new List<BudgetItemViewModel>();

        public List<BankAccountViewModel> BankAccountViewModels { get; } = new List<BankAccountViewModel>();

        public bool FromRegisterGrid { get; set; }

        public BudgetItemTypes? LockBudgetItemType { get; set; }

        public bool SourceHistoryIsIncome { get; set; }

        public BudgetItem HistoryFilterBudgetItem { get; set; }

        public BudgetPeriodHistory HistoryFilterBudgetPeriodItem { get; set; }

        public BankAccount HistoryFilterBankAccount { get; set; }

        public  BankAccountPeriodHistory HistoryFilterBankAccountPeriod { get; set; }
    }
}
