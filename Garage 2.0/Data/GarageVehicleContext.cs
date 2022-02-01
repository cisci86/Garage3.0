#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models;

    public class GarageVehicleContext : DbContext
    {
        public GarageVehicleContext (DbContextOptions<GarageVehicleContext> options)
            : base(options)
        {
        }

        public DbSet<Garage_2._0.Models.Vehicle> Vehicle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>()
            .HasData(
               new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Car, License = "Eg123", Color="Red", Make="Volvo", Model="Xc60",Wheels=4,Arrival= DateTime.Parse("2022-02-01 12:09:28") },
               new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Car, License = "AS123", Color = "White", Make = "Volvo", Model = "Xc60", Wheels = 4, Arrival = DateTime.Parse("2022 - 02 - 01 13:09:28") },
               new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Motorcycle, License = "MX123", Color = "Yellow", Make = "Volvo", Model = "Xc60", Wheels = 2, Arrival = DateTime.Parse("2022 - 02 - 01 14:09:28") },
               new Vehicle { Type = Garage_2._0.Interfaces.VehicleTypes.Bus, License = "RR123", Color = "Blue", Make = "Volvo", Model = "Xc60", Wheels = 8, Arrival = DateTime.Parse("2022 - 02 - 01 15:09:28") }
            );
    }

}
