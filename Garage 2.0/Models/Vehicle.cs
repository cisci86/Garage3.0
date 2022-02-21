using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace Garage_2._0.Models
{
    public class Vehicle
    {
        [Required]
        public string VehicleTypeName { get; set; }
        public VehicleType Type { get; set; }
        [Required]
        [Key]
        [Remote(action: "VerifyLicense", controller: "Vehicles")]
        public string License { get; set; }
        [Required]
        public string Color { get; set; }
        [StringLength(20, ErrorMessage = "Length of Make can't be more than 20")]
        [Required]
        public string Make { get; set; }
        [StringLength(20, ErrorMessage = "Length of Model can't be more than 20")]
        [Required]
        public string Model { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value of 0 or bigger")]
        public int Wheels { get; set; }
        [ReadOnly(true)]
        public DateTime Arrival { get; set; }
        public int ParkingSpotId { get; set; }
        public ParkingSpot ParkingSpot { get; set; }
        [Required]
        [Remote(action: "VerifyMember", controller: "Vehicles")]
        public string MemberId { get; set; }
        public Member Owner { get; set; }
    }

    
}
