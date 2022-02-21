using Garage_2._0.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace Garage_2._0.Models
{
    public class Member
    {
        [Required]
        [Key]
        
        public string SocialSecurityNumber { get; set; }
        [Required]
        public Name Name { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        [Required]
        public string MembershipId { get; set; }
        public MemberHasMembership Membership { get; set; }

        private Member()
        {
            SocialSecurityNumber = null;
            Name = null;
            MembershipId = null;
        }

        public Member(string socialSecurity, Name name, string memberShipId)
        {
            SocialSecurityNumber = socialSecurity;
            Name = name;
            MembershipId = memberShipId;
        }
    }
}
