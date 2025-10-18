using System;
using System.Collections.Generic;
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

        public string Description { get; set; }

        //public int? BudgetId { get; set; }

        //public virtual BudgetItem BudgetItem { get; set; }

        public int? RegisterId { get; set; }

        public virtual BankAccountRegisterItem RegisterItem { get; set; }

        public int? SourceId { get; set; }

        [AllowNull]
        public virtual BudgetItemSource Source { get; set; }

        [Required]
        public double Amount { get; set; }

        public int? QifMapId { get; set; }

        public virtual QifMap QifMap { get; set; }

        public bool MapTransaction { get; set; }

        public byte TransactionType { get; set; }

        public bool FromBank { get; set; }

        public virtual ICollection<BankTransactionBudget> BudgetItems { get; set; }

        public BankTransaction()
        {
            BudgetItems = new HashSet<BankTransactionBudget>();
        }
    }
}
