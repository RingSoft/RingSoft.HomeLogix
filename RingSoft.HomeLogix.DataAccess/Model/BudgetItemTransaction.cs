using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BudgetItemTransaction
    {
        [Required]
        [Key]
        public int BudgetItemId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        [Required]
        [Key]
        public DateTime TransactionDate { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
