using Microsoft.EntityFrameworkCore;
using RoomBookingBackend.Models;

namespace RoomBookingBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }

        // Tambahkan ini untuk Data Seeding 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding User Admin
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2, // Gunakan ID 2 karena ID 1 sudah pakai di pgAdmin
                FullName = "Admin1",
                Username = "adminku",
                Email = "admin@gmail.com",
                PasswordHash = "$2a$11$L1gcQKHTzIAIraON.575GOYlsqfFcWhu5raU8YKTGkKRg4k/OdVsa",
                Position = "Admin",
                PhoneNumber = "081122334455",
                CreatedAt = new DateTime(2026, 2, 10, 0, 0, 0, DateTimeKind.Utc)
            });
            base.OnModelCreating(modelBuilder);

            // Menambahkan data awal(seeding) untuk tabel Rooms
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    Name = "Mini Theater D3",
                    Capacity = 50,
                    Location = "Gedung D3 Lantai 1",
                    Facilities = "Sound System, Projector, Meja, Kursih, AC",
                    IsAvailable = true,
                    IsDeleted = false
                },
                new Room
                {
                    Id = 2,
                    Name = "Auditorium Gedung Pasca Sarjana",
                    Capacity = 200,
                    Location = "Lt.6 Gedung Pasca Sarjana",
                    Facilities = " LED Display/Videotron, Sound System, Projector, Meja, Kursih, AC",
                    IsAvailable = true,
                    IsDeleted = false
                }
            );
        }
    }
}