-- =============================================================================
-- 01_migration_chong_trung_lich.sql        (TV3 - DBA)
-- Yêu cầu: đã chạy script tạo 15 bảng (DatPhongKhachSan.sql) - PostgreSQL >= 12
-- Ghi chú: file KHÔNG chứa BEGIN/COMMIT để Alembic (TV2) tự bọc transaction.
--          Hàm cố ý không dùng ký tự phần trăm / hai chấm kép để chạy an toàn qua op.execute().
-- =============================================================================

-- 1. EXTENSION: btree_gist cho phép dùng toán tử "=" (kiểu int) trong chỉ mục GiST
CREATE EXTENSION IF NOT EXISTS btree_gist;

-- 2. LƯU KHOẢNG THỜI GIAN ĐẶT PHÒNG
-- Cột sinh tự động kiểu daterange, dạng [nhận, trả) : đóng đầu - mở cuối.
-- => khách A trả phòng 05/12 và khách B nhận phòng 05/12 KHÔNG bị coi là trùng.
ALTER TABLE ChiTietDatPhong
    ADD COLUMN IF NOT EXISTS KhoangNgay daterange
    GENERATED ALWAYS AS (daterange(NgayNhanDuKien, NgayTraDuKien, '[)')) STORED;

-- 3. RÀNG BUỘC TOÀN VẸN BỔ TRỢ
ALTER TABLE ChiTietDatPhong ALTER COLUMN MaPhong SET NOT NULL;      -- NULL sẽ "lách" được exclusion
ALTER TABLE ChiTietDatPhong ALTER COLUMN MaDatPhong SET NOT NULL;

ALTER TABLE ChiTietDatPhong
    ADD CONSTRAINT chk_ctdp_ngay CHECK (NgayTraDuKien > NgayNhanDuKien);

-- Trạng thái chi tiết đặt phòng:
--   GIỮ LỊCH      : 'Chờ xác nhận', 'Đã xác nhận', 'Đã nhận phòng'
--   KHÔNG GIỮ LỊCH: 'Đã trả phòng', 'Đã hủy', 'Hết hạn'
ALTER TABLE ChiTietDatPhong
    ADD CONSTRAINT chk_ctdp_trangthai CHECK (TrangThai IN
        ('Chờ xác nhận','Đã xác nhận','Đã nhận phòng','Đã trả phòng','Đã hủy','Hết hạn'));

ALTER TABLE DatPhong
    ADD CONSTRAINT chk_dp_trangthai CHECK (TrangThai IN
        ('Chờ thanh toán cọc','Đã xác nhận','Hoàn tất','Đã hủy','Hết hạn thanh toán'));

-- 4. EXCLUSION CONSTRAINT (tạo kèm chỉ mục GiST)
-- Hai dòng "đang giữ lịch" cùng MaPhong không được có KhoangNgay giao nhau (&&).
-- Mệnh đề WHERE => chỉ mục partial: dòng đã hủy/hết hạn/đã trả tự động rời khỏi chỉ mục
-- => lịch phòng được giải phóng ngay khi đổi trạng thái, không cần xóa dữ liệu.
ALTER TABLE ChiTietDatPhong
    ADD CONSTRAINT excl_phong_trung_lich
    EXCLUDE USING gist (MaPhong WITH =, KhoangNgay WITH &&)
    WHERE (TrangThai IN ('Chờ xác nhận','Đã xác nhận','Đã nhận phòng'));

-- Chỉ mục phụ cho tra cứu theo đơn / theo trạng thái
CREATE INDEX IF NOT EXISTS idx_ctdp_madatphong ON ChiTietDatPhong (MaDatPhong);
CREATE INDEX IF NOT EXISTS idx_dp_cho_coc_hethan
    ON DatPhong (HetHanThanhToan) WHERE TrangThai = 'Chờ thanh toán cọc';

-- 5. TRIGGER: đơn bị hủy / hết hạn => các chi tiết đang giữ lịch được nhả ngay
CREATE OR REPLACE FUNCTION fn_dongbo_trangthai_chitiet() RETURNS trigger
LANGUAGE plpgsql AS $$
BEGIN
    IF NEW.TrangThai IN ('Đã hủy','Hết hạn thanh toán') THEN
        UPDATE ChiTietDatPhong
           SET TrangThai = CASE WHEN NEW.TrangThai = 'Đã hủy' THEN 'Đã hủy' ELSE 'Hết hạn' END
         WHERE MaDatPhong = NEW.MaDatPhong
           AND TrangThai IN ('Chờ xác nhận','Đã xác nhận');   -- không đụng phòng đã nhận
    END IF;
    RETURN NEW;
