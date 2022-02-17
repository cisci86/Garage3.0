#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models;

public class GarageVehicleContext : DbContext
{

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
}