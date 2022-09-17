using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BudgetItemSource
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsIncome { get; set; }

        public virtual ICollection<BankAccountRegisterItemAmountDetail> AmountDetails { get; set; }

        public virtual ICollection<SourceHistory> History { get; set; }

        public ICollection<BankTransaction> Transactions { get; set; }

        public BudgetItemSource()
        {
            AmountDetails = new HashSet<BankAccountRegisterItemAmountDetail>();
            History = new HashSet<SourceHistory>();
            Transactions = new HashSet<BankTransaction>();
        }
    }
}
