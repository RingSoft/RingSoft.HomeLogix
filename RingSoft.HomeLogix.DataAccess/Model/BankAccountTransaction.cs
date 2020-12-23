using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum BankAccountTransactionTypes
    {
        Miscellaneous = 0,
        TansferToBankAccount = 1,
        MonthlyEscrow = 2
    }

    public class BankAccountTransaction
    {
        [Required]
        [Key]
        public int BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        [Required]
        [Key]
        public BankAccountTransactionTypes TransactionType { get; set; }

        [Required]
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public int TransferToBankAccountId { get; set; }

        public virtual BankAccount TransferToBankAccount { get; set; }
    }
}