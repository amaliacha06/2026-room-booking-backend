namespace RoomBookingBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FullName { get; set; } // Untuk tampilan "Nama Lengkap"
        public required string Username { get; set; } // Untuk tampilan "Username"
        public required string Email { get; set; }
        public required string PasswordHash { get; set; } // Password yang sudah di-BCrypt
        public string Position { get; set; } = string.Empty; // Level: Mahasiswa/Dosen/Staff
        public string PhoneNumber { get; set; } = string.Empty; // Untuk Detail
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Tanggal Bergabung
    }
}
