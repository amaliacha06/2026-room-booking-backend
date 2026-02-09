using System.ComponentModel.DataAnnotations;

namespace RoomBookingBackend.DTOs
{
    public class RoomCreateDto
    {
        [Required(ErrorMessage = "Nama ruangan wajib diisi")]
        public string Name { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "Kapasitas harus antara 1 sampai 1000")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Lokasi wajib diisi")]
        public string Location { get; set; } = string.Empty;

        public string Facilities { get; set; } = string.Empty;
    }
}