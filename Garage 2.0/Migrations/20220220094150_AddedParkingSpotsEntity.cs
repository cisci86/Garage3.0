using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class AddedParkingSpotsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkinSpot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    VehicleLicense = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkinSpot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkinSpot_Vehicle_VehicleLicense",
                        column: x => x.VehicleLicense,
                        principalTable: "Vehicle",
                        principalColumn: "License");
                });

            migrationBuilder.InsertData(
                table: "ParkinSpot",
                columns: new[] { "Id", "Available", "VehicleLicense" },
                values: new object[,]
                {
                    { 1, true, null },
                    { 2, true, null },
                    { 3, true, null },
                    { 4, true, null },
                    { 5, true, null },
                    { 6, true, null },
                    { 7, true, null },
                    { 8, true, null },
                    { 9, true, null },
                    { 10, true, null },
                    { 11, true, null },
                    { 12, true, null },
                    { 13, true, null },
                    { 14, true, null },
                    { 15, true, null },
                    { 16, true, null },
                    { 17, true, null },
                    { 18, true, null },
                    { 19, true, null },
                    { 20, true, null },
                    { 21, true, null },
                    { 22, true, null },
                    { 23, true, null },
                    { 24, true, null },
                    { 25, true, null },
                    { 26, true, null },
                    { 27, true, null },
                    { 28, true, null },
                    { 29, true, null },
                    { 30, true, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkinSpot_VehicleLicense",
                table: "ParkinSpot",
                column: "VehicleLicense");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkinSpot");
        }
    }
}
