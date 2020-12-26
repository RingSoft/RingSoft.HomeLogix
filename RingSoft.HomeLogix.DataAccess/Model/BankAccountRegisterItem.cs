using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum BankAccountTransactionTypes
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
        public string RegisterId { get; set; }

        [Required]
        public int BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        [Required]
        public BankAccountTransactionTypes TransactionType { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public int? BudgetItemId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public int? TransferToBankAccountId { get; set; }

        public virtual BankAccount TransferToBankAccount { get; set; }
    }
}