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
                name: "Coaches",
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
                    table.PrimaryKey("PK_Coaches", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoachID = table.Column<int>(type: "int", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Classes_Coaches_CoachID",
                        column: x => x.CoachID,
                        principalTable: "Coaches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoachID = table.Column<int>(type: "int", nullable: false),
                    ClassID = table.Column<int>(type: "int", nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Bookings_Classes_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Coaches_CoachID",
                        column: x => x.CoachID,
                        principalTable: "Coaches",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "ID", "Email", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "olena.fit@fitgym.com", "Олена Фітнес", "$2a$11$hMgF1UtGTeh2SmFxtodZje0aoyxzCFyUn4wUe3rRwxEzqieYvNaqW" },
                    { 2, "ihor.trainer@fitgym.com", "Ігор Тренер", "$2a$11$KWJFyA1XhpSB.45m2pVC2.3ODUpnbfUgLyD/cZVr7Rq8RfehL8Nxe" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ID", "CoachID", "Name", "TimeSlot" },
                values: new object[,]
                {
                    { 1, 1, "Ранковий HIIT", "Пн 08:00" },
                    { 2, 2, "Силова підготовка", "Вт 18:00" },
                    { 3, 1, "Функціональний мікс", "Чт 19:30" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "ID", "ClassID", "ClientName", "CoachID", "Status" },
                values: new object[,]
                {
                    { 1, 2, "Анна", 1, "Confirmed" },
                    { 2, 3, "Петро", 2, "Pending" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ClassID",
                table: "Bookings",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CoachID",
                table: "Bookings",
                column: "CoachID");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CoachID",
                table: "Classes",
                column: "CoachID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Coaches");
        }
    }
}
