using Bogus;
using Garage_2._0.Extensions;
using Garage_2._0.Models;

namespace Garage_2._0.Data
{
    public static class SeedData
    {
        private static Faker faker = null;
        private static Random gen = new Random();
        private static GarageVehicleContext? _context = null;

        public static async Task Start(GarageVehicleContext context)
        {

            faker = new Faker("sv");
            _context = context;
            IEnumerable<Member> members = null;
            IEnumerable<MemberHasMembership> hasMemberships = null;
            IEnumerable<Vehicle> vehicles = null;
            List<string> fakeSSNs = null;

            if (!context.Member.Any())
            {
                var memberShip = _context.Membership.ToList();
                fakeSSNs = MakeSocialSecurityNumber(15);
                members = GetMember(fakeSSNs, memberShip.Find(m => m.Type == "Standard")!);
                await context.AddRangeAsync(members);
                hasMemberships = GetMemberHasMembership(members, memberShip.First());
                await context.AddRangeAsync(hasMemberships);
            }
            else
                members = context.Member.ToList();




            if (!_context.Vehicle.Any())
            {
                var vehicleTypes = _context.VehicleType.ToList();
                List<string> licensPlates = GenerateLicensPlate(10);
                var parkingspots = _context.ParkinSpot.ToList();

                vehicles = GetVehicles(licensPlates, members.ToList(), vehicleTypes, parkingspots);
                await context.AddRangeAsync(vehicles);
            }

            await context.SaveChangesAsync();
        }

        private static IEnumerable<Member> GetMember(List<string> ssns, Membership membership)
        {
            var memers = new List<Member>();
            for (int i = 0; i < 15; i++)
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
            DateTime earliest = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - earliest).Days;
            foreach (var member in members)
            {
                earliest = new DateTime(2020, 1, 1);

                var hasMembership = new MemberHasMembership(member.SocialSecurityNumber);// member.MemberHasMembershipId);
                hasMembership.Member = member;
                hasMembership.Membership = membership;
                hasMembership.StartDate = earliest.AddDays(gen.Next(range));
                hasMemberships.Add(hasMembership);
                member.Memberships.Add(hasMembership);
            }
            return hasMemberships;
        }
        

        private static IEnumerable<Vehicle> GetVehicles(List<string> license, List<Member> members, List<VehicleType> vehicleTypes, List<ParkingSpot> parkingspots)
        {
            Faker faker2 = new Faker("en");
            var vehicles = new List<Vehicle>();
            for (int i = 0; i < 10; i++)
            {
                var make = faker.Vehicle.Manufacturer();
                var model = faker.Vehicle.Model();
                var licensePlate = license[i];
                var arrival = faker.Date.Between(DateTime.Now.AddDays(-30) , DateTime.Now);
                var rand = gen.Next(0, members.Count());
                var member = members[rand];
                var rand2 = gen.Next(0, vehicleTypes.Count());
                var vehicleType = vehicleTypes[rand2];
                var color = faker2.Commerce.Color();
                ParkingSpot parkingspot;
                do
                {
                    var randomSpot = gen.Next(parkingspots.Count);
                    parkingspot = parkingspots[randomSpot-1];
                } while (vehicles.Find(v => v.ParkingSpotId == parkingspot.Id) != null);

                var wheels = gen.Next(0, 10);
                var vehicle = new Vehicle
                {
                    VehicleTypeName = vehicleType.Name,
                    Type = vehicleType,
                    License = licensePlate,
                    Make = make,
                    Model = model,
                    Arrival = arrival,
                    MemberId = member.SocialSecurityNumber,
                    ParkingSpot = parkingspot,
                    Color = color,
                    Wheels = wheels,
                    Owner = member
                };
                vehicles.Add(vehicle);

                parkingspot.Available = false;       
                parkingspot.Vehicle = vehicle;
            }
            return vehicles;
        }

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
                if (!ssns.Contains(SSN) && _context.Member.Find(SSN) == null)
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
        private static List<string> GenerateLicensPlate(int howmany)
        {
            List<string> licens = new List<string>();
            for (int i = 0; i < howmany; i++)
            {
                string licensplate = "";
                for (int j = 0; j < 3; j++)
                {
                    licensplate += (char)gen.Next('A', 'Z');
                }
                for (int j = 0; j < 3; j++)
                {
                    licensplate += gen.Next(1, 10);
                }
                if (!licens.Contains(licensplate) && _context.Vehicle.Find(licensplate) == null)
                {
                    licens.Add(licensplate);
                }
                else i--;
            }
            return licens;
        }
    }
}

