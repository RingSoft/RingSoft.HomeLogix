using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// ReSharper disable VirtualMemberCallInConstructor

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class Store
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<BankAccountRegisterItemAmountDetail> AmountDetails { get; set; }

        public Store()
        {
            AmountDetails = new HashSet<BankAccountRegisterItemAmountDetail>();
        }
    }
}
