using System;

namespace RingSoft.HomeLogix.Library.PhoneModel
{
    public class HistoryData
    {
        public int HistoryId { get; set; }

        public int? BankAccountId { get; set; }

        public string BankName { get; set; }

        public int? BudgetItemId { get; set; }

        public string BudgetName { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public decimal ProjectedAmount { get; set; }

        public decimal ActualAmount { get; set; }

        public decimal Difference { get; set; }

        public string BankText { get; set; }

        public bool HasSourceHistory { get; set; }
    }
}
