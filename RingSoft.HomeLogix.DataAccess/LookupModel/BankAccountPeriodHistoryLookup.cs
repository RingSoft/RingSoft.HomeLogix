using System;

namespace RingSoft.HomeLogix.DataAccess.LookupModel
{
    public class BankAccountPeriodHistoryLookup
    {
        public string BankAccount { get; set; }

        public int Year { get; set; }

        public DateTime PeriodEndingDate { get; set; }

        public double TotalDeposits { get; set; }

        public double TotalWithdrawals { get; set; }

        public double Difference { get; set; }
    }
}
