using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class MainBudget
    {
        [Required]
        [Key]
        public int BudgetItemId { get; set; }

        public virtual BudgetItem BudgetItem { get; set; }

        [Required]
        public byte ItemType { get; set; }

        [Required]
        public double BudgetAmount { get; set; }

        [Required]
        public double ActualAmount { get; set; }

    }

    public class MainBudgetLookup
    {
        public string BudgetItem { get; set; }

        public byte ItemType { get; set; }

        public double BudgetAmount { get; set; }

        public double ActualAmount { get; set; }

        public double Difference { get; set; }

        public int BudgetItemId { get; set; }
    }
}
