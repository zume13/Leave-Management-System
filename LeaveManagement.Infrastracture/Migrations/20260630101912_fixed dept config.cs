using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixeddeptconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_DeptId",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DeptId",
                table: "Employees",
                column: "DeptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_DeptId",
                table: "Employees");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DeptId",
                table: "Employees",
                column: "DeptId",
                unique: true);
        }
    }
}
