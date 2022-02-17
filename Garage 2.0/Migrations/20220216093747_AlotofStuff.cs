using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class AlotofStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Member_ownerSocialSecurityNumber",
                table: "Vehicle");

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

            migrationBuilder.RenameColumn(
                name: "ownerSocialSecurityNumber",
                table: "Vehicle",
                newName: "OwnerSocialSecurityNumber");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Vehicle",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_ownerSocialSecurityNumber",
                table: "Vehicle",
                newName: "IX_Vehicle_OwnerSocialSecurityNumber");

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MembershipType",
                table: "Member",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Membership",
                columns: table => new
                {
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BenefitHourly = table.Column<double>(type: "float", nullable: false),
                    BenefitBase = table.Column<double>(type: "float", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "VehicleType",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleType", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_TypeName",
                table: "Vehicle",
                column: "TypeName");

            migrationBuilder.CreateIndex(
                name: "IX_Member_MembershipType",
                table: "Member",
                column: "MembershipType");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Membership_MembershipType",
                table: "Member",
                column: "MembershipType",
                principalTable: "Membership",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Member_OwnerSocialSecurityNumber",
                table: "Vehicle",
                column: "OwnerSocialSecurityNumber",
                principalTable: "Member",
                principalColumn: "SocialSecurityNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehicleType_TypeName",
                table: "Vehicle",
                column: "TypeName",
                principalTable: "VehicleType",
                principalColumn: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Membership_MembershipType",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Member_OwnerSocialSecurityNumber",
                table: "Vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehicleType_TypeName",
                table: "Vehicle");

            migrationBuilder.DropTable(
                name: "Membership");

            migrationBuilder.DropTable(
                name: "VehicleType");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_TypeName",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Member_MembershipType",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "MembershipType",
                table: "Member");

            migrationBuilder.RenameColumn(
                name: "OwnerSocialSecurityNumber",
                table: "Vehicle",
                newName: "ownerSocialSecurityNumber");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "Vehicle",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_OwnerSocialSecurityNumber",
                table: "Vehicle",
                newName: "IX_Vehicle_ownerSocialSecurityNumber");

            migrationBuilder.InsertData(
                table: "Vehicle",
                columns: new[] { "License", "Arrival", "Color", "Make", "Model", "ParkingSpot", "Type", "Wheels", "ownerSocialSecurityNumber" },
                values: new object[,]
                {
                    { "ASL123", new DateTime(2022, 2, 1, 13, 9, 28, 0, DateTimeKind.Unspecified), "White", "Volvo", "Xc60", 2, 0, 4, null },
                    { "EGW123", new DateTime(2022, 2, 1, 12, 9, 28, 0, DateTimeKind.Unspecified), "Red", "Volvo", "Xc60", 1, 0, 4, null },
                    { "MXP123", new DateTime(2022, 2, 1, 14, 9, 28, 0, DateTimeKind.Unspecified), "Yellow", "Volvo", "Xc60", 3, 2, 2, null },
                    { "RRH123", new DateTime(2022, 2, 1, 15, 9, 28, 0, DateTimeKind.Unspecified), "Blue", "Volvo", "Xc60", 4, 1, 8, null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Member_ownerSocialSecurityNumber",
                table: "Vehicle",
                column: "ownerSocialSecurityNumber",
                principalTable: "Member",
                principalColumn: "SocialSecurityNumber");
        }
    }
}
