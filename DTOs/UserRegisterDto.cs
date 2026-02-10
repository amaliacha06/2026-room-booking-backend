using System.ComponentModel.DataAnnotations;

namespace RoomBookingBackend.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Nama lengkap wajib diisi")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username wajib diisi")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email tidak boleh kosong")]
        [EmailAddress(ErrorMessage = "Format email salah! Pastikan menggunakan '@' (contoh: amel@pens.ac.id)")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password wajib diisi")]
        [MinLength(6, ErrorMessage = "Password minimal harus 6 karakter")] // Aku turunkan ke 6 sesuai kodingan awalmu tadi
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Posisi/Jabatan harus dipilih")]
        public string Position { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}