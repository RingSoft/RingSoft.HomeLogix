using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum PeriodHistoryTypes
    {
        Monthly = 0,
        Yearly = 1
    }

    public class BudgetPeriodHistory
    {
        [Required]
        public int BudgetItemId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        [Required]
        public byte PeriodType { get; set; }

        [Required]
        public DateTime PeriodEndingDate { get; set; }

        [Required]
        public double ProjectedAmount { get; set; }

        [Required]
        public double ActualAmount { get; set; }
    }
}
