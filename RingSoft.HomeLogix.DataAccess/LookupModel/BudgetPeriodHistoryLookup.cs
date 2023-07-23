using System;

namespace RingSoft.HomeLogix.DataAccess.LookupModel
{
    public class BudgetPeriodHistoryLookup
    {
        public string BudgetItem { get; set; }

        public DateTime PeriodEndingDate { get; set; }

        public double ProjectedAmount { get; set; }

        public double ActualAmount { get; set; }

        public double Difference { get; set; }
    }
}
