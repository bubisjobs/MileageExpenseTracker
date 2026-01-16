using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MileageExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class ChangedColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MileageClaims_AspNetUsers_ApproverId",
                table: "MileageClaims");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "MileageClaims",
                newName: "TeamLeadApprover");

            migrationBuilder.AlterColumn<string>(
                name: "FinanceApprover",
                table: "MileageClaims",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ApproverId",
                table: "MileageClaims",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "MileageClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MileageClaims_AspNetUsers_ApproverId",
                table: "MileageClaims",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MileageClaims_AspNetUsers_ApproverId",
                table: "MileageClaims");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "MileageClaims");

            migrationBuilder.RenameColumn(
                name: "TeamLeadApprover",
                table: "MileageClaims",
                newName: "EmployeeId");

            migrationBuilder.AlterColumn<string>(
                name: "FinanceApprover",
                table: "MileageClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApproverId",
                table: "MileageClaims",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MileageClaims_AspNetUsers_ApproverId",
                table: "MileageClaims",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
