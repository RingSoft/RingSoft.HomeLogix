using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class QifMap
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string BankText { get; set; }

        public int BudgetId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        public int? SourceId { get; set; }

        public virtual BudgetItemSource Source { get; set; }

        public virtual ICollection<BankTransaction> Transactions { get; set; }

        public QifMap()
        {
            Transactions = new HashSet<BankTransaction>();
        }
    }
}
