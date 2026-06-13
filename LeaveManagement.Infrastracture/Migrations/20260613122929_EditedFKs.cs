using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditedFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerificationTokens_Employees_Id",
                table: "EmailVerificationTokens");

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_employee_id",
                table: "EmailVerificationTokens",
                column: "employee_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerificationTokens_Employees_employee_id",
                table: "EmailVerificationTokens",
                column: "employee_id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailVerificationTokens_Employees_employee_id",
                table: "EmailVerificationTokens");

            migrationBuilder.DropIndex(
                name: "IX_EmailVerificationTokens_employee_id",
                table: "EmailVerificationTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailVerificationTokens_Employees_Id",
                table: "EmailVerificationTokens",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
