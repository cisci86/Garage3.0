#nullable disable
using Garage_2._0;
using Garage_2._0.Models;
using Microsoft.EntityFrameworkCore;

public class GarageVehicleContext : DbContext
{
    private Random gen = new Random();
    public GarageVehicleContext(DbContextOptions<GarageVehicleContext> options)
        : base(options)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
        Global.Garagecapacity = config.GetValue<int>("GarageCapacity:Capacity");
    }

    public DbSet<Garage_2._0.Models.Vehicle> Vehicle { get; set; }
    public DbSet<Garage_2._0.Models.Member> Member { get; set; }
    public DbSet<VehicleType> VehicleType { get; set; }
    public DbSet<Membership> Membership { get; set; }
    public DbSet<ParkingSpot> ParkinSpot { get; set; }

    //adds seed data to the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        modelBuilder.Entity<ParkingSpot>()
                    .HasData(
                    GetParkingSpots());
    }
    private static IEnumerable<ParkingSpot> GetParkingSpots()
    {
        var parkingSpots = new List<ParkingSpot>();
        for (int i = 0; i < Global.Garagecapacity; i++)
        {
            var spot = new ParkingSpot
            {
                Id = i + 1,
                Available = true
            };
            parkingSpots.Add(spot);
        }
        return parkingSpots;
    }
    private string MakeSocialSecurityNumber()
    {
        DateTime start = new DateTime(1900, 01, 01);
        int range = (DateTime.Now - start).Days;
        string birthDate = (start.AddDays(gen.Next(range))).ToString();
        string birthPlace = (gen.Next(14, 99)).ToString();
        string gender = (gen.Next(1, 9)).ToString();
        string firstNineNumbers = birthDate + birthPlace + gender;
        string controlnumber = generateControlNumber(firstNineNumbers);
        string SSN = firstNineNumbers + controlnumber;
        if (Member.Find(SSN) == null)
            return firstNineNumbers + controlnumber;
        else
            return MakeSocialSecurityNumber();
    }
    private string generateControlNumber(string nineDigits)
    {
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
