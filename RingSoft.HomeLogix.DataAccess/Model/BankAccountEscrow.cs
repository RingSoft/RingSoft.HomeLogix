using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public enum EscrowRecurringTypes
    {
        [Description("Day(s)")]
        Days = 0,
        [Description("Week(s)")]
        Weeks = 1,
        [Description("Month")]
        Months = 2,
    }

    public class BankAccountEscrow
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}
