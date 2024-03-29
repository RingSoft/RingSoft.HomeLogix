﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class History
    {
        [Required]
        [Key]
        public int Id { get; set; }

        public int? BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ItemType { get; set; }

        public int? BudgetItemId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        public int? TransferToBankAccountId { get; set; }

        public virtual BankAccount TransferToBankAccount { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [Required]
        public double ProjectedAmount { get; set; }

        [Required]
        public double ActualAmount { get; set; }

        [MaxLength(250)]
        public string BankText { get; set; }

        public ICollection<SourceHistory> Sources { get; set; }

        public History()
        {
            Sources = new HashSet<SourceHistory>();
        }
    }
}
