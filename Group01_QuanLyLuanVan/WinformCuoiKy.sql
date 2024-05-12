--USE master;
--GO
--ALTER DATABASE QuanLyLuanVan SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
--GO
--DROP DATABASE QuanLyLuanVan
--GO

CREATE DATABASE QuanLyLuanVan
GO

USE QuanLyLuanVan
GO

--DROP TABLE TaiKhoan
create table TaiKhoan(
	username nvarchar(30) primary key,
	password nvarchar(30),
	mail nvarchar(100),
	quyen int,
	trangThai int,
	code nvarchar(30),
	avatar varchar(100)
)
go

insert into TaiKhoan(username, password, mail, quyen , trangThai, code, avatar) 
values
('nuoc', '1' , 'giangvien444@gmail.com', 2 , 1, '505000', '/Resource/Image/addava.png'),
('hue', '1' , 'giangvien444@gmail.com', 0 , 1, '505000', '/Resource/Image/addava.png'),
('admin', '1' , 'giangvien444@gmail.com', 0 , 1, '505000', '/Resource/Image/addava.png'),
('mai', '1' , 'giangvien444@gmail.com', 1 , 1, '505000', '/Resource/Image/addava.png'),
('hoa', '1' , 'giangvien444@gmail.com', 1 , 1, '505000', '/Resource/Image/addava.png'),
('lan', '1' , 'giangvien444@gmail.com', 1 , 1, '505000', '/Resource/Image/addava.png'),
('phap', '1' , 'giangvien444@gmail.com', 2 , 1, '505000', '/Resource/Image/addava.png'),
('huyen', '1' , 'giangvien444@gmail.com', 2 , 1, '505000', '/Resource/Image/addava.png'),
('hang', '1' , 'giangvien444@gmail.com', 2 , 1, '505000', '/Resource/Image/addava.png'),
('khang', '1' , 'giangvien444@gmail.com', 2 , 1, '505000', '/Resource/Image/addava.png'),
('anh', '1' , 'giangvien444@gmail.com', 2 , 1, '505000', '/Resource/Image/addava.png')
go
--DROP TABLE Khoa
create table Khoa (
	id integer identity(1,1) not null constraint Mk_ID unique,
	khoaId as right('K0' + cast(ID as varchar(10)),10) persisted constraint PK_Mk primary key clustered ,   
    tenKhoa nvarchar(100)
)
go


insert into Khoa(tenKhoa)
values (N'Công nghệ thông tin'),
( N'Kinh tế')
go


create table GiangVien(
	id integer identity(1,1) not null constraint Mgv_ID unique,
	giangVienId as right('GV0' + cast(ID as varchar(10)),10) persisted constraint PK_Mgv primary key clustered , 
	hoTen nvarchar(100),
	ngaySinh Datetime,
	gioiTinh nvarchar(10),
	diaChi nvarchar(100),
	email nvarchar(50),
	SDT nvarchar (10),
	khoaId varchar(10) references Khoa(khoaId),
	username nvarchar(30) references TaiKhoan(username)
)
go

insert into GiangVien(hoTen, ngaySinh, gioiTinh, diaChi, email, SDT, khoaId, username)
values 
('Thanh Hoa' , '1990-01-01' , N'Nữ' , N'Gia Lai' , 'giangvien@gmail.com', '1111432343', 'K01' , 'hoa'),
('Thanh Lan' , '1990-05-01' , N'Nữ' , N'Hà Nội' , 'giangvien444@gmail.com', '1110000343', 'K01' , 'lan'),
('Thanh Mai' , '1990-05-01' , N'Nữ' , N'Hà Nội' , 'giangvien444@gmail.com', '1110000343', 'K02' , 'mai')
go

--DROP TABLE Nhom
create table Nhom(	
	nhomId integer primary key
)
go


create table SinhVien(
	id integer not null constraint Msv_ID unique,
	sinhVienId as right('SV' + cast(ID as varchar(10)),10) persisted constraint PK_Msv primary key clustered , 
	hoTen nvarchar(100),
	ngaySinh Datetime,
	gioiTinh nvarchar(10),
	diaChi nvarchar(100),
	email nvarchar(50),
	SDT nvarchar (10),
	khoaId varchar(10) references Khoa(khoaId),
	username nvarchar(30) references TaiKhoan(username),
	nhomId integer references Nhom(nhomId)
)
go

