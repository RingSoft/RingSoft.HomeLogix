using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class SystemMaster
    {
        [Required]
        [Key]
        [MaxLength(50)]
        public string HouseholdName { get; set; }

        [MaxLength(50)]
        public string PhoneLogin { get; set; }

        [MaxLength(250)]
        public string PhonePassword { get; set; }

        [MaxLength(50)]
        [Required]
        public string AppGuid { get; set; }

    }
}
