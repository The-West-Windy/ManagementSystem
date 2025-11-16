using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServerApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Librarians",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Librarians", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BorrowRequests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibrarianID = table.Column<int>(type: "int", nullable: false),
                    BookID = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BorrowRequests_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowRequests_Librarians_LibrarianID",
                        column: x => x.LibrarianID,
                        principalTable: "Librarians",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "ID", "Author", "Status", "Title" },
                values: new object[,]
                {
                    { 1, "Сунь-Цзи", "Available", "Мистецтво війни" },
                    { 2, "Тарас Шевченко", "Borrowed", "Кобзар" },
                    { 3, "Джордж Орвелл", "Available", "1984" }
                });

            migrationBuilder.InsertData(
                table: "Librarians",
                columns: new[] { "ID", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "olena.koval@library.com", "Олена Коваль", "$2b$12$C6UzMDM.H6dfI/f/IK6G7.ueWnACpPiiPMTKoXoB4GAibl0JZ8D4e" },
                    { 2, "ihor.petrenko@library.com", "Ігор Петренко", "$2a$11$BJ6xZCQBBYXQLDjed75FPeFuquR7YDrn.EI47VkK0j6v9.3fMT6f6" }
                });

            migrationBuilder.InsertData(
                table: "BorrowRequests",
                columns: new[] { "ID", "BookID", "LibrarianID", "RequestDate", "Status" },
                values: new object[,]
                {
                    { 1, 2, 1, new DateTime(2025, 11, 3, 21, 55, 7, 305, DateTimeKind.Local).AddTicks(6918), "Approved" },
                    { 2, 3, 2, new DateTime(2025, 11, 3, 21, 55, 7, 308, DateTimeKind.Local).AddTicks(1976), "Pending" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRequests_BookID",
                table: "BorrowRequests",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRequests_LibrarianID",
                table: "BorrowRequests",
                column: "LibrarianID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowRequests");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Librarians");
        }
    }
}
