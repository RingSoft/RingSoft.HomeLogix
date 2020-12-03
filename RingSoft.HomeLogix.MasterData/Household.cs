using System.ComponentModel.DataAnnotations;

namespace RingSoft.HomeLogix.MasterData
{
    public class Household
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string FilePath { get; set; }

        [Required]
        [MaxLength(250)]
        public string FileName { get; set; }

        [Required]
        public bool IsDefault { get; set; }
    }
}
