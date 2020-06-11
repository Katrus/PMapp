using Microsoft.EntityFrameworkCore.Migrations;

namespace PMApp.Data.Migrations
{
    public partial class RemovePetAndParkingFromRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Parking_fee",
                table: "Rent");

            migrationBuilder.DropColumn(
                name: "Pet_fee",
                table: "Rent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Parking_fee",
                table: "Rent",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Pet_fee",
                table: "Rent",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
