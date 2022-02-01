using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "Abc123");

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "Bcd123");

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "Cde123");

            migrationBuilder.DeleteData(
                table: "Vehicle",
                keyColumn: "License",
                keyValue: "Def123");

            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "License", "Arrival", "Color", "Make", "Model", "Type", "Wheels" },
                values: new object[,]
                {
                    { "AS123", new DateTime(2022, 2, 1, 13, 9, 28, 0, DateTimeKind.Unspecified), "White", "Volvo", "Xc60", 0, 4 },
                    { "Eg123", new DateTime(2022, 2, 1, 12, 9, 28, 0, DateTimeKind.Unspecified), "Red", "Volvo", "Xc60", 0, 4 },
                    { "MX123", new DateTime(2022, 2, 1, 14, 9, 28, 0, DateTimeKind.Unspecified), "Yellow", "Volvo", "Xc60", 2, 2 },
                    { "RR123", new DateTime(2022, 2, 1, 15, 9, 28, 0, DateTimeKind.Unspecified), "Blue", "Volvo", "Xc60", 1, 8 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                columns: new[] { "License", "Arrival", "Color", "Make", "Model", "Type", "Wheels" },
                values: new object[,]
                {
                    { "Abc123", new DateTime(2022, 2, 1, 12, 9, 28, 0, DateTimeKind.Unspecified), "Red", "Volvo", "Xc60", 0, 4 },
                    { "Bcd123", new DateTime(2022, 2, 1, 13, 9, 28, 0, DateTimeKind.Unspecified), "White", "Volvo", "Xc60", 0, 4 },
                    { "Cde123", new DateTime(2022, 2, 1, 14, 9, 28, 0, DateTimeKind.Unspecified), "Yellow", "Volvo", "Xc60", 2, 2 },
                    { "Def123", new DateTime(2022, 2, 1, 15, 9, 28, 0, DateTimeKind.Unspecified), "Blue", "Volvo", "Xc60", 1, 8 }
                });
        }
    }
}
