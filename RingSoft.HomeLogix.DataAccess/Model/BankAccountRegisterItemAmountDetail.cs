using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class BankAccountRegisterItemAmountDetail
    {
        [Required]
        [Key]
        public int RegisterId { get; set; }

        public virtual BankAccountRegisterItem RegisterItem { get; set; }

        [Required]
        [Key]
        public int DetailId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int StoreId { get; set; }

        public virtual Store Store { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
