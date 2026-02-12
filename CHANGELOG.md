# Changelog
Semua perubahan pada proyek **Room Booking Backend** akan dicatat dalam file ini.

# [1.0.0] - 2026-02-12

# Added
- Initial Setup: Inisialisasi project ASP.NET Core dan konfigurasi koneksi database PostgreSQL
- Database Schema: Implementasi tabel `Rooms`, `Users`, dan `Bookings` serta relasi antar tabel
- Room Management: Fitur CRUD lengkap untuk pengelolaan data ruangan oleh Admin
- User Authentication: Sistem login dan registrasi mahasiswa/admin menggunakan JWT Token
- Booking Core System: Fitur peminjaman ruangan yang menghubungkan User ID dan Room ID
- Conflict Validation: Logika pencegahan bentrok jadwal jika ruangan dipesan pada waktu yang sama
- Security (DTOs): Implementasi Data Transfer Object untuk melindungi data sensitif di level Controller
- Soft Delete: Mekanisme pembatalan booking melalui kolom `IsDeleted` untuk integritas data

### Fixed
- Perbaikan konfigurasi pada `Program.cs` dengan membuat controller (`AddControllers`) dan pemetaan rute (`MapControllers`) agar API dapat diakses oleh client
- Perbaikan validasi rentang waktu (jam selesai tidak boleh mendahului jam mulai)
- Perbaikan query data menggunakan `.Include()` agar nama ruangan/user tampil di respon API

# Documentation
- Penulisan panduan instalasi dan riwayat proses di `README.md`
- Pembuatan file `CHANGELOG.md` untuk pelacakan versi