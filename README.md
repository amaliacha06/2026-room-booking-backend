# 2026-room-booking-backend
Bagian Backend Sistem Peminjaman Ruangan

# Room Booking System API
Sistem Backend berbasis REST API untuk manajemen peminjaman ruangan menggunakan ASP.NET Core untuk mengelola peminjaman ruangan, autentikasi user, dan logika validasi.

# Teknologi Utama
- Framework: .NET 8 (ASP.NET Core)
- Database: PostgreSQL
- ORM: Entity Framework Core
- Security: JWT Authentication
- Documentation: Swagger UI

## Cara Menjalankan Project
1. Clone repository 
2. Pastikan PostgreSQL sudah aktif dan buat database baru
3. Sesuaikan ConnectionStrings di file appsettings.json
4. Jalankan perintah dotnet ef database update di terminal untuk migrasi tabel dan seeder
5. Jalankan aplikasi dengan dotnet run.
6. Buka http://localhost:[PORT]/swagger untuk mulai mencoba API

## Proses Pengembangan

# 1.Setup Database & Environment (feature/setup-db)
- Framework Initialization: Inisialisasi proyek menggunakan ASP.NET Core Web API
- Database Connection: Konfigurasi koneksi ke PostgreSQL melalui `appsettings.json`
- Entity Framework Core: Instalasi NuGet Packages untuk EF Core dan PostgreSQL Provider
- Swagger Setup: Konfigurasi Swagger UI sebagai dokumentasi API untuk testing

# 2.Room Management Module (feature/room-crud)
- Components: Pembuatan `Models`, `Controller`, dan `AppDbContext` khusus untuk data ruangan
- Data Transformation: Implementasi DTOs untuk keamanan dan standarisasi respon API
- Seeding Data: Penerapan Seeder untuk mengisi data ruangan awal secara otomatis saat migrasi
- Verification: Berhasil melakukan testing CRUD melalui Swagger dan pengecekan data di pgAdmin

# 3.User Authentication & Security (feature/user-auth`)
- Identity System : Implementasi fitur Register dan Login untuk dua role pengguna: Admin dan Mahasiswa
- JWT Security: Penerapan Bearer Token Authentication menggunakan JWT untuk mengamankan endpoint
- Role-Based Access Control (RBAC): Pengaturan izin akses di mana Admin memiliki akses penuh dan Mahasiswa memiliki akses terbatas
- User Seeding: Penambahan data awal user (Admin & Mahasiswa) untuk keperluan testing
- Verification: Berhasil melakukan testing melalui Swagger dan pengecekan data di pgAdmin

# 4.Booking Core System (feature/booking-crud)
- Relationship Mapping: Menghubungkan entitas User dan Room ke dalam entitas Booking melalui Foreign Key
- Conflict Validation Logic: Implementasi algoritma untuk mencegah double-booking pada ruangan dan waktu yang sama
- Business Rules: Validasi durasi waktu (jam selesai tidak boleh lebih awal dari jam mulai)
- Soft Delete: Mekanisme pembatalan booking melalui kolom IsDeleted untuk menjaga integritas riwayat data
- - Verification: Berhasil melakukan testing CRUD melalui Swagger dan pengecekan data di pgAdmin
