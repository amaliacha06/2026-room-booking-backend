using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomBookingBackend.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        public string Purpose { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        // Status: Pending, Approved, Rejected, Canceled
        public string Status { get; set; } = "Pending";
        public bool IsDeleted { get; set; } = false; // Untuk Soft Delete
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}