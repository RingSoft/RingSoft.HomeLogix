﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

    public enum BudgetItemSpendingTypes
    {
        Month = 0,
        Day = 1,
        Week = 2
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

        public DateTime? EndingDate { get; set; }

        public bool? DoEscrow { get; set; }

        public int? TransferToBankAccountId { get; set; }

        public virtual BankAccount TransferToBankAccount { get; set; }

        public BudgetItemSpendingTypes SpendingType { get; set; }

        public DayOfWeek SpendingDayOfWeek { get; set; }

        public DateTime? LastCompletedDate { get; set; }

        public DateTime? NextTransactionDate { get; set; }

        public decimal? SpendingAmount { get; set; }
    }
}
