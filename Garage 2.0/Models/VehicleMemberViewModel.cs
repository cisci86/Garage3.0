#nullable disable
namespace Garage_2._0.Models
{
    public class VehicleMemberViewModel
    {
        public string License { get; set; }
        public string Owner { get; set; }
        public string TimeSpent { get; set; }
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
            //ToDo check if it is correct
            Membership = owner.Memberships.Last().MembershipId;
            VehicleType = vehicleType;
            TimeSpan _t = DateTime.Now.Subtract(currentTime);
            TimeSpent = $"{_t.Days} days, {_t.Hours} hours, {_t.Minutes} minutes";
        }

    }
}
