using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Librarians",
                keyColumn: "ID",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$hMgF1UtGTeh2SmFxtodZje0aoyxzCFyUn4wUe3rRwxEzqieYvNaqW");

            migrationBuilder.UpdateData(
                table: "Librarians",
                keyColumn: "ID",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$KWJFyA1XhpSB.45m2pVC2.3ODUpnbfUgLyD/cZVr7Rq8RfehL8Nxe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
