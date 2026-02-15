using System.ComponentModel.DataAnnotations;

namespace RoomBookingBackend.DTOs
{
    public class BookingUpdateDto
    {
        [Required(ErrorMessage = "Tujuan peminjaman wajib diisi")]
        public string Purpose { get; set; } = string.Empty;

        [Required(ErrorMessage = "Waktu mulai wajib diisi")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Waktu selesai wajib diisi")]
        public DateTime EndTime { get; set; }
    }
}
