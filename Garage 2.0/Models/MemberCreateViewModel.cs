using Garage_2._0.Validation;

namespace Garage_2._0.Models
{
    public class MemberCreateViewModel
    {
        public string SocialSecurityNumber { get; set; }
        public string NameFirstName { get; set; }
        [NameAttribute]
        public string NameLastName { get; set; }
    }
}
