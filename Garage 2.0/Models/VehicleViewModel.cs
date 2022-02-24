using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Garage_2._0.Models
{
    public class VehicleViewModel
    {
        public VehicleType Type { get; set; }
        public string License { get; set; }
        public string Make { get; set; }
        public string TimeSpent { get; set; }
    }
}
