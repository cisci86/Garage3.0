using Garage_2._0.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Garage_2._0.Models
{
    public class MemberCreateViewModel
    {
        [SocialSecurityNumber]
        [Remote(action: "CheckForDuplicateMembers", controller: "Members")]
        public string SocialSecurityNumber { get; set; }
        public string NameFirstName { get; set; }
        [Name]
        public string NameLastName { get; set; }
    }
}
