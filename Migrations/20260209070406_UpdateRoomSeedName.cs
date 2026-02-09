using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace room_booking_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomSeedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Capacity", "Facilities", "Location", "Name" },
                values: new object[] { 50, "Sound System, Projector, Meja, Kursih, AC", "Gedung D3 Lantai 1", "Mini Theater D3" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Capacity", "Facilities", "Location", "Name" },
                values: new object[] { 200, " LED Display/Videotron, Sound System, Projector, Meja, Kursih, AC", "Lt.6 Gedung Pasca Sarjana", "Auditorium Gedung Pasca Sarjana" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Capacity", "Facilities", "Location", "Name" },
                values: new object[] { 100, "Sound System, Projector, AC", "Gedung D4 Lantai 2", "Ruang Teater 1" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Capacity", "Facilities", "Location", "Name" },
                values: new object[] { 10, "Whiteboard, AC", "Gedung D3 Lantai 1", "Ruang Meeting Kecil" });
        }
    }
}
