using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PMApp.Data.Migrations
{
    public partial class AddDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Org_name = table.Column<string>(nullable: false),
                    Unit_Count = table.Column<int>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Zip_code = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    TPID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    TID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Last_name = table.Column<string>(nullable: false),
                    First_name = table.Column<string>(nullable: false),
                    Employer = table.Column<string>(nullable: true),
                    Salary = table.Column<int>(nullable: true),
                    Lease_start_date = table.Column<DateTime>(nullable: false),
                    Lease_end_date = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Pets = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.TID);
                });

            migrationBuilder.CreateTable(
                name: "Tax_Parcels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    BuildingId = table.Column<long>(nullable: false),
                    amount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tax_Parcels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tax_Parcels_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    UID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unit_Number = table.Column<int>(nullable: false),
                    Rent_Amount = table.Column<float>(nullable: false),
                    BuildingId = table.Column<long>(nullable: false),
                    Square_footage = table.Column<int>(nullable: true),
                    Occupied = table.Column<string>(nullable: true),
                    Ready_to_rent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.UID);
                    table.ForeignKey(
                        name: "FK_Unit_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Infractions",
                columns: table => new
                {
                    IID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day_opened = table.Column<DateTime>(nullable: false),
                    Day_closed = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Resolution = table.Column<string>(nullable: true),
                    TenantTID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infractions", x => x.IID);
                    table.ForeignKey(
                        name: "FK_Infractions_Tenant_TenantTID",
                        column: x => x.TenantTID,
                        principalTable: "Tenant",
                        principalColumn: "TID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    VID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    License_plate = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Make = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    stall_number = table.Column<int>(nullable: false),
                    TenantTID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.VID);
                    table.ForeignKey(
                        name: "FK_Vehicle_Tenant_TenantTID",
                        column: x => x.TenantTID,
                        principalTable: "Tenant",
                        principalColumn: "TID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Move_in",
                columns: table => new
                {
                    MIID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitUID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Carpet = table.Column<string>(nullable: true),
                    Appliances = table.Column<string>(nullable: true),
                    Walls = table.Column<string>(nullable: true),
                    Refundable_deposit = table.Column<float>(nullable: false),
                    Nonrefundable_deposit = table.Column<float>(nullable: false),
                    Pet_deposit = table.Column<float>(nullable: false),
                    TenantTID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Move_in", x => x.MIID);
                    table.ForeignKey(
                        name: "FK_Move_in_Tenant_TenantTID",
                        column: x => x.TenantTID,
                        principalTable: "Tenant",
                        principalColumn: "TID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Move_in_Unit_UnitUID",
                        column: x => x.UnitUID,
                        principalTable: "Unit",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Move_out",
                columns: table => new
                {
                    MOID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Carpet = table.Column<string>(nullable: true),
                    Appliances = table.Column<string>(nullable: true),
                    Walls = table.Column<string>(nullable: true),
                    Cleaning_fee = table.Column<float>(nullable: false),
                    Damage_fee = table.Column<float>(nullable: false),
                    fees_paid = table.Column<float>(nullable: false),
                    TenantTID = table.Column<int>(nullable: false),
                    UnitUID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Move_out", x => x.MOID);
                    table.ForeignKey(
                        name: "FK_Move_out_Tenant_TenantTID",
                        column: x => x.TenantTID,
                        principalTable: "Tenant",
                        principalColumn: "TID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Move_out_Unit_UnitUID",
                        column: x => x.UnitUID,
                        principalTable: "Unit",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rent",
                columns: table => new
                {
                    RID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_due = table.Column<DateTime>(nullable: false),
                    Date_paid = table.Column<DateTime>(nullable: false),
                    Rent_amount = table.Column<float>(nullable: false),
                    Pet_fee = table.Column<float>(nullable: false),
                    Parking_fee = table.Column<float>(nullable: false),
                    TenantTID = table.Column<int>(nullable: false),
                    UnitUID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rent", x => x.RID);
                    table.ForeignKey(
                        name: "FK_Rent_Tenant_TenantTID",
                        column: x => x.TenantTID,
                        principalTable: "Tenant",
                        principalColumn: "TID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rent_Unit_UnitUID",
                        column: x => x.UnitUID,
                        principalTable: "Unit",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Repair_History",
                columns: table => new
                {
                    RHID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cost = table.Column<float>(nullable: false),
                    Work_description = table.Column<string>(nullable: true),
                    Ticket_opened = table.Column<DateTime>(nullable: false),
                    Ticket_closed = table.Column<DateTime>(nullable: false),
                    Work_started = table.Column<DateTime>(nullable: false),
                    Work_ended = table.Column<DateTime>(nullable: false),
                    UnitUID = table.Column<int>(nullable: false),
                    ContractorCID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repair_History", x => x.RHID);
                    table.ForeignKey(
                        name: "FK_Repair_History_Unit_UnitUID",
                        column: x => x.UnitUID,
                        principalTable: "Unit",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contractor",
                columns: table => new
                {
                    CID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company_name = table.Column<string>(nullable: true),
                    Contact_name = table.Column<string>(nullable: true),
                    Specialty = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Zip_code = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Repair_HistoryRHID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractor", x => x.CID);
                    table.ForeignKey(
                        name: "FK_Contractor_Repair_History_Repair_HistoryRHID",
                        column: x => x.Repair_HistoryRHID,
                        principalTable: "Repair_History",
                        principalColumn: "RHID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contractor_Repair_HistoryRHID",
                table: "Contractor",
                column: "Repair_HistoryRHID");

            migrationBuilder.CreateIndex(
                name: "IX_Infractions_TenantTID",
                table: "Infractions",
                column: "TenantTID");

            migrationBuilder.CreateIndex(
                name: "IX_Move_in_TenantTID",
                table: "Move_in",
                column: "TenantTID");

            migrationBuilder.CreateIndex(
                name: "IX_Move_in_UnitUID",
                table: "Move_in",
                column: "UnitUID");

            migrationBuilder.CreateIndex(
                name: "IX_Move_out_TenantTID",
                table: "Move_out",
                column: "TenantTID");

            migrationBuilder.CreateIndex(
                name: "IX_Move_out_UnitUID",
                table: "Move_out",
                column: "UnitUID");

            migrationBuilder.CreateIndex(
                name: "IX_Rent_TenantTID",
                table: "Rent",
                column: "TenantTID");

            migrationBuilder.CreateIndex(
                name: "IX_Rent_UnitUID",
                table: "Rent",
                column: "UnitUID");

            migrationBuilder.CreateIndex(
                name: "IX_Repair_History_ContractorCID",
                table: "Repair_History",
                column: "ContractorCID");

            migrationBuilder.CreateIndex(
                name: "IX_Repair_History_UnitUID",
                table: "Repair_History",
                column: "UnitUID");

            migrationBuilder.CreateIndex(
                name: "IX_Tax_Parcels_BuildingId",
                table: "Tax_Parcels",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_BuildingId",
                table: "Unit",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_TenantTID",
                table: "Vehicle",
                column: "TenantTID");

            migrationBuilder.AddForeignKey(
                name: "FK_Repair_History_Contractor_ContractorCID",
                table: "Repair_History",
                column: "ContractorCID",
                principalTable: "Contractor",
                principalColumn: "CID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contractor_Repair_History_Repair_HistoryRHID",
                table: "Contractor");

            migrationBuilder.DropTable(
                name: "Infractions");

            migrationBuilder.DropTable(
                name: "Move_in");

            migrationBuilder.DropTable(
                name: "Move_out");

            migrationBuilder.DropTable(
                name: "Rent");

            migrationBuilder.DropTable(
                name: "Tax_Parcels");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.DropTable(
                name: "Repair_History");

            migrationBuilder.DropTable(
                name: "Contractor");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
