using System.ComponentModel.DataAnnotations;

namespace RoomBookingBackend.Models
{
    public class Room
    {
        public int Id { get; set; } // ID Ruangan (Tampil di Tabel)
        public required string Name { get; set; } // Nama Ruangan (Tampil di Tabel)
        public int Capacity { get; set; } // Muncul di Detail
        public string Location { get; set; } = string.Empty; // Muncul di Detail
        public bool IsAvailable { get; set; } = true; // Status (Tampil di Tabel)
        public string Facilities { get; set; } = string.Empty; // Muncul di Detail
        public bool IsDeleted { get; set; } = false; // Soft Delete sesuai Notes
    }
}
