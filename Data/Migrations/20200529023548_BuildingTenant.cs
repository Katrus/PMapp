using Microsoft.EntityFrameworkCore.Migrations;

namespace PMApp.Data.Migrations
{
    public partial class BuildingTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BuildingId",
                table: "Tenant",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_BuildingId",
                table: "Tenant",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_Buildings_BuildingId",
                table: "Tenant",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "BuildingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenant_Buildings_BuildingId",
                table: "Tenant");

            migrationBuilder.DropIndex(
                name: "IX_Tenant_BuildingId",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Tenant");
        }
    }
}
