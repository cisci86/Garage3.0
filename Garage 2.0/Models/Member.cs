using Garage_2._0.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace Garage_2._0.Models
{
    public class Member
    {
        [Key]
        public string SocialSecurityNumber { get; set; }
        [Required]
        public Name Name { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        [Required]
        public ICollection<MemberHasMembership> Memberships { get; set; } = new List<MemberHasMembership>();

        private Member()
        {
            SocialSecurityNumber = null;
            Name = null;
        }

        public Member(string socialSecurity, Name name)
        {
            SocialSecurityNumber = socialSecurity;
            Name = name;
        }
    }
}
