﻿using System;
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

        [Required]
        public DateTime StartingDate { get; set; }

        public DateTime? EndingDate { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool DoEscrow { get; set; }

        public int? TransferToBankAccountId { get; set; }

        public virtual BankAccount TransferToBankAccount { get; set; }

        public DateTime? LastCompletedDate { get; set; }

        [Required]
        public DateTime NextTransactionDate { get; set; }

        [Required]
        public decimal MonthlyAmount { get; set; }

        [Required]
        public decimal CurrentMonthAmount { get; set; }

        [Required]
        public decimal PreviousMonthAmount { get; set; }

        [Required]
        public decimal CurrentYearAmount { get; set; }

        [Required]
        public decimal PreviousYearAmount { get; set; }

        public decimal? EscrowBalance { get; set; }

        public string Notes { get; set; }

        public virtual ICollection<BankAccountRegisterItem> RegisterItems { get; set; }

        public BudgetItem()
        {
            RegisterItems = new HashSet<BankAccountRegisterItem>();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Description) ? base.ToString() : Description;
        }
    }
}
