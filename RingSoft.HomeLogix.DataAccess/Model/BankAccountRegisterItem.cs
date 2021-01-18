using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum BankAccountRegisterItemTypes
    {
        BudgetItem = 0,
        Miscellaneous = 1,
        TansferToBankAccount = 2,
        MonthlyEscrow = 3
    }

    public class BankAccountRegisterItem
    {
        [Required]
        [Key]
        [MaxLength(50)]
        public string RegisterId { get; set; }

        [Required]
        public int BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        [Required]
        public BankAccountRegisterItemTypes ItemType { get; set; }

        [Required]
        public DateTime ItemDate { get; set; }

        public int? BudgetItemId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        public int? TransferToBankAccountId { get; set; }

        public virtual BankAccount TransferToBankAccount { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public decimal ProjectedAmount { get; set; }

        public decimal? ActualAmount { get; set; }
    }
}