using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RoomBookingBackend.Data;
using RoomBookingBackend.Models;

namespace RoomBookingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/Bookings (Untuk nampilin tabel di Frontend)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBookings()
        {
            // PENTING: Pakai .Include agar Nama Ruangan & User bisa muncul di UI
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.IsDeleted == false)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new
                {
                    b.Id,
                    UserName = b.User.FullName,
                    RoomName = b.Room.Name,
                    b.Purpose,
                    b.StartTime,
                    b.EndTime,
                    b.Status,
                    b.CreatedAt
                })
                .ToListAsync();

            return Ok(bookings);
        }

        // 2. POST: api/Bookings (Untuk fitur 'Pilih Rentang Waktu' & Simpan)
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            // 1. Validasi: Jam Selesai tidak boleh sebelum Jam Mulai
            if (booking.EndTime <= booking.StartTime)
            {
                return BadRequest(new { message = "Waktu selesai harus lebih lambat dari waktu mulai!" });
            }
            // LOGIKA VALIDASI BENTROK (Fitur Paling Penting!)
            var isConflict = await _context.Bookings
                .AnyAsync(b => b.RoomId == booking.RoomId &&
                               b.IsDeleted == false &&
                               ((booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                                (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
                                (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)));

            if (isConflict)
            {
                return Conflict(new { message = "Maaf, ruangan sudah dipesan pada jam tersebut!" });
            }
            //Lanjutkan simpan ke database
            booking.CreatedAt = DateTime.UtcNow;
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookings), new { id = booking.Id }, booking);
        }
        // 3. GET [id]: api/Bookings/ (Untuk ambil detail satu booking saja)
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetBookingById(int id)
        {
            // Cari satu booking, sertakan data Room dan User-nya
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.Id == id && b.IsDeleted == false)
                .Select(b => new
                {
                    b.Id,
                    UserName = b.User.FullName,
                    RoomName = b.Room.Name,
                    b.Purpose,
                    b.StartTime,
                    b.EndTime,
                    b.Status,
                    b.CreatedAt
                })
                .FirstOrDefaultAsync();

            // Jika id tidak ditemukan, kasih respon 404
            if (booking == null)
            {
                return NotFound(new { message = $"Data booking dengan ID {id} tidak ditemukan." });
            }

            return Ok(booking);
        }
        // 4.PUT: api/Bookings/{id} (Untuk User Edit Pesanan)
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] Booking updatedBooking)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            // Logika User: Hanya bisa edit kalau masih Pending
            if (booking.Status != "Pending")
            {
                return BadRequest(new { message = "Pesanan sudah diproses admin, tidak bisa diubah lagi!" });
            }

            // Sekarang updatedBooking sudah ada di parameter, jadi tidak error lagi!
            booking.Purpose = updatedBooking.Purpose;
            booking.StartTime = updatedBooking.StartTime;
            booking.EndTime = updatedBooking.EndTime;
            booking.RoomId = updatedBooking.RoomId;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        // 5. PUT: api/Bookings/ (Untuk Admin menyetujui atau menolak pinjaman)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            //Memperbarui status sesuai input
            booking.Status = newStatus; // Misal: "Approved" atau "Rejected"
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // 5. DELETE: api/Bookings (Untuk menghapus data booking)
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();
            // 1. CEK: Apakah dia Admin?
            bool isAdmin = User.IsInRole("Admin");

            // 2. LOGIKA: Jika BUKAN Admin DAN status sudah BUKAN Pending, maka TOLAK
            if (!isAdmin && booking.Status != "Pending")
            {
                return BadRequest(new { message = "Booking sudah disetujui Admin!" });
            }

            // 3. Jika dia Admin, atau dia Mahasiswa tapi statusnya masih Pending, maka eksekusi Soft Delete
            booking.IsDeleted = true;
            await _context.SaveChangesAsync();
            // Kita pakai Soft Delete agar data tidak benar-benar hilang dari database
            booking.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}