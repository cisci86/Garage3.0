using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Garage_2._0.Models
{
    public class Receipt
    {
        
        public string MemberName { get; set; }
        public string MemberId { get; set; }
        public string VehicleTypeName { get; set; }
        [Key]
        public string License { get; set; }
        public ParkingSpot ParkingSpot { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime CheckOut { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh} Hours {0:mm} Minutes")]
        public TimeSpan ParkingDuration { get; set; }
        
        public string Price { get; set; }

    }
}
