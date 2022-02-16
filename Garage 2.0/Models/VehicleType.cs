using System.ComponentModel.DataAnnotations;
#nullable disable

namespace Garage_2._0.Models
{
    public class VehicleType
    {
        [Required]
        [Key]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
