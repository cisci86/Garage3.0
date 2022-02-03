using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class ParkingSpots
    {
        [Key]
        public int SpotId { get; set; }
        public string License { get; set; }
        public bool Available { get; set; }
    }
}