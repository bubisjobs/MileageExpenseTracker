using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MileageExpenseTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class DBCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MileageClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RatePerKm = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DecisionAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DecisionComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalKilometers = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalReimbursement = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MileageClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MileageClaims_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MileageClaims_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MileageTrips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TripDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TripTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EndLocation = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Kilometers = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reimbursement = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MileageTrips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MileageTrips_MileageClaims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "MileageClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MileageClaims_ApproverId",
                table: "MileageClaims",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_MileageClaims_EmployeeId",
                table: "MileageClaims",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MileageTrips_ClaimId",
                table: "MileageTrips",
                column: "ClaimId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MileageTrips");

            migrationBuilder.DropTable(
                name: "MileageClaims");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
