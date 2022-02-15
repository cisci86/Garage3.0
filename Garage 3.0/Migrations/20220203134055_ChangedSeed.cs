using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class ChangedSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "AS123",
                column: "ParkingSpot",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "Eg123",
                column: "ParkingSpot",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "MX123",
                column: "ParkingSpot",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "RR123",
                column: "ParkingSpot",
                value: 4);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "AS123",
                column: "ParkingSpot",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "Eg123",
                column: "ParkingSpot",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "MX123",
                column: "ParkingSpot",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "RR123",
                column: "ParkingSpot",
                value: 0);
        }
    }
}
