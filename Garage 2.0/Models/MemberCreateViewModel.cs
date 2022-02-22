using Garage_2._0.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class MemberCreateViewModel
    {
        [SocialSecurityNumber]
        [Remote(action: "CheckForDuplicateMembers", controller: "Members")]
        public string SocialSecurityNumber { get; set; }
        [Display(Name="First name")]
        public string NameFirstName { get; set; }
        [Display(Name="Last name")]
        [Name]
        public string NameLastName { get; set; }
    }
}
