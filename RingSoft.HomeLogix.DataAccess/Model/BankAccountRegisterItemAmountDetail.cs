﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BankAccountRegisterItemAmountDetail
    {
        [Required]
        public int RegisterId { get; set; }

        public virtual BankAccountRegisterItem RegisterItem { get; set; }

        [Required]
        public int DetailId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int SourceId { get; set; }

        public virtual BudgetItemSource Source { get; set; }

        [Required]
        public double Amount { get; set; }

        [MaxLength(250)]
        public string BankText { get; set; }

        public ICollection<BankTransaction> Transactions { get; set; }

        public BankAccountRegisterItemAmountDetail()
        {
            Transactions = new HashSet<BankTransaction>();
        }
    }
}
