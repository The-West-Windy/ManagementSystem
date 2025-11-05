using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Librarians",
                columns: new[] { "ID", "Email", "Name", "PasswordHash", "Role" },
                values: new object[] { 3, "admin@library.com", "Admin", "admin123", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Librarians",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
