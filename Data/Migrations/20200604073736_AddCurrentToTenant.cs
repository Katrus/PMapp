using Microsoft.EntityFrameworkCore.Migrations;

namespace PMApp.Data.Migrations
{
    public partial class AddCurrentToTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Current",
                table: "Tenant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Current",
                table: "Tenant");
        }
    }
}
