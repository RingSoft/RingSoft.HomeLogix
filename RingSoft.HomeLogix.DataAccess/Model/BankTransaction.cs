﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BankTransaction
    {
        [Required]
        public int BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public string BankTransactionText { get; set; }

        public int? BudgetId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        public int? SourceId { get; set; }

        [AllowNull]
        public virtual BudgetItemSource Source { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
