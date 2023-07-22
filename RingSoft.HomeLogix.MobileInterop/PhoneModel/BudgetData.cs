using System;

namespace RingSoft.HomeLogix.Library.PhoneModel
{
    public class BudgetData
    {
        public int BudgetItemId { get; set; }

        public string Description { get; set; }

        public double BudgetAmount { get; set; }

        public double ActualAmount { get; set; }

        public decimal Difference { get; set; }

        public bool HistoryExists { get; set; }

        public DateTime CurrentDate { get; set; }
    }
}
