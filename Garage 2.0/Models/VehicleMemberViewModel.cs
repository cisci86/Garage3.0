#nullable disable
namespace Garage_2._0.Models
{
    public class VehicleMemberViewModel
    {
        public string License { get; set; }
        public string Owner { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public string Membership { get; set; }
        public string VehicleType { get; set; }

        public VehicleMemberViewModel()
        {
            License = null;
            Owner = null;
            VehicleType = null;
            Membership = null;
            TimeSpent = default;
        }

        public VehicleMemberViewModel(string license, DateTime currentTime, Member owner, string vehicleType)
        {
            License = license;
            Owner = $"{owner.Name.FirstName} {owner.Name.LastName}";
            Membership = "Test";//owner.MemberHasMembershipId;
            VehicleType = vehicleType;
            TimeSpent = DateTime.Now.Subtract(currentTime);
        }

    }
}
