#nullable disable
using Garage_2._0.Models;
using Microsoft.EntityFrameworkCore;

public class GarageVehicleContext : DbContext
{
    public GarageVehicleContext(DbContextOptions<GarageVehicleContext> options)
        : base(options)
    {
    }

    public DbSet<Garage_2._0.Models.Vehicle> Vehicle { get; set; }
    public DbSet<Garage_2._0.Models.Member> Member { get; set; }


    //adds seed data to the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>()
            .HasData(
               new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Car, License = "EGW123", Color = "Red", Make = "Volvo", Model = "Xc60", Wheels = 4, Arrival = DateTime.Parse("2022-02-01 12:09:28"), ParkingSpot = 1 },
               new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Car, License = "ASL123", Color = "White", Make = "Volvo", Model = "Xc60", Wheels = 4, Arrival = DateTime.Parse("2022 - 02 - 01 13:09:28"), ParkingSpot = 2 },
               new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Motorcycle, License = "MXP123", Color = "Yellow", Make = "Volvo", Model = "Xc60", Wheels = 2, Arrival = DateTime.Parse("2022 - 02 - 01 14:09:28"), ParkingSpot = 3 },
               new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Bus, License = "RRH123", Color = "Blue", Make = "Volvo", Model = "Xc60", Wheels = 8, Arrival = DateTime.Parse("2022 - 02 - 01 15:09:28"), ParkingSpot = 4 }
            );

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
    }
}
