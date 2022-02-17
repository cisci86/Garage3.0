using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class fix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberHasMembership_Membership_MembershipId",
                table: "MemberHasMembership");

            migrationBuilder.DropIndex(
                name: "IX_MemberHasMembership_MembershipId",
                table: "MemberHasMembership");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MemberHasMembership_MembershipId",
                table: "MemberHasMembership",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberHasMembership_Membership_MembershipId",
                table: "MemberHasMembership",
                column: "MembershipId",
                principalTable: "Membership",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
