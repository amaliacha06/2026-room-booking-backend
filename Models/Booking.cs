namespace RoomBookingBackend.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Room? Room { get; set; } // navigation boleh null
    }

}
