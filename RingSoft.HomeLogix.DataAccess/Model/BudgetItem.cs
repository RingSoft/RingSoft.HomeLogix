using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum BudgetItemTypes
    {
        Income = 0,
        Expense = 1
    }

    public enum BudgetItemRecurringType
    {
        Days = 0,
        Weeks = 1,
        Months = 2,
        Years = 3
    }

    public enum BudgetSpendingType
    {
        Days = 0,
        Weeks = 1
    }

    public class BudgetItem
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public BudgetItemTypes Type { get; set; }

        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int RecurringPeriod { get; set; }

        [Required]
        public BudgetItemRecurringType RecurringType { get; set; }

        [Required]
        public DateTime StartingDate { get; set; }

        public bool? DoEscrow { get; set; }

        public BudgetSpendingType? SpendingType { get; set; }

        public DayOfWeek? SpendingDayOfWeek { get; set; }

        public string Notes { get; set; }
    }
}
