using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum BudgetItemTypes
    {
        Income = 0,
        Expense = 1
    }

    public enum BudgetItemRecurringTypes
    {
        Days = 0,
        Weeks = 1,
        Months = 2,
        Years = 3
    }

    public enum BudgetSpendingTypes
    {
        Months = 0,
        Days = 1,
        Weeks = 2
    }

    public class BudgetItem
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int Index { get; set; }

        [Required]
        public BudgetItemTypes Type { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public int BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int RecurringPeriod { get; set; }

        [Required]
        public BudgetItemRecurringTypes RecurringType { get; set; }

        [Required]
        public DateTime StartingDate { get; set; }

        public bool? DoEscrow { get; set; }

        public int? EscrowBankAccountId { get; set; }

        public virtual BankAccount EscrowBankAccount { get; set; }

        public BudgetSpendingTypes? SpendingType { get; set; }

        public DayOfWeek? SpendingDayOfWeek { get; set; }

        public string Notes { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        public DateTime? NextTransactionDate { get; set; }
    }
}
