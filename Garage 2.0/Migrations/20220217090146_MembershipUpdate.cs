using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class MembershipUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Membership_MembershipId",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_MembershipId",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Membership");

            migrationBuilder.AlterColumn<string>(
                name: "MembershipId",
                table: "Member",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "MemberHasMembership",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MembershipId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberHasMembership", x => new { x.MemberId, x.MembershipId });
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

            migrationBuilder.CreateIndex(
                name: "IX_MemberHasMembership_MemberId",
                table: "MemberHasMembership",
                column: "MemberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberHasMembership_MembershipId",
                table: "MemberHasMembership",
                column: "MembershipId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberHasMembership");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Membership",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "MembershipId",
                table: "Member",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Member_MembershipId",
                table: "Member",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Membership_MembershipId",
                table: "Member",
                column: "MembershipId",
                principalTable: "Membership",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
