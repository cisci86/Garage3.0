#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models;

public class GarageVehicleContext : DbContext
{
    private Random gen = new Random();

    public GarageVehicleContext(DbContextOptions<GarageVehicleContext> options)
        : base(options)
    {
    }

    public DbSet<Garage_2._0.Models.Vehicle> Vehicle { get; set; }
    public DbSet<Garage_2._0.Models.Member> Member { get; set; }
    public DbSet<VehicleType> VehicleType { get; set; }
    public DbSet<Membership> Membership { get; set; }

    //adds seed data to the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //DO NOT REMOVE!!!
        Membership standard = new Membership
        {
            Type = "Standard",
            BenefitBase = 1d,
            BenefitHourly = 1d
        };
        Membership pro = new Membership
        {
            Type = "Pro",
            BenefitBase = 0.9d,
            BenefitHourly = 0.9d
        };
        modelBuilder.Entity<Membership>()
            .HasData(standard, pro);


        //May remove at a later time maybe
        List<string> fakeSSNs = MakeSocialSecurityNumber(3);
        MemberHasMembership moqMembership1 = new MemberHasMembership
        {
            MemberId = fakeSSNs[0],
            Member = null,
            MembershipId = standard.Type,
            Membership = standard,
            ExpiryDate = null
        };
        Member moqMember1 = new Member(fakeSSNs[0], new Name("Carl", "Testsson"), moqMembership1);
        moqMembership1.Member = moqMember1;
        MemberHasMembership moqMembership2 = new MemberHasMembership
        {
            MemberId = fakeSSNs[1],
            Member = null,
            MembershipId = standard.Type,
            Membership = standard,
            ExpiryDate = null
        };
        Member moqMember2 = new Member(fakeSSNs[1], new Name("Bertil", "Test"), moqMembership2);
        moqMembership2.Member = moqMember2;
        MemberHasMembership moqMembership3 = new MemberHasMembership
        {
            MemberId = fakeSSNs[2],
            Member = null,
            MembershipId = standard.Type,
            Membership = standard,
            ExpiryDate = null
        };
        Member moqMember3 = new Member(fakeSSNs[2], new Name("Lina", "Testdotter"), moqMembership3);
        moqMembership3.Member = moqMember3;


        modelBuilder.Entity<Member>().HasData(moqMember1, moqMember2, moqMember3);

        //ToDo fix model builder
        //modelBuilder.Entity<Vehicle>()
        //    .HasData(
        //       new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Car, License = "EGW123", Color="Red", Make="Volvo", Model="Xc60",Wheels=4,Arrival= DateTime.Parse("2022-02-01 12:09:28"), ParkingSpot = 1 },
        //       new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Car, License = "ASL123", Color = "White", Make = "Volvo", Model = "Xc60", Wheels = 4, Arrival = DateTime.Parse("2022 - 02 - 01 13:09:28"), ParkingSpot = 2},
        //       new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Motorcycle, License = "MXP123", Color = "Yellow", Make = "Volvo", Model = "Xc60", Wheels = 2, Arrival = DateTime.Parse("2022 - 02 - 01 14:09:28"), ParkingSpot = 3 },
        //       new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Bus, License = "RRH123", Color = "Blue", Make = "Volvo", Model = "Xc60", Wheels = 8, Arrival = DateTime.Parse("2022 - 02 - 01 15:09:28"), ParkingSpot = 4 }
        //    );

        modelBuilder.Entity<Member>()
                    .OwnsOne(m => m.Name)
                    .Property(n => n.FirstName)
                    .HasColumnName("FirstName");
        modelBuilder.Entity<Member>()
                    .OwnsOne(m => m.Name)
                    .Property(n => n.LastName)
                    .HasColumnName("LastName");

        modelBuilder.Entity<Member>()
                    .HasMany(m => m.Vehicles);

        modelBuilder.Entity<MemberHasMembership>()
                    .HasKey(e => new { e.MemberId, e.MembershipId });
    }

    //adds seed data to the database
    private List<string> MakeSocialSecurityNumber(int howMany)
    {
        List<string> ssns = new List<string>();
        for (int i = 0; i < howMany; i++)
        {
            DateTime start = new DateTime(1900, 01, 01);
            int range = (DateTime.Now - start).Days;
            string birthDate = (start.AddDays(gen.Next(range))).ToString("yymmdd");
/*            //TODO FIX
            var birtDate2 = birthDate.Split('-');
            birthDate = birtDate2[0].Substring(2) + birthDate[1] + birthDate[2];
*/          string birthPlace = (gen.Next(14, 100)).ToString();
            string gender = (gen.Next(1, 10)).ToString();
            string firstNineNumbers = birthDate + birthPlace + gender;
            string controlnumber = generateControlNumber(firstNineNumbers);
            string SSN = firstNineNumbers + controlnumber;
            //Make sure the random SSN is not a duplicate of already existing data
            if (!ssns.Contains(SSN))
                ssns.Add(SSN);
            else
                i--;
        }
        return ssns;
    }
    private string generateControlNumber(string nineDigits)
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
