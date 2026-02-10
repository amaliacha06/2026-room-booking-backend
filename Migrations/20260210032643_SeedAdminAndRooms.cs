using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace room_booking_backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminAndRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "PasswordHash", "PhoneNumber", "Position", "Username" },
                values: new object[] { 2, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gmail.com", "Admin1", "$2a$11$qR7mB.wO4FmY9n9y/q.uSu6X6.vR/8p4Wp5K5QY9Gz8e4b7X6c1qG", "081122334455", "Admin", "adminku" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
