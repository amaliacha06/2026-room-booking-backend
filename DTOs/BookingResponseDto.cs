namespace RoomBookingBackend.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        
        // Menampilkan namanya
        public string UserName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        
        public string Purpose { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        // Status ini penting untuk filter di UI nanti
        public string Status { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
    }
}