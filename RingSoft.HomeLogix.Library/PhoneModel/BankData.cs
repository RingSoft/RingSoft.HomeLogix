﻿using System;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Library.PhoneModel
{
    public class BankData
    {
        public int BankId { get; set; }
        
        public string Description { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal ProjectedLowestBalance { get; set; }

        public DateTime ProjectedLowestBalanceDate { get; set; }

        public BankAccountTypes AccountType { get; set; }

    }
}