END $$;

DROP TRIGGER IF EXISTS trg_dongbo_trangthai_chitiet ON DatPhong;
CREATE TRIGGER trg_dongbo_trangthai_chitiet
    AFTER UPDATE OF TrangThai ON DatPhong
    FOR EACH ROW
    WHEN (OLD.TrangThai IS DISTINCT FROM NEW.TrangThai)
    EXECUTE FUNCTION fn_dongbo_trangthai_chitiet();

-- 6. HÀM NGHIỆP VỤ ------------------------------------------------------------

-- 6.1 Kiểm tra phòng còn trống trong khoảng ngày (chỉ để hiển thị; chốt chặn thật là constraint)
CREATE OR REPLACE FUNCTION fn_phong_con_trong(p_maphong INT, p_nhan DATE, p_tra DATE)
RETURNS BOOLEAN LANGUAGE sql STABLE AS $$
    SELECT NOT EXISTS (
        SELECT 1 FROM ChiTietDatPhong
         WHERE MaPhong = p_maphong
           AND TrangThai IN ('Chờ xác nhận','Đã xác nhận','Đã nhận phòng')
           AND KhoangNgay && daterange(p_nhan, p_tra, '[)')
    );
$$;

-- 6.2 Tạo đơn đặt 1 phòng (đơn + chi tiết trong cùng 1 giao dịch)
CREATE OR REPLACE FUNCTION fn_tao_dat_phong(
    p_makh INT, p_maphong INT, p_nhan DATE, p_tra DATE,
    p_nguoi_lon INT DEFAULT 2, p_tre_em INT DEFAULT 0, p_phut_het_han INT DEFAULT 120)
RETURNS INT LANGUAGE plpgsql AS $$
DECLARE
    v_gia NUMERIC(12,2);  v_std INT;
    v_pt_nl NUMERIC(12,2); v_pt_te NUMERIC(12,2);
    v_dem INT;  v_phuthu_dem NUMERIC(12,2);
    v_tong NUMERIC(12,2); v_coc NUMERIC(12,2); v_madp INT;
BEGIN
    IF p_tra <= p_nhan THEN
        RAISE EXCEPTION 'NGAY_KHONG_HOP_LE: ngay tra phai sau ngay nhan';
    END IF;

    SELECT lp.GiaPhong, lp.SoNguoiLonTieuChuan,
           COALESCE(lp.PhuThuNguoiLon,0), COALESCE(lp.PhuThuTreEm,0)
      INTO v_gia, v_std, v_pt_nl, v_pt_te
      FROM Phong p JOIN LoaiPhong lp ON lp.MaLoaiPhong = p.MaLoaiPhong
     WHERE p.MaPhong = p_maphong;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'PHONG_KHONG_TON_TAI: ma phong khong hop le';
    END IF;

    v_dem        := p_tra - p_nhan;
    v_phuthu_dem := GREATEST(p_nguoi_lon - v_std, 0) * v_pt_nl + p_tre_em * v_pt_te;
    v_tong       := v_dem * (v_gia + v_phuthu_dem);
    v_coc        := ROUND(v_tong * 0.2, 0);                 -- cọc 20%

    INSERT INTO DatPhong (MaKH, HetHanThanhToan, TongTienDuKien, TongTienCoc, TrangThai)
    VALUES (p_makh, LOCALTIMESTAMP + make_interval(mins => p_phut_het_han),
            v_tong, v_coc, 'Chờ thanh toán cọc')
    RETURNING MaDatPhong INTO v_madp;

    BEGIN
        INSERT INTO ChiTietDatPhong (MaDatPhong, MaPhong, NgayNhanDuKien, NgayTraDuKien,
                                     SoNguoiLon, SoTreEm, DonGiaPhong, TienPhuThu, TienCoc, TrangThai)
        VALUES (v_madp, p_maphong, p_nhan, p_tra, p_nguoi_lon, p_tre_em,
                v_gia, v_phuthu_dem * v_dem, v_coc, 'Chờ xác nhận');
    EXCEPTION WHEN exclusion_violation THEN
        RAISE EXCEPTION 'PHONG_DA_BI_DAT: phong da co lich trong khoang ngay nay';
    END;

    RETURN v_madp;
