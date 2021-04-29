using RingSoft.HomeLogix.Library.ViewModels.Budget;
using System.Collections.Generic;

namespace RingSoft.HomeLogix.Library.ViewModels
{
    public class ViewModelInput
    {
        public List<BudgetItemViewModel> BudgetItemViewModels { get; } = new List<BudgetItemViewModel>();

        public List<BankAccountViewModel> BankAccountViewModels { get; } = new List<BankAccountViewModel>();

        public bool FromRegisterGrid { get; set; }
    }
}
