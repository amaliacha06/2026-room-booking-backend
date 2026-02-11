using System.ComponentModel.DataAnnotations;

namespace RoomBookingBackend.DTOs
{
    public class BookingCreateDto
    {
        [Required(ErrorMessage = "Ruangan harus dipilih")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "User ID harus ada")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tujuan peminjaman wajib diisi")]
        public string Purpose { get; set; } = string.Empty;

        [Required(ErrorMessage = "Waktu mulai wajib diisi")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Waktu selesai wajib diisi")]
        public DateTime EndTime { get; set; }
    }
}