using System;

namespace RingSoft.HomeLogix.DataAccess.LookupModel
{
    public class BankAccountPeriodHistoryLookup
    {
        public string BankAccount { get; set; }

        public DateTime PeriodEndingDate { get; set; }

        public decimal TotalDeposits { get; set; }

        public decimal TotalWithdrawals { get; set; }

        public decimal Difference { get; set; }
    }
}
