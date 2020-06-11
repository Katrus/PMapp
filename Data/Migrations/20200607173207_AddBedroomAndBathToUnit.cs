using Microsoft.EntityFrameworkCore.Migrations;

namespace PMApp.Data.Migrations
{
    public partial class AddBedroomAndBathToUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Bath",
                table: "Unit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Bedroom",
                table: "Unit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bath",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "Bedroom",
                table: "Unit");
        }
    }
}
