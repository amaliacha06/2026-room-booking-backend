using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace room_booking_backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialBookingsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Purpose" },
                values: new object[] { new DateTime(2026, 2, 10, 18, 14, 14, 167, DateTimeKind.Utc).AddTicks(6284), "Workshop UI/UX" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Purpose" },
                values: new object[] { new DateTime(2026, 2, 10, 18, 14, 14, 167, DateTimeKind.Utc).AddTicks(6311), "Pameran PENSASI & Beasiswa Study Aboard" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Purpose" },
                values: new object[] { new DateTime(2026, 2, 10, 17, 36, 8, 758, DateTimeKind.Utc).AddTicks(9417), "Belajar pembuatan web" });

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Purpose" },
                values: new object[] { new DateTime(2026, 2, 10, 17, 36, 8, 758, DateTimeKind.Utc).AddTicks(9440), "Tes pengajuan peminjaman" });
        }
    }
}
