using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class MemberEditviewModel
    {
        public string SocialSecurityNumber { get; set; }
        [Display(Name="First name")]
        public string NameFirstName { get; set; }
        [Display(Name="Last name")]
        public string NameLastName { get; set; }
    }
}
