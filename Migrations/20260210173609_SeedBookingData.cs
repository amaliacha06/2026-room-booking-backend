using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace room_booking_backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedBookingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CreatedAt", "EndTime", "IsDeleted", "Purpose", "RoomId", "StartTime", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 10, 17, 36, 8, 758, DateTimeKind.Utc).AddTicks(9417), new DateTime(2026, 2, 11, 3, 0, 0, 0, DateTimeKind.Utc), false, "Belajar pembuatan web", 1, new DateTime(2026, 2, 11, 1, 0, 0, 0, DateTimeKind.Utc), "Selesai", 1 },
                    { 2, new DateTime(2026, 2, 10, 17, 36, 8, 758, DateTimeKind.Utc).AddTicks(9440), new DateTime(2026, 2, 12, 8, 0, 0, 0, DateTimeKind.Utc), false, "Tes pengajuan peminjaman", 2, new DateTime(2026, 2, 12, 6, 0, 0, 0, DateTimeKind.Utc), "Pending", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
