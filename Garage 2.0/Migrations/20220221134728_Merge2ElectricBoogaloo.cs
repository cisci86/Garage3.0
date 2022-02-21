using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class Merge2ElectricBoogaloo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.SocialSecurityNumber);
                });

            migrationBuilder.CreateTable(
                name: "Membership",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BenefitHourly = table.Column<double>(type: "float", nullable: false),
                    BenefitBase = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "ParkinSpot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Available = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkinSpot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleType",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleType", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "MemberHasMembership",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MembershipId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberHasMembership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberHasMembership_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "SocialSecurityNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberHasMembership_Membership_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Membership",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    License = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleTypeName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Wheels = table.Column<int>(type: "int", nullable: false),
                    Arrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParkingSpotId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.License);
                    table.ForeignKey(
                        name: "FK_Vehicle_Member_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Member",
                        principalColumn: "SocialSecurityNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicle_ParkinSpot_ParkingSpotId",
                        column: x => x.ParkingSpotId,
                        principalTable: "ParkinSpot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicle_VehicleType_VehicleTypeName",
                        column: x => x.VehicleTypeName,
                        principalTable: "VehicleType",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Membership",
                columns: new[] { "Type", "BenefitBase", "BenefitHourly" },
                values: new object[,]
                {
                    { "Pro", 0.90000000000000002, 0.90000000000000002 },
                    { "Standard", 1.0, 1.0 }
                });

            migrationBuilder.InsertData(
                table: "ParkinSpot",
                columns: new[] { "Id", "Available" },
                values: new object[,]
                {
                    { 1, true },
                    { 2, true },
                    { 3, true },
                    { 4, true },
                    { 5, true },
                    { 6, true },
                    { 7, true },
                    { 8, true },
                    { 9, true },
                    { 10, true },
                    { 11, true },
                    { 12, true },
                    { 13, true },
                    { 14, true },
                    { 15, true },
                    { 16, true },
                    { 17, true },
                    { 18, true },
                    { 19, true },
                    { 20, true },
                    { 21, true },
                    { 22, true },
                    { 23, true },
                    { 24, true },
                    { 25, true },
                    { 26, true },
                    { 27, true },
                    { 28, true },
                    { 29, true },
                    { 30, true }
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
                name: "IX_MemberHasMembership_MemberId",
                table: "MemberHasMembership",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberHasMembership_MembershipId",
                table: "MemberHasMembership",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_MemberId",
                table: "Vehicle",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_ParkingSpotId",
                table: "Vehicle",
                column: "ParkingSpotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_VehicleTypeName",
                table: "Vehicle",
                column: "VehicleTypeName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberHasMembership");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "Membership");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "ParkinSpot");

            migrationBuilder.DropTable(
                name: "VehicleType");
        }
    }
}
