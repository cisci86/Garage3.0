using Garage_2._0.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class Vehicle : IVehicle
    {
        public string Type { get; private set; }
        [Key]
        public string License { get; private set; }

        public string Color { get; private set; }
        [StringLength(20, ErrorMessage = "Length of Make can't be more than 20")]
        public string Make { get; private set; }
        [StringLength(20, ErrorMessage = "Length of Make can't be more than 20")]
        public string Model { get; private set; }
        [Range(0, int.MaxValue)]
        public int Wheels { get; private set; }
        
        public DateTime Arrival { get; }

        public Vehicle(string type, string license, string color, string make, string model, int wheels)
        {
            Type = type;
            License = license;
            Color = color;
            Make = make;
            Model = model;
            Wheels = wheels;
            Arrival = DateTime.Now;
        }
    }
}
