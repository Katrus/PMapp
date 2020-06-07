using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMApp.Data.Migrations
{
    public partial class Checklist2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checklist",
                columns: table => new
                {
                    checklistId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reminder = table.Column<string>(nullable: true),
                    When = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklist", x => x.checklistId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checklist");
        }
    }
}
