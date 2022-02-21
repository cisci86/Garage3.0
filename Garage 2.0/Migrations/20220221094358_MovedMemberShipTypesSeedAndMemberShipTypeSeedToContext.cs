using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class MovedMemberShipTypesSeedAndMemberShipTypeSeedToContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkinSpot_Vehicle_VehicleLicense",
                table: "ParkinSpot");

            migrationBuilder.DropIndex(
                name: "IX_ParkinSpot_VehicleLicense",
                table: "ParkinSpot");

            migrationBuilder.DropColumn(
                name: "VehicleLicense",
                table: "ParkinSpot");

            migrationBuilder.RenameColumn(
                name: "ParkingSpot",
                table: "Vehicle",
                newName: "ParkingSpotId");

            migrationBuilder.InsertData(
                table: "Membership",
                columns: new[] { "Type", "BenefitBase", "BenefitHourly" },
                values: new object[,]
                {
                    { "Pro", 0.90000000000000002, 0.90000000000000002 },
                    { "Standard", 1.0, 1.0 }
                });

            migrationBuilder.InsertData(
                table: "VehicleType",
                columns: new[] { "Name", "Description", "Size" },
                values: new object[,]
                {
                    { "Bananamobile", "Dimitris main way of transport, unmatched by any other vehicle. Aquatic, airborne and an atv all at once!", 1 },
                    { "Bus", "Bigger type of transportation that takes over 6 people", 1 },
                    { "Car", "The regular everyday vehicle most commonly used by people to travel both short and long distances", 1 },
                    { "Motorcycle", "A two wheeled vehicle that makes the owner respected in certain communities", 1 },
                    { "Zeppelin", "An airship in very limited edition", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_ParkingSpotId",
                table: "Vehicle",
                column: "ParkingSpotId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_ParkinSpot_ParkingSpotId",
                table: "Vehicle",
                column: "ParkingSpotId",
                principalTable: "ParkinSpot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_ParkinSpot_ParkingSpotId",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_ParkingSpotId",
                table: "Vehicle");

            migrationBuilder.DeleteData(
                table: "Membership",
                keyColumn: "Type",
                keyValue: "Pro");

            migrationBuilder.DeleteData(
                table: "Membership",
                keyColumn: "Type",
                keyValue: "Standard");

            migrationBuilder.DeleteData(
                table: "VehicleType",
                keyColumn: "Name",
                keyValue: "Bananamobile");

            migrationBuilder.DeleteData(
                table: "VehicleType",
                keyColumn: "Name",
                keyValue: "Bus");

            migrationBuilder.DeleteData(
                table: "VehicleType",
                keyColumn: "Name",
                keyValue: "Car");

            migrationBuilder.DeleteData(
                table: "VehicleType",
                keyColumn: "Name",
                keyValue: "Motorcycle");

            migrationBuilder.DeleteData(
                table: "VehicleType",
                keyColumn: "Name",
                keyValue: "Zeppelin");

            migrationBuilder.RenameColumn(
                name: "ParkingSpotId",
                table: "Vehicle",
                newName: "ParkingSpot");

            migrationBuilder.AddColumn<string>(
                name: "VehicleLicense",
                table: "ParkinSpot",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkinSpot_VehicleLicense",
                table: "ParkinSpot",
                column: "VehicleLicense");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkinSpot_Vehicle_VehicleLicense",
                table: "ParkinSpot",
                column: "VehicleLicense",
                principalTable: "Vehicle",
                principalColumn: "License");
        }
    }
}
