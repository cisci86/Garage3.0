namespace Garage_2._0.Models
{
    public class Name
    {
        public string FirstName { get; set; }
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