insert into SinhVien(id, hoTen, ngaySinh, gioiTinh, diaChi, email, SDT, khoaId, username)
values(21110594, 'Thi Nuoc' , '1990-08-01' , N'Nữ' , N'	Quãng Ngãi' , 'sinhvien@gmail.com', '1111432343', 'K01' , 'nuoc'), 
(21110587, 'Van Khang' , '1990-08-01' , N'Nam' , N'	Quãng Ngãi' , 'sinhvien@gmail.com', '1111432343', 'K01' , 'khang') ,
(21110588,'Ngoc Anh' , '1990-07-01' , N'Nữ' , N'	Quãng Bình' , 'sinhvien555@gmail.com', '1111234563', 'K01' , 'anh'),
(21110589,'Van Phap' , '1990-07-01' , N'Nam' , N'	Quãng Bình' , 'sinhvien555@gmail.com', '1111234563', 'K01' , 'phap'),
(21110580,'Ngoc Huyen' , '1990-07-01' , N'Nữ' , N'	Quãng Bình' , 'sinhvien555@gmail.com', '1111234563', 'K01' , 'huyen'),
(21110581,'Ngoc Hang' , '1990-07-01' , N'Nữ' , N'	Quãng Bình' , 'sinhvien555@gmail.com', '1111234563', 'K01' , 'hang')



--DROP TABLE TheLoai
CREATE TABLE TheLoai (
	id integer identity(1,1) not null constraint Mtl_ID unique,
	theLoaiId as right('TL0' + cast(ID as varchar(10)),10) persisted constraint PK_Mtl primary key clustered , 
	tenTheLoai nvarchar(200),
	khoaId varchar(10) REFERENCES KHOA(khoaId)
)
GO

INSERT INTO TheLoai (tenTheLoai, khoaId)
VALUES
(N'Lập trình Web', 'K01'),
(N'Lập trình di động', 'K01'),
(N'Mạng và an ninh mạng', 'K01'),
(N'Trí tuệ nhân tạo', 'K01'),
(N'Kinh doanh quốc tế', 'K02'),
(N'Kế toán', 'K02');
GO


--DROP TABLE DeTai
CREATE TABLE DeTai (
	id integer identity(1,1) not null constraint Mdt_ID unique,
	deTaiId as right('DT0' + cast(ID as varchar(10)),10) persisted constraint PK_Mdt primary key clustered , 
	tenDeTai nvarchar(200),
	moTa nvarchar(200),
	yeuCauChung nvarchar(200),
	soLuong int,
	trangThai int,
	ngayBatDau Datetime,
	ngayKetThuc Datetime,
	theLoaiId varchar(10) REFERENCES THELOAI(theLoaiId),
	nhomId integer REFERENCES NHOM(nhomId),
	an integer,
	diem float,
	giangVienId varchar(10) REFERENCES GIANGVIEN(giangVienId)
)
GO

