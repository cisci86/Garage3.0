using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class _ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    License = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Make = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Wheels = table.Column<int>(type: "int", nullable: false),
                    Arrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParkingSpot = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.License);
                });

            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "License", "Arrival", "Color", "Make", "Model", "ParkingSpot", "Type", "Wheels" },
                values: new object[,]
                {
                    { "AS123", new DateTime(2022, 2, 1, 13, 9, 28, 0, DateTimeKind.Unspecified), "White", "Volvo", "Xc60", 0, 0, 4 },
                    { "Eg123", new DateTime(2022, 2, 1, 12, 9, 28, 0, DateTimeKind.Unspecified), "Red", "Volvo", "Xc60", 0, 0, 4 },
                    { "MX123", new DateTime(2022, 2, 1, 14, 9, 28, 0, DateTimeKind.Unspecified), "Yellow", "Volvo", "Xc60", 0, 2, 2 },
                    { "RR123", new DateTime(2022, 2, 1, 15, 9, 28, 0, DateTimeKind.Unspecified), "Blue", "Volvo", "Xc60", 0, 1, 8 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
