using Garage_2._0.Validation;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Garage_2._0.Models
{
    public class Member
    {
        [Key]
        public string SocialSecurityNumber { get; set; }
        
        public Name Name { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    
        private Member()
        {
            SocialSecurityNumber = null!;
            Name = null!;
        }

        public Member(string socialSecurity, Name name)
        {
            SocialSecurityNumber = socialSecurity;
            Name = name;
        }
    }
}