INSERT INTO DeTai (tenDeTai, moTa, yeuCauChung, soLuong, trangThai, ngayBatDau, ngayKetThuc, theLoaiId, giangVienId, an, diem)
VALUES
(N'Xây dựng ứng dụng hỗ trợ chấm công dựa trên nhận dạng khuôn mặt người – tại công ty tin học Hoài Ân', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL01', 'GV01', 0, 0),
(N'Dự báo qua hành vi mua sắm của người tiêu dùng trong hoạt động kinh doanh hàng tiêu dùng', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL01', 'GV01', 0, 0),
(N'Ứng dụng công nghệ bản đồ tự tổ chức (SOM – Self Organizing Map) nhằm phát hiện tấn công DoS qua hành vi', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL01', 'GV01', 0, 0),
(N'Song song hóa bài toán JSP trên môi trường tính toán song song và phân tán', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL01', 'GV01', 0, 0),
(N'Ứng dụng khai phá dữ liệu vào giải pháp hỗ trợ chẩn đoán và điều trị bệnh sốt xuất huyết', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL02', 'GV01', 0, 0),
(N'Giải pháp vượt qua sự ngăn chặn thu thập dữ liệu thương mại điện tử (An anti-anti-crawling solution )', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL03', 'GV02', 0, 0),
(N'Ứng dụng một số giải thuật data mining vào kết quả học tập Trung học cơ sở Chu Văn An', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL04', 'GV01', 0, 0),
(N'Xây dựng ứng dụng hỗ trợ chấm công dựa trên nhận dạng khuôn mặt người – tại công ty tin học Hoài Ân', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL04', 'GV02', 0, 0),
(N'Đánh giá nhu cầu khai thác thông tin khoa học công nghệ hỗ trợ định hướng sản xuất trên lĩnh vực nông nghiệp', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL03', 'GV03', 0, 0),
(N'Xây dựng chương trình phát hiện và rút trích văn bản từ hình ảnh trên thiết bị di động', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL05', 'GV01', 0, 0),
(N'Ứng dụng công nghệ Blockchain trong xác minh thông tin quá trình học tập của du học sinh Lào', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL06', 'GV03', 0, 0),
(N'Tối ưu lượng nước tưới trong quá trình sản xuất cây cao su giống tại tỉnh Champasak', N'Dùng những kiến thức đã học', N'Đã đọc qua sách', 2, 0, '2023-03-03', '2024-03-03', 'TL06', 'GV03', 0, 0);
GO


--DROP TABLE YeuCau
CREATE TABLE YeuCau (
    yeuCauId int IDENTITY(1,1) PRIMARY KEY,
    noiDung nvarchar(200),
    trangThai int,
	phanTram int,
    deTaiId varchar(10) REFERENCES DETAI(deTaiId)
);

UPDATE YeuCau SET trangThai =  10 FROM YeuCau where yeuCauId = 1

--trangThai: Đã hoàn thành, Chưa hoàn thành
--INSERT INTO YeuCau (noiDung, trangThai, deTaiId) VALUES (@noiDung, @trangThai, @deTaiId)
INSERT INTO YeuCau (noiDung, trangThai, deTaiId)
VALUES
(N'Thiết kế database test trạng thái2', 100, 'DT06'),
(N'Thiết kế giao diện2', '', 'DT06')
,

(N'Thiết kế database test trạng thái2', 100, 'DT06'),
(N'Thiết kế giao diện2', '', 'DT06')
,(N'Thiết kế database test trạng thái2', 100, 'DT06'),
(N'Thiết kế giao diện2', '', 'DT06')
,(N'Thiết kế database test trạng thái2', 100, 'DT06'),
(N'Thiết kế giao diện2', '', 'DT06')
,(N'Thiết kế database test trạng thái2', 100, 'DT06'),
(N'Thiết kế giao diện2', '', 'DT06')
,(N'Thiết kế database test trạng thái2', 100, 'DT06'),
(N'Thiết kế giao diện2', '', 'DT06')
,
(N'Code chức năng đăng ký, đăng nhập2', '', 'DT02'),
(N'Code chức năng chỉnh sửa thông tin2', '', 'DT02'),
(N'Code chức năng nộp bài2', '', 'DT02'),
(N'Code chức năng thông báo2', '', 'DT02'),
(N'Thiết kế database test trạng thái', 100, 'DT01'),
(N'Thiết kế giao diện', '', 'DT01'),
(N'Code chức năng đăng ký, đăng nhập', '', 'DT01'),
(N'Code chức năng chỉnh sửa thông tin', '', 'DT01'),
(N'Code chức năng nộp bài', '', 'DT01'),
(N'Code chức năng thông báo', '', 'DT01');
GO


--DROP TABLE YeuCau
CREATE TABLE TinNhanYeuCau (
    tinNhanId int IDENTITY(1,1) PRIMARY KEY,
    tinNhan nvarchar(200),
	thoiGian Datetime,
	username nvarchar(30) REFERENCES TaiKhoan(username),
    yeuCauId int REFERENCES YeuCau(yeuCauId)
);

INSERT INTO TinNhanYeuCau (tinNhan, thoiGian, username,yeuCauId)
VALUES
(N'Nội dung tin nhắn 1', '2023-03-03', 'phap', 1),
(N'Nội dung tin nhắn 2', '2023-03-03', 'phap', 1),
(N'Nội dung tin nhắn 3', '2023-03-03', 'phap', 1),
(N'Nội dung tin nhắn 1', '2023-03-03', 'phap', 7),
(N'Nội dung tin nhắn 2', '2023-03-03', 'phap', 7),
(N'Nội dung tin nhắn 3', '2023-03-03', 'phap', 7);
GO


--DROP TABLE DanhGia
CREATE TABLE DanhGia (	
	danhGiaId int IDENTITY(1,1) PRIMARY KEY,
	noiDung nvarchar(200),
	deTaiId varchar(10) REFERENCES DETAI(deTaiId),
	ngay date
);
GO

INSERT INTO DanhGia (noiDung, deTaiId,ngay)
VALUES
(N'Rất Hay', 'DT01', '2023-03-03');
GO


--DROP TABLE TienDo
CREATE TABLE TienDo (
	tienDoId int IDENTITY(1,1) PRIMARY KEY, 
	noiDung nvarchar(200),
	phanTram int,
	ngay date,
	deTaiId varchar(10) REFERENCES DETAI(deTaiId)
)
GO

INSERT INTO TienDo (noiDung, phanTram,deTaiId,ngay)
VALUES
(N'làm xong phần giao diện',10,'DT01','2023-03-03');
GO

--DROP TABLE TienDo
CREATE TABLE ThongBao (	
	thongBaoId int IDENTITY(1,1) PRIMARY KEY,
	tieude nvarchar(200),
	noiDung nvarchar(200),
	deTaiId varchar(10) REFERENCES DETAI(deTaiId),
	ngay date,
	trangthai int
)
GO


INSERT INTO ThongBao(tieude,noidung, deTaiId,ngay,trangthai)
VALUES
(N'ltwindow4' ,N'Chieu mai hop meet luc 12h, cac em nho chuan bi bai day du de thuyet trinh nhe' ,'DT01','2023-05-03',0),
(N'ltwindow1' ,N'Chieu mai hop meet luc 7h, cac em nho chuan bi bai day du de thuyet trinh nhe, thực hiện tất cả bải tập được giao' ,'DT01','2023-03-03',1),
(N'ltwindow2' ,N'Chieu mai hop meet luc 8h, cac em nho chuan bi bai day du de thuyet trinh nhe' ,'DT01','2023-05-03',1),
(N'ltwindow3' ,N'Chieu mai hop meet luc 9h, cac em nho chuan bi bai day du de thuyet trinh nhe' ,'DT01','2023-05-03',1),
(N'ltwindow4' ,N'Chieu mai hop meet luc 10h, cac em nho chuan bi bai day du de thuyet trinh nhe' ,'DT01','2023-05-03',1)
GO
use QuanLyLuanVan

UPDATE ThongBao SET trangthai = 0 WHERE deTaiId IN (SELECT deTaiId FROM DeTai WHERE nhomId = 1) AND noiDung = 'a'

--SELECT thongBaoId, tieude, noiDung, ThongBao.deTaiId, ngay  FROM ThongBao INNER JOIN DeTai ON ThongBao.deTaiId = DeTai.deTaiId WHERE DeTai.deTaiId = 'DT01'
----select * from TaiKhoan
----select * from giangvien
----select * from sinhvien
----select * from khoa
--select * from detai
--select * from sinhvien
--select * from danhgia
--select * from tiendo
--select * from theloai


--go
--select * from YeuCau



--SELECT yc.yeuCauId, yc.noiDung, yc.trangThai,yc.deTaiId
--FROM YeuCau yc
--JOIN DeTai dt ON yc.deTaiId = dt.deTaiId
--JOIN SinhVien sv on sv.nhomId = dt.nhomId
--WHERE sv.username = 'phap';

--UPDATE YeuCau SET trangThai = 44 FROM YeuCau yc JOIN DeTai dt ON yc.deTaiId = dt.deTaiId JOIN SinhVien sv ON sv.nhomId = dt.nhomId WHERE sv.username = 'phap' and noiDung = 'a';


--SELECT SV.sinhVienId, SV.hoTen, K.tenKhoa FROM SinhVien SV INNER JOIN DeTai DT ON SV.nhomId = DT.nhomId INNER JOIN Khoa K ON SV.khoaId = K.khoaId WHERE DT.trangThai = 3;

--SELECT DISTINCT SV.sinhVienId, SV.hoTen, SV.email
--FROM SinhVien SV
--JOIN Nhom NH ON SV.nhomId = NH.nhomId
--JOIN DeTai DT ON NH.nhomId = DT.nhomId
--WHERE DT.trangThai = 3;

--SELECT DISTINCT SV.* FROM SinhVien SV JOIN Nhom NH ON SV.nhomId = NH.nhomId JOIN DeTai DT ON NH.nhomId = DT.nhomId WHERE DT.trangThai = 3;

--SELECT gv.giangVienId, gv.hoTen, COUNT(dt.nhomId) AS SoLuongNhom FROM GiangVien gv LEFT JOIN DeTai dt ON gv.giangVienId = dt.giangVienId AND dt.trangThai = 1 GROUP BY gv.giangVienId, gv.hoTen HAVING COUNT(dt.nhomId) > 2

--SELECT gv.giangVienId, gv.hoTen, COUNT(dt.nhomId) AS SoLuongNhom FROM GiangVien gv LEFT JOIN DeTai dt ON gv.giangVienId = dt.giangVienId GROUP BY gv.giangVienId, gv.hoTen HAVING COUNT(dt.nhomId) > 2


--SELECT gv.giangVienId, gv.hoTen, COUNT(dt.nhomId) AS SoLuongNhom FROM GiangVien gv LEFT JOIN DeTai dt ON gv.giangVienId = dt.giangVienId GROUP BY gv.giangVienId, gv.hoTen