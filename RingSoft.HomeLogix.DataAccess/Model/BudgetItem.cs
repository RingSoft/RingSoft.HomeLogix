using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum BudgetItemTypes
    {
        Income = 0,
        Expense = 1,
        Transfer = 2
    }

    public enum BudgetItemRecurringTypes
    {
        [Description("Day(s)")]
        Days = 0,
        [Description("Week(s)")]
        Weeks = 1,
        [Description("Month(s)")]
        Months = 2,
        [Description("Year(s)")]
        Years = 3
    }

    public class BudgetItem
    {
        [Required]
        [Key]
        public int Id { get; set; }

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

        public DateTime? StartingDate { get; set; }

        public DateTime? EndingDate { get; set; }

        public int? TransferToBankAccountId { get; set; }

        public virtual BankAccount TransferToBankAccount { get; set; }

        [Required]
        public decimal MonthlyAmount { get; set; }

        [Required]
        public decimal CurrentMonthAmount { get; set; }

        [Required]
        public DateTime CurrentMonthEnding { get; set; }

        public DateTime? LastCompletedDate { get; set; }

        public string Notes { get; set; }


        public virtual ICollection<BankAccountRegisterItem> RegisterItems { get; set; }

        public virtual ICollection<History> History { get; set; }

        public virtual ICollection<BudgetPeriodHistory> PeriodHistory { get; set; }

        public BudgetItem()
        {
            RegisterItems = new HashSet<BankAccountRegisterItem>();
            History = new HashSet<History>();
            PeriodHistory = new HashSet<BudgetPeriodHistory>();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Description) ? base.ToString() : Description;
        }
    }
}
