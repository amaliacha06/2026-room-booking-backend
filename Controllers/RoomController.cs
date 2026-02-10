using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookingBackend.Data;
using RoomBookingBackend.Models;
using RoomBookingBackend.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace RoomBookingBackend.Controllers
{
    [Route("api/[controller]")] // Attribute-based Routing sesuai Notes
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomController(AppDbContext context)
        {
            _context = context;
        }

        //READ ALL (Data Utama)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            // Hanya ambil yang belum di-soft delete
            return await _context.Rooms.Where(r => !r.IsDeleted).ToListAsync();
        }

        //READ DEATIL
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            if (room == null) return NotFound(new { message = "Ruangan tidak ditemukan" });
            return room;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<Room>> PostRoom(RoomCreateDto dto)
        {
            // Mapping dari DTO ke Model
            var room = new Room {
                Name = dto.Name,
                Capacity = dto.Capacity,
                Location = dto.Location,
                Facilities = dto.Facilities
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Cuma Admin yang boleh edit!
        public async Task<IActionResult> PutRoom(int id, RoomCreateDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null || room.IsDeleted) return NotFound();

            room.Name = dto.Name;
            room.Capacity = dto.Capacity;
            room.Location = dto.Location;
            room.Facilities = dto.Facilities;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Hanya Admin yang boleh hapus
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            // Implementasi Soft Delete sesuai Notes
            room.IsDeleted = true; 
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}