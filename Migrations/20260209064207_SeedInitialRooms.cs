using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace room_booking_backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "Facilities", "IsAvailable", "IsDeleted", "Location", "Name" },
                values: new object[,]
                {
                    { 1, 100, "Sound System, Projector, AC", true, false, "Gedung D4 Lantai 2", "Ruang Teater 1" },
                    { 2, 10, "Whiteboard, AC", true, false, "Gedung D3 Lantai 1", "Ruang Meeting Kecil" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
