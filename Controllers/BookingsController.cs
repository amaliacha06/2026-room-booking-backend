using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RoomBookingBackend.Data;
using RoomBookingBackend.Models;
using RoomBookingBackend.DTOs;

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
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetBookings()
        {
            // Pakai .Include agar Nama Ruangan & User bisa muncul di UI
            var bookings = await _context.Bookings
                .Include(b => b.Room) // Ambil data ruangan
                .Include(b => b.User)
                .Where(b => b.IsDeleted == false)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingResponseDto // Ubah ke DTO 
                {
                    Id = b.Id,
                    RoomName = b.Room.Name,
                    UserName = b.User.Username,
                    Purpose = b.Purpose,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();

            return Ok(bookings);
        }
        [Authorize]
        [HttpGet("my-bookings")]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetMyBookings()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User ID tidak ditemukan dalam token." });
            }
            var userId = int.Parse(userIdClaim.Value);


            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.UserId == userId && b.IsDeleted == false)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    RoomName = b.Room.Name,
                    UserName = b.User.Username,
                    Purpose = b.Purpose,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();

            return Ok(bookings);
        }


        // 2. POST: api/Bookings (Untuk fitur 'Pilih Rentang Waktu' & Simpan)
        [Authorize]
        [HttpPost]
        // 1. return type ke BookingResponseDto dan parameter ke BookingCreateDto
        public async Task<ActionResult<BookingResponseDto>> CreateBooking(BookingCreateDto bookingDto)
        {
            // 1. Validasi: Jam Selesai tidak boleh sebelum Jam Mulai
            if (bookingDto.EndTime <= bookingDto.StartTime)
            {

                return BadRequest(new { message = "Waktu selesai harus lebih lambat dari waktu mulai!" });
            }
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "User ID tidak ditemukan dalam token." });
            }

            var userId = int.Parse(userIdClaim.Value);

            // 2. Mapping dari DTO ke Model asli database
            var booking = new Booking
            {
                RoomId = bookingDto.RoomId,
                UserId = userId, // ambil dari token
                Purpose = bookingDto.Purpose,
                StartTime = bookingDto.StartTime,
                EndTime = bookingDto.EndTime,
                Status = "Pending", // Otomatis diset sistem
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            // 3. LOGIKA VALIDASI BENTROK
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
            // simpan ke database
            booking.CreatedAt = DateTime.UtcNow;

            // 1. Simpan dulu ke database supaya punya ID
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // 2.BARU panggil data lengkapnya pakai Include
            var createdBooking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == booking.Id);

            // 3.Kembalikan data dalam bentuk ResponseDto agar seragam
            return CreatedAtAction(nameof(GetBookings), new { id = booking.Id }, new BookingResponseDto
            {
                Id = booking.Id,
                RoomName = createdBooking?.Room?.Name ?? "N/A",
                UserName = createdBooking?.User?.Username ?? "N/A",
                Purpose = booking.Purpose,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt
            });
        }

        // 3. GET [id]: api/Bookings/ (Untuk ambil detail satu booking saja)
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingResponseDto>> GetBookingById(int id)
        {
            // Cari satu booking, sertakan data Room dan User-nya
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.Id == id && b.IsDeleted == false)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    UserName = b.User.Username,
                    RoomName = b.Room.Name,
                    Purpose = b.Purpose,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt
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
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingUpdateDto updatedDto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            // Hanya boleh edit jika status Pending
            if (booking.Status != "Pending")
            {
                return BadRequest(new { message = "Pesanan sudah diproses admin, tidak bisa diubah lagi!" });
            }

            // Update data
            booking.Purpose = updatedDto.Purpose;
            booking.StartTime = updatedDto.StartTime;
            booking.EndTime = updatedDto.EndTime;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Booking berhasil diperbarui" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? "Terjadi kesalahan sistem." });
            }
        }

        // 5. PUT: api/Bookings/ (Untuk Admin menyetujui atau menolak pinjaman)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<ActionResult<BookingResponseDto>> UpdateBookingStatus(int id, [FromBody] string status)
        {
            var booking = await _context.Bookings
         .Include(b => b.Room)
         .Include(b => b.User)
         .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) return NotFound();

            //Memperbarui status sesuai input
            booking.Status = status; // Misal: "Approved" atau "Rejected"
            await _context.SaveChangesAsync();

            return Ok(new BookingResponseDto // Kembalikan DTO
            {
                Id = booking.Id,
                RoomName = booking.Room.Name,
                UserName = booking.User.Username,
                Status = booking.Status,
                Purpose = booking.Purpose,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                CreatedAt = booking.CreatedAt
            });
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

            // 2. Jika Bukan Admin dan status sudah Bukan Pending, maka TOLAK
            if (!isAdmin && booking.Status != "Pending")
            {
                return BadRequest(new { message = "Booking sudah disetujui Admin!" });
            }

            // 3. Jika dia Admin, atau dia Mahasiswa tapi statusnya masih Pending, maka eksekusi Soft Delete
            booking.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}