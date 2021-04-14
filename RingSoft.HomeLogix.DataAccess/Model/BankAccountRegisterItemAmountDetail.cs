using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BankAccountRegisterItemAmountDetail
    {
        [Required]
        public int RegisterId { get; set; }

        public virtual BankAccountRegisterItem RegisterItem { get; set; }

        [Required]
        public int DetailId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int SourceId { get; set; }

        public virtual BudgetItemSource Source { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
