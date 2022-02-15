using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class ChangedseedAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "AS123");

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "Eg123");

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "MX123");

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "RR123");

            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "License", "Arrival", "Color", "Make", "Model", "ParkingSpot", "Type", "Wheels" },
                values: new object[,]
                {
                    { "ASL123", new DateTime(2022, 2, 1, 13, 9, 28, 0, DateTimeKind.Unspecified), "White", "Volvo", "Xc60", 2, 0, 4 },
                    { "EGW123", new DateTime(2022, 2, 1, 12, 9, 28, 0, DateTimeKind.Unspecified), "Red", "Volvo", "Xc60", 1, 0, 4 },
                    { "MXP123", new DateTime(2022, 2, 1, 14, 9, 28, 0, DateTimeKind.Unspecified), "Yellow", "Volvo", "Xc60", 3, 2, 2 },
                    { "RRH123", new DateTime(2022, 2, 1, 15, 9, 28, 0, DateTimeKind.Unspecified), "Blue", "Volvo", "Xc60", 4, 1, 8 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "ASL123");

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "EGW123");

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "MXP123");

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "RRH123");

            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "License", "Arrival", "Color", "Make", "Model", "ParkingSpot", "Type", "Wheels" },
                values: new object[,]
                {
                    { "AS123", new DateTime(2022, 2, 1, 13, 9, 28, 0, DateTimeKind.Unspecified), "White", "Volvo", "Xc60", 2, 0, 4 },
                    { "Eg123", new DateTime(2022, 2, 1, 12, 9, 28, 0, DateTimeKind.Unspecified), "Red", "Volvo", "Xc60", 1, 0, 4 },
                    { "MX123", new DateTime(2022, 2, 1, 14, 9, 28, 0, DateTimeKind.Unspecified), "Yellow", "Volvo", "Xc60", 3, 2, 2 },
                    { "RR123", new DateTime(2022, 2, 1, 15, 9, 28, 0, DateTimeKind.Unspecified), "Blue", "Volvo", "Xc60", 4, 1, 8 }
                });
        }
    }
}
