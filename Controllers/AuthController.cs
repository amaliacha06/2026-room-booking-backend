using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookingBackend.Data;
using RoomBookingBackend.DTOs;
using RoomBookingBackend.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens; // Untuk keamanan token
using System.IdentityModel.Tokens.Jwt; //Untuk membuat token JWT
using System.Security.Claims; // Untuk menyimpan data user di dalam token
using System.Text; //  Untuk encoding kunci rahasia

namespace RoomBookingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration; //Agar bisa baca appsettings.json
        // Tambahkan IConfiguration di Constructor
        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 1. ENDPOINT REGISTRASI
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            // Cek apakah email sudah dipakai
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("Email sudah terdaftar!");
            }
            // Proses "Mengacak" Password pakai BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Simpan data dari DTO ke Model User
            var user = new User
            {
                FullName = request.FullName,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash, // Yang disimpan adalah versi acak
                Position = request.Position,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow // Tanggal otomatis dari sistem
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registrasi akun berhasil!");
        }

        // 2. ENDPOINT LOGIN (Nanti lengkapi dengan Token)
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            // Cari user berdasarkan email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return BadRequest("User tidak ditemukan!");
            }

            // Verifikasi Password: Cek apakah input cocok dengan Hash di database
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Password salah!");
            }
            // Panggil fungsi buat token
            var token = CreateToken(user);

            return Ok(new
            {
                message = "Login sukses!",
                token = token, // Kirim token ke user
                user = new {  id = user.Id, user.FullName, user.Email, user.Position }
            });
        }
        // Fungsi baru untuk membuat Token JWT
        private string CreateToken(User user)
        {
            // Ambil data dari appsettings.json
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Data yang akan dibawa oleh token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Position ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Token berlaku 1 hari
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}