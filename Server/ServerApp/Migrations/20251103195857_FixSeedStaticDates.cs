using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedStaticDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BorrowRequests",
                keyColumn: "ID",
                keyValue: 1,
                column: "RequestDate",
                value: new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "BorrowRequests",
                keyColumn: "ID",
                keyValue: 2,
                column: "RequestDate",
                value: new DateTime(2025, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BorrowRequests",
                keyColumn: "ID",
                keyValue: 1,
                column: "RequestDate",
                value: new DateTime(2025, 11, 3, 21, 55, 7, 305, DateTimeKind.Local).AddTicks(6918));

            migrationBuilder.UpdateData(
                table: "BorrowRequests",
                keyColumn: "ID",
                keyValue: 2,
                column: "RequestDate",
                value: new DateTime(2025, 11, 3, 21, 55, 7, 308, DateTimeKind.Local).AddTicks(1976));
        }
    }
}
