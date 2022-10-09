using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class SourceHistory
    {
        [Required]
        public int HistoryId { get; set; }

        public virtual History HistoryItem { get; set; }

        [Required]
        public int DetailId { get; set; }

        [Required]
        public int SourceId { get; set; }

        public virtual BudgetItemSource Source { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [MaxLength(250)]
        public string BankText { get; set; }
    }
}
