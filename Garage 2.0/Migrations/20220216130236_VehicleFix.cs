using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage_2._0.Migrations
{
    public partial class VehicleFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehicleType_TypeName",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "TypeName",
                table: "Vehicle",
                newName: "VehicleTypeName");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_TypeName",
                table: "Vehicle",
                newName: "IX_Vehicle_VehicleTypeName");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehicleType_VehicleTypeName",
                table: "Vehicle",
                column: "VehicleTypeName",
                principalTable: "VehicleType",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehicleType_VehicleTypeName",
                table: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "VehicleTypeName",
                table: "Vehicle",
                newName: "TypeName");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicle_VehicleTypeName",
                table: "Vehicle",
                newName: "IX_Vehicle_TypeName");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehicleType_TypeName",
                table: "Vehicle",
                column: "TypeName",
                principalTable: "VehicleType",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
