using Bogus;
using Garage_2._0.Models;

namespace Garage_2._0.Data
{
    public static class SeedData
    {
        private static Faker faker = null;
        private static Random gen = new Random();
        private static GarageVehicleContext? _context = null;
        private static IConfiguration _config = null!;

        public static async Task Start(GarageVehicleContext context, IConfiguration config)
        {

            faker = new Faker("sv");
            _context = context;
            _config = config;
            var memberShip = await GetMembership();
            IEnumerable<Member> members = null;
            IEnumerable<MemberHasMembership> hasMemberships = null;
            IEnumerable<Vehicle> vehicles = null;

            List<string> fakeSSNs = MakeSocialSecurityNumber(15);
            if (!context.Member.Any())
            {
                members = GetMember(fakeSSNs, memberShip.Find(m => m.Type == "Standard")!);
                await context.AddRangeAsync(members);
                hasMemberships = GetMemberHasMembership(members, memberShip.First());
                await context.AddRangeAsync(hasMemberships);
            }
            else 
                members = context.Member.ToList();
            
           
            var vehicleTypes = await GetVehicleTypes();

            List<string> licensPlates = GenerateLicensPlate(10);

            if (!_context.Vehicle.Any())
            {
                vehicles = GetVehicles(licensPlates, members.ToList(), vehicleTypes);
                await context.AddRangeAsync(vehicles);
            }

            await context.SaveChangesAsync();
        }
        private async static Task<List<Membership>> GetMembership()
        {
            if (!_context.Membership.Any())
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
                await _context.AddRangeAsync(memberShip);
                return memberShip;
            }
            return _context.Membership.ToList();
        }

        private static IEnumerable<Member> GetMember(List<string> ssns, Membership membership)
        {
            var memers = new List<Member>();
            for (int i = 0; i < 15; i++)
            {
                var ssn = ssns[i];
                var fName = faker.Name.FirstName();
                var lName = faker.Name.LastName();

                var member = new Member(ssn, new Name(fName, lName), membership.Type);
                memers.Add(member);
            }
            return memers;
        }

        private static IEnumerable<MemberHasMembership> GetMemberHasMembership(IEnumerable<Member> members, Membership membership)
        {
            var hasMemberships = new List<MemberHasMembership>();
            foreach (var member in members)
            {
                var hasMembership = new MemberHasMembership(member.SocialSecurityNumber, member.MembershipId);
                hasMemberships.Add(hasMembership);
                member.Membership = hasMembership;
            }
            return hasMemberships;
        }
        private static async Task<List<VehicleType>> GetVehicleTypes()
        {
            if (!_context.VehicleType.Any())
            {
                var vehicleTypes = new List<VehicleType>();
                var car = new VehicleType
                {
                    Name = "Car",
                    Description = "The regular everyday vehicle most commonly used by people to travel both short and long distances",
                    Size = 1
                };
                vehicleTypes.Add(car);
                var bus = new VehicleType
                {
                    Name = "Bus",
                    Description = "Bigger type of transportation that takes over 6 people",
                    Size = 1
                };
                vehicleTypes.Add(bus);
                var motorcycle = new VehicleType
                {
                    Name = "Motorcycle",
                    Description = "A two wheeled vehicle that makes the owner respected in certain communities",
                    Size = 1
                };
                vehicleTypes.Add(motorcycle);
                var zeppelin = new VehicleType
                {
                    Name = "Zeppelin",
                    Description = "An airship in very limited edition",
                    Size = 1
                };
                vehicleTypes.Add(zeppelin);
                var bananamobile = new VehicleType
                {
                    Name = "Bananamobile",
                    Description = "Dimitris main way of transport, unmatched by any other vehicle. Aquatic, airborne and an atv all at once!",
                    Size = 1
                };
                await _context.AddRangeAsync(vehicleTypes);
                return vehicleTypes;
            }
            return _context.VehicleType.ToList();

        }

        private static IEnumerable<Vehicle> GetVehicles(List<string> license, List<Member> members, List<VehicleType> vehicleTypes)
        {
            Faker faker2 = new Faker("en");
            var vehicles = new List<Vehicle>();
            for (int i = 0; i < 10; i++)
            {
                var make = faker.Vehicle.Manufacturer();
                var model = faker.Vehicle.Model();
                var licensePlate = license[i];
                var arrival = faker.Date.Past(10, DateTime.Now);
                var rand = gen.Next(0, members.Count());
                var member = members[rand];
                var rand2 = gen.Next(0, vehicleTypes.Count());
                var vehicleType = vehicleTypes[rand2];
                var color = faker2.Commerce.Color();
                var wheels = gen.Next(0, _config.GetValue<int>("GarageCapacity:Capacity"));
                var vehicle = new Vehicle
                {
                    VehicleTypeName = vehicleType.Name,
                    Type = vehicleType,
                    License = licensePlate,
                    Make = make,
                    Model = model,
                    Arrival = arrival,
                    MemberId = member.SocialSecurityNumber,
                    ParkingSpot = i + 1,
                    Color = color,
                    Wheels = wheels,
                    Owner = member
                };
                vehicles.Add(vehicle);
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

