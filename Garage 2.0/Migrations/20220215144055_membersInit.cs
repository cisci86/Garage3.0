using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class membersInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ownerSocialSecurityNumber",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_ownerSocialSecurityNumber",
                table: "Vehicle",
                column: "ownerSocialSecurityNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Member_ownerSocialSecurityNumber",
                table: "Vehicle",
                column: "ownerSocialSecurityNumber",
                principalTable: "Member",
                principalColumn: "SocialSecurityNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Member_ownerSocialSecurityNumber",
                table: "Vehicle");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Vehicle_ownerSocialSecurityNumber",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "ownerSocialSecurityNumber",
                table: "Vehicle");
        }
    }
}