END $$;

-- 6.3 Xác nhận đã nhận cọc (khóa dòng đơn để không đua với job hết hạn)
CREATE OR REPLACE FUNCTION fn_xac_nhan_coc(p_madatphong INT) RETURNS VOID
LANGUAGE plpgsql AS $$
DECLARE v_tt VARCHAR(50); v_han TIMESTAMP;
BEGIN
    SELECT TrangThai, HetHanThanhToan INTO v_tt, v_han
      FROM DatPhong WHERE MaDatPhong = p_madatphong FOR UPDATE;
    IF NOT FOUND THEN
        RAISE EXCEPTION 'DON_KHONG_TON_TAI: khong tim thay don';
    END IF;
    IF v_tt <> 'Chờ thanh toán cọc' THEN
        RAISE EXCEPTION 'DON_KHONG_CON_CHO_THANH_TOAN: don khong o trang thai cho coc';
    END IF;
    IF v_han < LOCALTIMESTAMP THEN
        RAISE EXCEPTION 'DON_DA_HET_HAN: qua han thanh toan coc';
    END IF;

    UPDATE DatPhong SET TrangThai = 'Đã xác nhận' WHERE MaDatPhong = p_madatphong;
    UPDATE ChiTietDatPhong SET TrangThai = 'Đã xác nhận'
     WHERE MaDatPhong = p_madatphong AND TrangThai = 'Chờ xác nhận';
END $$;

-- 6.4 Hủy đơn => trigger tự nhả lịch phòng
CREATE OR REPLACE FUNCTION fn_huy_dat_phong(p_madatphong INT, p_lydo TEXT) RETURNS BOOLEAN
LANGUAGE plpgsql AS $$
DECLARE v_n INT;
BEGIN
    UPDATE DatPhong SET TrangThai = 'Đã hủy', LyDoHuy = p_lydo
     WHERE MaDatPhong = p_madatphong
       AND TrangThai IN ('Chờ thanh toán cọc','Đã xác nhận');
    GET DIAGNOSTICS v_n = ROW_COUNT;
    RETURN v_n > 0;
END $$;

-- 6.5 Giải phóng các đơn quá hạn thanh toán cọc. Trả về số đơn đã xử lý.
-- Được gọi bởi worker Redis (khi key hold hết TTL) và bởi job định kỳ dự phòng.
CREATE OR REPLACE FUNCTION fn_giai_phong_don_het_han() RETURNS INT
LANGUAGE plpgsql AS $$
DECLARE v_n INT;
BEGIN
    UPDATE DatPhong
       SET TrangThai = 'Hết hạn thanh toán', LyDoHuy = 'Qua han thanh toan coc'
     WHERE TrangThai = 'Chờ thanh toán cọc'
       AND HetHanThanhToan < LOCALTIMESTAMP;
    GET DIAGNOSTICS v_n = ROW_COUNT;
    RETURN v_n;
END $$;

-- 7. XỬ LÝ LỖI VI PHẠM RÀNG BUỘC (tham khảo cho tầng ứng dụng)
--   SQLSTATE 23P01 exclusion_violation  -> trùng lịch phòng   -> báo "phòng đã được đặt"
--   SQLSTATE 23514 check_violation      -> sai ngày/trạng thái -> báo lỗi dữ liệu
--   SQLSTATE 23503 foreign_key_violation-> khách/phòng không tồn tại
--   Thông báo bắt đầu bằng PHONG_DA_BI_DAT / DON_DA_HET_HAN ... do các hàm ở mục 6 ném ra.

-- =============================================================================
-- ROLLBACK THỦ CÔNG (dùng cho downgrade)
-- =============================================================================
-- DROP TRIGGER IF EXISTS trg_dongbo_trangthai_chitiet ON DatPhong;
-- DROP FUNCTION IF EXISTS fn_giai_phong_don_het_han(), fn_huy_dat_phong(INT,TEXT),
--      fn_xac_nhan_coc(INT), fn_tao_dat_phong(INT,INT,DATE,DATE,INT,INT,INT),
--      fn_phong_con_trong(INT,DATE,DATE), fn_dongbo_trangthai_chitiet();
-- ALTER TABLE ChiTietDatPhong DROP CONSTRAINT excl_phong_trung_lich;
-- ALTER TABLE ChiTietDatPhong DROP COLUMN KhoangNgay;
