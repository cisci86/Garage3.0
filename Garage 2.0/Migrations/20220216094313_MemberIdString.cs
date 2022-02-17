using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class MemberIdString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Member_OwnerSocialSecurityNumber",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_OwnerSocialSecurityNumber",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "OwnerSocialSecurityNumber",
                table: "Vehicle");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_MemberId",
                table: "Vehicle",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Member_MemberId",
                table: "Vehicle",
                column: "MemberId",
                principalTable: "Member",
                principalColumn: "SocialSecurityNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Member_MemberId",
                table: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_MemberId",
                table: "Vehicle");

            migrationBuilder.AlterColumn<int>(
                name: "MemberId",
                table: "Vehicle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerSocialSecurityNumber",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_OwnerSocialSecurityNumber",
                table: "Vehicle",
                column: "OwnerSocialSecurityNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Member_OwnerSocialSecurityNumber",
                table: "Vehicle",
                column: "OwnerSocialSecurityNumber",
                principalTable: "Member",
                principalColumn: "SocialSecurityNumber");
        }
    }
}
