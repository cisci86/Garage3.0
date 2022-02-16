using System.ComponentModel.DataAnnotations;
#nullable disable

namespace Garage_2._0.Models
{
    public class Membership
    {
        [Required]
        [Key]
        public string Type { get; set; }
        [Required]
        public double BenefitHourly { get; set; }
        [Required]
        public double BenefitBase { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}
