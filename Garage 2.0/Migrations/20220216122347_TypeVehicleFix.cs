using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class TypeVehicleFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Membership_MembershipType",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Member_MemberId",
                table: "Vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehicleType_TypeName",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "MembershipType",
                table: "Member",
                newName: "MembershipId");

            migrationBuilder.RenameIndex(
                name: "IX_Member_MembershipType",
                table: "Member",
                newName: "IX_Member_MembershipId");

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "VehicleType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Membership_MembershipId",
                table: "Member",
                column: "MembershipId",
                principalTable: "Membership",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Member_MemberId",
                table: "Vehicle",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "SocialSecurityNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehicleType_TypeName",
                table: "Vehicle",
                column: "TypeName",
                principalTable: "VehicleType",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Membership_MembershipId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Member_MemberId",
                table: "Vehicle");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehicleType_TypeName",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "VehicleType");

            migrationBuilder.RenameColumn(
                name: "MembershipId",
                table: "Member",
                newName: "MembershipType");

            migrationBuilder.RenameIndex(
                name: "IX_Member_MembershipId",
                table: "Member",
                newName: "IX_Member_MembershipType");

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Membership_MembershipType",
                table: "Member",
                column: "MembershipType",
                principalTable: "Membership",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Member_MemberId",
                table: "Vehicle",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "SocialSecurityNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehicleType_TypeName",
                table: "Vehicle",
                column: "TypeName",
                principalTable: "VehicleType",
                principalColumn: "Name");
        }
    }
}
