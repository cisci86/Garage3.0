using Garage_2._0.Validation;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class Name
    {
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        private Name()
        {
            FirstName = null!;
            LastName = null!;
        }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}