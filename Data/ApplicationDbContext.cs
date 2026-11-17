using System;
using System.Collections.Generic;
using DatPhongKhachSan.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSan.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<chitietdatphong> chitietdatphongs { get; set; }

    public virtual DbSet<chitietsudungdichvu> chitietsudungdichvus { get; set; }

    public virtual DbSet<datphong> datphongs { get; set; }

    public virtual DbSet<dichvu> dichvus { get; set; }

    public virtual DbSet<hoadon> hoadons { get; set; }

    public virtual DbSet<khachhang> khachhangs { get; set; }

    public virtual DbSet<loaiphong> loaiphongs { get; set; }

    public virtual DbSet<nhanphong> nhanphongs { get; set; }

    public virtual DbSet<nhanvien> nhanviens { get; set; }

    public virtual DbSet<phieudonphong> phieudonphongs { get; set; }

    public virtual DbSet<phong> phongs { get; set; }

    public virtual DbSet<quyen> quyens { get; set; }

    public virtual DbSet<taikhoan> taikhoans { get; set; }

    public virtual DbSet<thanhtoan> thanhtoans { get; set; }

    public virtual DbSet<traphong> traphongs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<chitietdatphong>(entity =>
        {
            entity.HasKey(e => e.machitietdatphong).HasName("chitietdatphong_pkey");

            entity.ToTable("chitietdatphong");

            entity.Property(e => e.dongiaphong).HasPrecision(12, 2);
            entity.Property(e => e.sotreem).HasDefaultValue(0);
            entity.Property(e => e.tiencoc).HasPrecision(12, 2);
            entity.Property(e => e.tienphuthu)
                .HasPrecision(12, 2)
                .HasDefaultValue(0m);
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Chờ xác nhận'::character varying");

            entity.HasOne(d => d.madatphongNavigation).WithMany(p => p.chitietdatphongs)
                .HasForeignKey(d => d.madatphong)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("chitietdatphong_madatphong_fkey");

            entity.HasOne(d => d.maphongNavigation).WithMany(p => p.chitietdatphongs)
                .HasForeignKey(d => d.maphong)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("chitietdatphong_maphong_fkey");
        });

        modelBuilder.Entity<chitietsudungdichvu>(entity =>
        {
            entity.HasKey(e => e.machitietdv).HasName("chitietsudungdichvu_pkey");

            entity.ToTable("chitietsudungdichvu");

            entity.Property(e => e.dongia).HasPrecision(12, 2);
            entity.Property(e => e.thanhtien).HasPrecision(12, 2);
            entity.Property(e => e.thoigiansudung)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.madvNavigation).WithMany(p => p.chitietsudungdichvus)
                .HasForeignKey(d => d.madv)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("chitietsudungdichvu_madv_fkey");

            entity.HasOne(d => d.manhanphongNavigation).WithMany(p => p.chitietsudungdichvus)
                .HasForeignKey(d => d.manhanphong)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("chitietsudungdichvu_manhanphong_fkey");

            entity.HasOne(d => d.manvNavigation).WithMany(p => p.chitietsudungdichvus)
                .HasForeignKey(d => d.manv)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("chitietsudungdichvu_manv_fkey");
        });

        modelBuilder.Entity<datphong>(entity =>
        {
            entity.HasKey(e => e.madatphong).HasName("datphong_pkey");

            entity.ToTable("datphong");

            entity.Property(e => e.hethanthanhtoan).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ngaytao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.tongtiencoc).HasPrecision(12, 2);
            entity.Property(e => e.tongtiendukien).HasPrecision(12, 2);
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Chờ thanh toán cọc'::character varying");

            entity.HasOne(d => d.makhNavigation).WithMany(p => p.datphongs)
                .HasForeignKey(d => d.makh)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("datphong_makh_fkey");
        });

        modelBuilder.Entity<dichvu>(entity =>
        {
            entity.HasKey(e => e.madv).HasName("dichvu_pkey");

            entity.ToTable("dichvu");

            entity.Property(e => e.dongia).HasPrecision(12, 2);
            entity.Property(e => e.donvitinh).HasMaxLength(20);
            entity.Property(e => e.loaidv).HasMaxLength(50);
            entity.Property(e => e.tendv).HasMaxLength(100);
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Đang phục vụ'::character varying");
        });

        modelBuilder.Entity<hoadon>(entity =>
        {
            entity.HasKey(e => e.mahoadon).HasName("hoadon_pkey");

            entity.ToTable("hoadon");

            entity.HasIndex(e => e.machitietdatphong, "hoadon_machitietdatphong_key").IsUnique();

            entity.Property(e => e.ngaylap)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.tiencocdatru).HasPrecision(12, 2);
            entity.Property(e => e.tiendichvu)
                .HasPrecision(12, 2)
                .HasDefaultValue(0m);
            entity.Property(e => e.tienphong).HasPrecision(12, 2);
            entity.Property(e => e.tienphuthu)
                .HasPrecision(12, 2)
                .HasDefaultValue(0m);
            entity.Property(e => e.tongtien).HasPrecision(12, 2);
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Chưa thanh toán'::character varying");

            entity.HasOne(d => d.machitietdatphongNavigation).WithOne(p => p.hoadon)
                .HasForeignKey<hoadon>(d => d.machitietdatphong)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("hoadon_machitietdatphong_fkey");

            entity.HasOne(d => d.manvNavigation).WithMany(p => p.hoadons)
                .HasForeignKey(d => d.manv)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("hoadon_manv_fkey");
        });

        modelBuilder.Entity<khachhang>(entity =>
        {
            entity.HasKey(e => e.makh).HasName("khachhang_pkey");

            entity.ToTable("khachhang");

            entity.HasIndex(e => e.sodienthoai, "khachhang_sodienthoai_key").IsUnique();

            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.gioitinh).HasMaxLength(10);
            entity.Property(e => e.hoten).HasMaxLength(100);
            entity.Property(e => e.sodienthoai).HasMaxLength(15);
        });

        modelBuilder.Entity<loaiphong>(entity =>
        {
            entity.HasKey(e => e.maloaiphong).HasName("loaiphong_pkey");

            entity.ToTable("loaiphong");

            entity.Property(e => e.giaphong).HasPrecision(12, 2);
            entity.Property(e => e.phuthunguoilon)
                .HasPrecision(12, 2)
                .HasDefaultValue(0m);
            entity.Property(e => e.phuthutreem)
                .HasPrecision(12, 2)
                .HasDefaultValue(0m);
            entity.Property(e => e.tenloaiphong).HasMaxLength(100);
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Hoạt động'::character varying");
        });

        modelBuilder.Entity<nhanphong>(entity =>
        {
            entity.HasKey(e => e.manhanphong).HasName("nhanphong_pkey");

            entity.ToTable("nhanphong");

            entity.HasIndex(e => e.machitietdatphong, "nhanphong_machitietdatphong_key").IsUnique();

            entity.Property(e => e.thoigiannhan)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.machitietdatphongNavigation).WithOne(p => p.nhanphong)
                .HasForeignKey<nhanphong>(d => d.machitietdatphong)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("nhanphong_machitietdatphong_fkey");

            entity.HasOne(d => d.manvNavigation).WithMany(p => p.nhanphongs)
                .HasForeignKey(d => d.manv)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("nhanphong_manv_fkey");
        });

        modelBuilder.Entity<nhanvien>(entity =>
        {
            entity.HasKey(e => e.manv).HasName("nhanvien_pkey");

            entity.ToTable("nhanvien");

            entity.HasIndex(e => e.sodienthoai, "nhanvien_sodienthoai_key").IsUnique();

            entity.Property(e => e.email).HasMaxLength(100);
            entity.Property(e => e.gioitinh).HasMaxLength(10);
            entity.Property(e => e.hoten).HasMaxLength(100);
            entity.Property(e => e.sodienthoai).HasMaxLength(15);
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Đang làm việc'::character varying");
        });

        modelBuilder.Entity<phieudonphong>(entity =>
        {
            entity.HasKey(e => e.madonphong).HasName("phieudonphong_pkey");

            entity.ToTable("phieudonphong");

            entity.HasIndex(e => e.matraphong, "phieudonphong_matraphong_key").IsUnique();

            entity.Property(e => e.thoigianbatdau).HasColumnType("timestamp without time zone");
            entity.Property(e => e.thoigianhoanthanh).HasColumnType("timestamp without time zone");
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Chờ dọn dẹp'::character varying");

            entity.HasOne(d => d.manvNavigation).WithMany(p => p.phieudonphongs)
                .HasForeignKey(d => d.manv)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("phieudonphong_manv_fkey");

            entity.HasOne(d => d.matraphongNavigation).WithOne(p => p.phieudonphong)
                .HasForeignKey<phieudonphong>(d => d.matraphong)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("phieudonphong_matraphong_fkey");
        });

        modelBuilder.Entity<phong>(entity =>
        {
            entity.HasKey(e => e.maphong).HasName("phong_pkey");

            entity.ToTable("phong");

            entity.HasIndex(e => e.tenphong, "phong_tenphong_key").IsUnique();

            entity.Property(e => e.tenphong).HasMaxLength(20);
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Trống'::character varying");

            entity.HasOne(d => d.maloaiphongNavigation).WithMany(p => p.phongs)
                .HasForeignKey(d => d.maloaiphong)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("phong_maloaiphong_fkey");
        });

        modelBuilder.Entity<quyen>(entity =>
        {
            entity.HasKey(e => e.maquyen).HasName("quyen_pkey");

            entity.ToTable("quyen");

            entity.Property(e => e.tenquyen).HasMaxLength(50);
        });

        modelBuilder.Entity<taikhoan>(entity =>
        {
            entity.HasKey(e => e.mataikhoan).HasName("taikhoan_pkey");

            entity.ToTable("taikhoan");

            entity.HasIndex(e => e.tendangnhap, "taikhoan_tendangnhap_key").IsUnique();

            entity.Property(e => e.tendangnhap).HasMaxLength(50);
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Hoạt động'::character varying");

            entity.HasOne(d => d.makhNavigation).WithMany(p => p.taikhoans)
                .HasForeignKey(d => d.makh)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("taikhoan_makh_fkey");

            entity.HasOne(d => d.manvNavigation).WithMany(p => p.taikhoans)
                .HasForeignKey(d => d.manv)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("taikhoan_manv_fkey");

            entity.HasOne(d => d.maquyenNavigation).WithMany(p => p.taikhoans)
                .HasForeignKey(d => d.maquyen)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("taikhoan_maquyen_fkey");
        });

        modelBuilder.Entity<thanhtoan>(entity =>
        {
            entity.HasKey(e => e.mathanhtoan).HasName("thanhtoan_pkey");

            entity.ToTable("thanhtoan");

            entity.Property(e => e.loaigiaodich).HasMaxLength(50);
            entity.Property(e => e.magiaodich).HasMaxLength(100);
            entity.Property(e => e.ngaythanhtoan)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.phuongthuc).HasMaxLength(50);
            entity.Property(e => e.sotien).HasPrecision(12, 2);
            entity.Property(e => e.trangthai)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Thành công'::character varying");

            entity.HasOne(d => d.machitietdatphongNavigation).WithMany(p => p.thanhtoans)
                .HasForeignKey(d => d.machitietdatphong)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("thanhtoan_machitietdatphong_fkey");

            entity.HasOne(d => d.mahoadonNavigation).WithMany(p => p.thanhtoans)
                .HasForeignKey(d => d.mahoadon)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("thanhtoan_mahoadon_fkey");
        });

        modelBuilder.Entity<traphong>(entity =>
        {
            entity.HasKey(e => e.matraphong).HasName("traphong_pkey");

            entity.ToTable("traphong");

            entity.HasIndex(e => e.machitietdatphong, "traphong_machitietdatphong_key").IsUnique();

            entity.Property(e => e.thoigiantra)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.machitietdatphongNavigation).WithOne(p => p.traphong)
                .HasForeignKey<traphong>(d => d.machitietdatphong)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("traphong_machitietdatphong_fkey");

            entity.HasOne(d => d.manvNavigation).WithMany(p => p.traphongs)
                .HasForeignKey(d => d.manv)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("traphong_manv_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
