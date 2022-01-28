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
    }
