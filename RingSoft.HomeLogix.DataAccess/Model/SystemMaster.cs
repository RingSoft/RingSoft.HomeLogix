using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.DataAccess.Model
{
    public class SystemMaster
    {
        [Required]
        [Key]
        [MaxLength(50)]
        public string HouseholdName { get; set; }
    }
}
