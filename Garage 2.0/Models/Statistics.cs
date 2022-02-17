namespace Garage_2._0.Models
{
    public class Statistics
    {
        public Dictionary<string, int> VehicleTypeCounter { get; set; } = new Dictionary<string, int>();
        public int TotalWheelAmount { get; set; }
        public double TotalCostsGenerated { get; set; }
    }
}
