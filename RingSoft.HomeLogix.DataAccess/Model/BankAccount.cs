using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BankAccount
    {
        public BankAccount()
        {
            BudgetItems = new HashSet<BudgetItem>();
            BudgetEscrowItems = new HashSet<BudgetItem>();
        }

        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public decimal CurrentBalance { get; set; }

        public string Notes { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public virtual ICollection<BudgetItem> BudgetEscrowItems { get; set; }
    }
}
