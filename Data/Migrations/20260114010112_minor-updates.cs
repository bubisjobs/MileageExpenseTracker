using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MileageExpenseTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class minorupdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MileageClaims_Users_ApproverId",
                table: "MileageClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_MileageClaims_Users_EmployeeId",
                table: "MileageClaims");

            migrationBuilder.DropIndex(
                name: "IX_MileageTrips_ClaimId",
                table: "MileageTrips");

            migrationBuilder.DropIndex(
                name: "IX_MileageClaims_EmployeeId",
                table: "MileageClaims");

            migrationBuilder.AlterColumn<decimal>(
                name: "Reimbursement",
                table: "MileageTrips",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Kilometers",
                table: "MileageTrips",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalReimbursement",
                table: "MileageClaims",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalKilometers",
                table: "MileageClaims",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "RatePerKm",
                table: "MileageClaims",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_MileageTrips_ClaimId_TripDate",
                table: "MileageTrips",
                columns: new[] { "ClaimId", "TripDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MileageClaims_EmployeeId_Status_StartDate",
                table: "MileageClaims",
                columns: new[] { "EmployeeId", "Status", "StartDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_MileageClaims_Users_ApproverId",
                table: "MileageClaims",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MileageClaims_Users_EmployeeId",
                table: "MileageClaims",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MileageClaims_Users_ApproverId",
                table: "MileageClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_MileageClaims_Users_EmployeeId",
                table: "MileageClaims");

            migrationBuilder.DropIndex(
                name: "IX_MileageTrips_ClaimId_TripDate",
                table: "MileageTrips");

            migrationBuilder.DropIndex(
                name: "IX_MileageClaims_EmployeeId_Status_StartDate",
                table: "MileageClaims");

            migrationBuilder.AlterColumn<decimal>(
                name: "Reimbursement",
                table: "MileageTrips",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Kilometers",
                table: "MileageTrips",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalReimbursement",
                table: "MileageClaims",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalKilometers",
                table: "MileageClaims",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "RatePerKm",
                table: "MileageClaims",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.CreateIndex(
                name: "IX_MileageTrips_ClaimId",
                table: "MileageTrips",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_MileageClaims_EmployeeId",
                table: "MileageClaims",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MileageClaims_Users_ApproverId",
                table: "MileageClaims",
                column: "ApproverId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MileageClaims_Users_EmployeeId",
                table: "MileageClaims",
                column: "EmployeeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
