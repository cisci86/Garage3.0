using Bogus;
using Garage_2._0.Models;

namespace Garage_2._0.Data
{
    public static class SeedData
    {
        private static Faker faker = null;
        private static Random gen = new Random();

        public static async Task Start(GarageVehicleContext _context)
        {
            faker = new Faker("sv");

            var memberShip = GetMembership();
            await _context.AddRangeAsync(memberShip);

            List<string> fakeSSNs = MakeSocialSecurityNumber(3);

            var members = GetMember(fakeSSNs, memberShip.First());
            await _context.AddRangeAsync(members);

            var hasMemberships = GetMemberHasMembership(members, memberShip.First());
            await _context.AddRangeAsync(hasMemberships);

            await _context.SaveChangesAsync();
        }
        private static IEnumerable<Membership> GetMembership()
        {
            var memberShip = new List<Membership>();
            Membership standard = new Membership
            {
                Type = "Standard",
                BenefitBase = 1d,
                BenefitHourly = 1d

            };
            memberShip.Add(standard);
            Membership pro = new Membership
            {
                Type = "Pro",
                BenefitBase = 0.9d,
                BenefitHourly = 0.9d
            };
            memberShip.Add(pro);
            return memberShip;
        }

        private static IEnumerable<Member> GetMember(List<string> ssns, Membership membership)
        {
            var memers = new List<Member>();
            for (int i = 0; i < 3; i++)
            {
                var ssn = ssns[i];
                var fName = faker.Name.FirstName();
                var lName = faker.Name.LastName();

                var member = new Member(ssn, new Name(fName, lName));
                memers.Add(member);
            }
            return memers;
        }

        private static IEnumerable<MemberHasMembership> GetMemberHasMembership(IEnumerable<Member> members, Membership membership)
        {
            var hasMemberships = new List<MemberHasMembership>();
            foreach (var member in members)
            {
                var hasMembership = new MemberHasMembership("Test");// member.MemberHasMembershipId);
                hasMemberships.Add(hasMembership);
                member.Memberships.Add(hasMembership);
            }
            return hasMemberships;
        }




        //ToDo fix model builder
        //modelBuilder.Entity<Vehicle>()
        //    .HasData(
        //       new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Car, License = "EGW123", Color="Red", Make="Volvo", Model="Xc60",Wheels=4,Arrival= DateTime.Parse("2022-02-01 12:09:28"), ParkingSpot = 1 },
        //       new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Car, License = "ASL123", Color = "White", Make = "Volvo", Model = "Xc60", Wheels = 4, Arrival = DateTime.Parse("2022 - 02 - 01 13:09:28"), ParkingSpot = 2},
        //       new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Motorcycle, License = "MXP123", Color = "Yellow", Make = "Volvo", Model = "Xc60", Wheels = 2, Arrival = DateTime.Parse("2022 - 02 - 01 14:09:28"), ParkingSpot = 3 },
        //       new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Bus, License = "RRH123", Color = "Blue", Make = "Volvo", Model = "Xc60", Wheels = 8, Arrival = DateTime.Parse("2022 - 02 - 01 15:09:28"), ParkingSpot = 4 }
        //    );



        //adds seed data to the database
        private static List<string> MakeSocialSecurityNumber(int howMany)
        {
            List<string> ssns = new List<string>();
            for (int i = 0; i < howMany; i++)
            {
                DateTime start = new DateTime(1900, 01, 01);
                int range = (DateTime.Now - start).Days;
                string birthDate = start.AddDays(gen.Next(range)).ToString("yyyyMMdd");

                string birthDateSix = birthDate.Substring(3);
                string birthPlace = gen.Next(14, 100).ToString();
                string gender = gen.Next(1, 10).ToString();
                string firstNineNumbers = birthDateSix + birthPlace + gender;
                string controlnumber = generateControlNumber(firstNineNumbers);

                string SSN = birthDate + "-" + birthPlace + gender + controlnumber;
                //Make sure the random SSN is not a duplicate of already existing data
                if (!ssns.Contains(SSN))
                    ssns.Add(SSN);
                else
                    i--;
            }
            return ssns;
        }
        private static string generateControlNumber(string nineDigits)
        {
            Console.WriteLine(nineDigits);
            string newNumbers = "";
            for (int i = 0; i < nineDigits.Length - 1; i++)
            {
                if (i % 2 == 0)
                    newNumbers += (int.Parse(nineDigits[i].ToString()) * 2).ToString();
                else
                    newNumbers += nineDigits[i].ToString();
            }
            //Sets the last of the 4 digits (control number)
            int controlNumber = 0;
            //goes true all of the new numbers one by one and adds them together
            foreach (char n in newNumbers)
            {
                controlNumber += int.Parse(n.ToString());
            }
            //The formula to calculate the correct control number
            controlNumber = (10 - (controlNumber % 10)) % 10;
            return controlNumber.ToString();
        }
    }
}

