using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BankTransactionBudget
    {
        [Required]
        public int BankId { get; set; }

        [Required]
        public int TransactionId { get; set; }

        public virtual BankTransaction BankTransaction { get; set; }

        [Required]
        public int RowId { get; set; }

        [Required]
        public int BudgetItemId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
