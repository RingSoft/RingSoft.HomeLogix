using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BankAccountPeriodHistory
    {
        [Required]
        public int BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }
        
        [Required]
        public byte PeriodType { get; set; }

        [Required]
        public DateTime PeriodEndingDate { get; set; }

        [Required]
        public double TotalDeposits { get; set; }

        [Required]
        public double TotalWithdrawals { get; set; }
    }
}
