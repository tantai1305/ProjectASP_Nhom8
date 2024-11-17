using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectASP_Nhom8.Models
{
    public partial class QLThamQuanDNContext : DbContext
    {
        public QLThamQuanDNContext()
        {
        }

        public QLThamQuanDNContext(DbContextOptions<QLThamQuanDNContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChuyenThamQuan> ChuyenThamQuans { get; set; } = null!;
        public virtual DbSet<DangKy> DangKies { get; set; } = null!;
        public virtual DbSet<DoanhNghiep> DoanhNghieps { get; set; } = null!;
        public virtual DbSet<GiangVien> GiangViens { get; set; } = null!;
        public virtual DbSet<SinhVien> SinhViens { get; set; } = null!;
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChuyenThamQuan>(entity =>
            {
                entity.HasKey(e => e.MaThamQuan);

                entity.ToTable("ChuyenThamQuan");

                entity.Property(e => e.MaThamQuan).HasMaxLength(11);

                entity.Property(e => e.HocKy).HasMaxLength(10);

                entity.Property(e => e.MaDn)
                    .HasMaxLength(11)
                    .HasColumnName("MaDN");

                entity.Property(e => e.MoTa).HasColumnType("text");

                entity.Property(e => e.Msgv)
                    .HasMaxLength(11)
                    .HasColumnName("MSGV");

                entity.Property(e => e.NgayToChuc).HasColumnType("datetime");

                entity.Property(e => e.NienKhoa).HasMaxLength(20);

                entity.Property(e => e.ThoiGianBatDau).HasColumnType("datetime");

                entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime");

                entity.HasOne(d => d.MaDnNavigation)
                    .WithMany(p => p.ChuyenThamQuans)
                    .HasForeignKey(d => d.MaDn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DoanhNghiep_ChuyenThamQuan");

                entity.HasOne(d => d.MsgvNavigation)
                    .WithMany(p => p.ChuyenThamQuans)
                    .HasForeignKey(d => d.Msgv)
                    .HasConstraintName("FK_GiangVien_ChuyenThamQuan");
            });

            modelBuilder.Entity<DangKy>(entity =>
            {
                entity.HasKey(e => new { e.Mssv, e.MaThamQuan });

                entity.ToTable("DangKy");

                entity.Property(e => e.Mssv)
                    .HasMaxLength(11)
                    .HasColumnName("MSSV");

                entity.Property(e => e.MaThamQuan).HasMaxLength(11);

                entity.Property(e => e.NgayDangKy).HasColumnType("datetime");

                entity.HasOne(d => d.MaThamQuanNavigation)
                    .WithMany(p => p.DangKies)
                    .HasForeignKey(d => d.MaThamQuan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChuyenThamQuan_DangKy");

                entity.HasOne(d => d.MssvNavigation)
                    .WithMany(p => p.DangKies)
                    .HasForeignKey(d => d.Mssv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SinhVien_DangKy");
            });

            modelBuilder.Entity<DoanhNghiep>(entity =>
            {
                entity.HasKey(e => e.MaDn);

                entity.ToTable("DoanhNghiep");

                entity.Property(e => e.MaDn)
                    .HasMaxLength(11)
                    .HasColumnName("MaDN");

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.TenDn)
                    .HasMaxLength(50)
                    .HasColumnName("TenDN");
            });

            modelBuilder.Entity<GiangVien>(entity =>
            {
                entity.HasKey(e => e.Msgv);

                entity.ToTable("GiangVien");

                entity.Property(e => e.Msgv)
                    .HasMaxLength(11)
                    .HasColumnName("MSGV");

                entity.Property(e => e.Email).HasMaxLength(30);

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(10)
                    .HasColumnName("SDT");

                entity.Property(e => e.TaiKhoan).HasMaxLength(11);

                entity.HasOne(d => d.TaiKhoanNavigation)
                    .WithMany(p => p.GiangViens)
                    .HasForeignKey(d => d.TaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaiKhoan_GiangVien");
            });

            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.HasKey(e => e.Mssv);

                entity.ToTable("SinhVien");

                entity.Property(e => e.Mssv)
                    .HasMaxLength(11)
                    .HasColumnName("MSSV");

                entity.Property(e => e.Email).HasMaxLength(30);

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(10)
                    .HasColumnName("SDT");

                entity.Property(e => e.TaiKhoan).HasMaxLength(11);

                entity.HasOne(d => d.TaiKhoanNavigation)
                    .WithMany(p => p.SinhViens)
                    .HasForeignKey(d => d.TaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TaiKhoan_SinhVien");
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasKey(e => e.TaiKhoan1);

                entity.ToTable("TaiKhoan");

                entity.Property(e => e.TaiKhoan1)
                    .HasMaxLength(11)
                    .HasColumnName("TaiKhoan");

                entity.Property(e => e.MatKhau).HasMaxLength(15);

                entity.Property(e => e.Quyen).HasMaxLength(15);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
