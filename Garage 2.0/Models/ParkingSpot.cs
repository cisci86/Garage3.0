using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Garage_2._0.Models
{
    public class ParkingSpot
    {
        [Key]
        public int Id { get; set; }
        public bool Available { get; set; }
       // public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
