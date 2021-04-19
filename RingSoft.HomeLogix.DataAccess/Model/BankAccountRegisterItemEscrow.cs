using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BankAccountRegisterItemEscrow
    {
        [Required]
        public int RegisterId { get; set; }

        public virtual BankAccountRegisterItem RegisterItem { get; set; }

        [Required]
        public int BudgetItemId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
