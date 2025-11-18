using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Librarians",
                keyColumn: "ID",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2b$12$C6UzMDM.H6dfI/f/IK6G7.ueWnACpPiiPMTKoXoB4GAibl0JZ8D4e");

            migrationBuilder.UpdateData(
                table: "Librarians",
                keyColumn: "ID",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2b$12$C6UzMDM.H6dfI/f/IK6G7.NMNsvcRC5TBpBzR40cz1K3pzGNuHPgi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Librarians",
                keyColumn: "ID",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$4Z7eSFwe/E0NXaiT/MwUn.JEl6NgEGvQIsmxTTF1oT5PN8ENJfaCe");

            migrationBuilder.UpdateData(
                table: "Librarians",
                keyColumn: "ID",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$BDoW42ycnv59lLtKqLNN7enbDN9gUKOI6WGHkD8Dqu3uFrQ7Xxfb.");
        }
    }
}
