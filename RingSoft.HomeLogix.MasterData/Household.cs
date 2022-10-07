using System.ComponentModel;
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

        [MaxLength(250)]
        public string FilePath { get; set; }

        [MaxLength(250)]
        public string FileName { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        [DefaultValue(0)]
        public byte Platform { get; set; }

        [MaxLength(50)]
        public string Server { get; set; }

        [MaxLength(50)]
        public string Database { get; set; }

        public byte? AuthenticationType { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }
    }
}
