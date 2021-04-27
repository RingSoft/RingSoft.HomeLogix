using System;

namespace RingSoft.HomeLogix.DataAccess.LookupModel
{
    public class BudgetPeriodHistoryLookup
    {
        public string BudgetItem { get; set; }

        public DateTime PeriodEndingDate { get; set; }

        public decimal ProjectedAmount { get; set; }

        public decimal ActualAmount { get; set; }

        public decimal Difference { get; set; }
    }
}
