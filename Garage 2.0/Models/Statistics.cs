using Garage_2._0.Interfaces;

namespace Garage_2._0.Models
{
    public class Statistics
    {
        public Dictionary<VehicleTypes, int> VehicleTypeCounter { get; set; } = new Dictionary<VehicleTypes, int>();
        public int TotalWheelAmount { get; set; }
        public double TotalCostsGenerated { get; set; }
    }
}
