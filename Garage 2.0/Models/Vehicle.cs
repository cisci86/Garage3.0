using Garage_2._0.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace Garage_2._0.Models
{
    public class Vehicle : IVehicle
    {
        public VehicleTypes Type { get; set; }
        [Key]
        [Remote(action: "VerifyLicense", controller: "Vehicles")]
        public string License { get; set; }

        public string Color { get; set; }
        [StringLength(20, ErrorMessage = "Length of Make can't be more than 20")]
        public string Make { get; set; }
        [StringLength(20, ErrorMessage = "Length of Model can't be more than 20")]
        public string Model { get; set; }
        [Range(0, int.MaxValue)]
        public int Wheels { get; set; }
        [ReadOnly(true)] //Not sure if this is needed
        public DateTime Arrival { get; set; }
        //The error message saids that we need to have an empty constructor so I removed the one we hade.

        // I tried this but then all of the times in all the items changed to the time that I added the last thing. I moved It to the Control in the Create method.

        //public Vehicle() 
        //{
        //    Arrival = DateTime.Now;
        //}
    }


}
