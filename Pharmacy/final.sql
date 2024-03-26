USE master
GO
IF exists( SELECT * FROM sysdatabases WHERE name='Pharmacy')
	DROP DATABASE Pharmacy
GO
CREATE DATABASE Pharmacy
GO
USE Pharmacy
GO
CREATE TABLE News
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[tittle] nvarchar(50) NULL,
	[summary] nvarchar(100) NULL,
	[content] nvarchar(MAX) NULL,
	[img] nvarchar(50) NULL,
	[meta] nvarchar(MAX) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL
)
GO
INSERT News ([tittle], [summary], [content], [img], [meta], [hide], [order],[datebegin]) VALUES 
(N'Giao hàng nhanh chóng, miễn phí', N'Bạn đang cần thuốc nhưng ngại ra đường. Đừng lo, sản phẩm sẽ được giao tận nơi', N'Nội dung chính 1', 'bg_1.jpg', 'giao-hang-nhanh-chong-mien-phi', 1, 1, '2023-03-03'),
(N'Mùa giảm giá lên tới 50%', N'Từ 01/03/2023 đến 03/07/2023 Giảm giá một số sản phẩm sau', 'Nội dung chính 2', 'bg_1.jpg', 'mua-giam-gia-len-toi-50', 1, 2, '2023-03-03'),
(N'Sở hữu thẻ quà tặng', N'Khách hàng sẽ nhận được nhiều lợi ích hơn với thẻ quà tặng của chúng tôi', 'Nội dung chính 3', 'bg_1.jpg','so-huu-the-qua-tang', 1, 3, '2023-03-03'),
(N'Hội viên & Quyền lợi', N'Hơn 1000 khách hàng đã trở thành Hội viên của Pharmacy và nhận', 'Nội dung chính 4', 'bg_1.jpg', 'hoi-vien-&-quyen-loi', 1, 4, '2023/02/28')


GO
CREATE TABLE Category
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[name] nvarchar(50) NULL,
	[meta] nvarchar(50) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL
)
GO
INSERT Category ([name], [meta], [hide], [order], [datebegin]) VALUES (N'Dược phẩm','duoc-pham',1,1,'2023-03-03'),
(N'Thực phẩm chức năng','thuc-pham-chuc-nang',1,2,'2023-03-03'),
(N'Chăm sóc sức khoẻ','cham-soc-suc-khoe',1,3,'2023-03-03'),
(N'Chăm sóc cá nhân','cham-soc-ca-nhan',1,4,'2023-03-03'),
(N'Thiết bị y tế','thiet-bi-y-te',1,4,'2023-03-03')


GO
CREATE TABLE Product
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[name] nvarchar(50) NULL,
	[img] nvarchar(50) NULL,
	[price] int NULL,
	[description] nvarchar(max) NULL,
	[quantity] int NULL,
	[purchase] int NULL,-- lượt mua 
	[isSale] bit NULL,
	[priceSale] int NULL,
	[meta] nvarchar(max) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL,
	[id_category] int NOT NULL
	CONSTRAINT Product_idCategory_fk FOREIGN KEY (id_category) REFERENCES Category(id)
)
GO
INSERT Product ([name], [img], [price], [description], [quantity], [purchase], [isSale], [priceSale], [meta], [hide], [order], [datebegin], [id_category]) VALUES 
(N'Dược phẩm trị xương khớp', 'product_01.png', 100000, N'Mô tả 1', 1000, 50, 1 ,60000, 'duoc-pham-tri-xuong-khop', 1, 1, '2023-03-03', 1),
(N'Dược phẩm trị ho', 'product_02.png', 200000, N'Mô tả 2', 1000, 500, 0, 0, 'duoc-pham-tri-ho', 1, 2, '2023-03-03', 1),
(N'Thực phẩm chức năng tăng cân', 'product_03.png', 300000, N'Mô tả 3', 1000, 700, 0, 0, 'thuc-pham-chuc-nang-tang-can', 1, 3, '2023-03-18', 2),
(N'Thực phẩm chức năng sáng mắt', 'product_04.png', 400000, N'Mô tả 4', 1000, 500, 0, 0, 'thuc-pham-chuc-nang-sang-mat', 1, 4, '2023-03-11', 2),
(N'Chăm sóc sức khoẻ phục hồi tóc hư tổn', 'product_06.png', 600000, N'Mô tả 5', 1000, 600, 1, 20000, 'cham-soc-suc-khoe-phuc-hoi-toc-hu-ton', 1, 6, '2023-03-10', 3),
(N'Chăm sóc cá nhân băng y tế', 'product_01.png', 700000, N'Mô tả 6', 1000, 500, 0, 0, 'cham-soc-ca-nhan-bang-y-te', 1, 7, '2023-03-19', 4),
(N'Chăm sóc cá nhân sữa rửa mặt', 'product_02.png', 800000, N'Mô tả 7', 1000, 200, 0,0, 'cham-soc-ca-nhan-sua-rua-mat', 1, 8, '2023-03-03', 4),
(N'Thiết bị y tế Kit Test Covid', 'product_03.png', 900000, N'Mô tả 8', 1000, 100, 0, 0, 'thiet-bi-y-te-kit-test-covid', 1, 9, '2023-03-03', 5),
(N'Thiết bị y tế nhiệt kế', 'product_04.png', 1000000, N'Mô tả 9', 1000, 500, 1,60000, 'thiet-bi-y-te-nhiet-ke', 1, 10, '2023-03-03', 5)

---------------------------------------------- Trang chủ --------------------------------------------

GO
CREATE TABLE Menu
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[name] nvarchar(50) NULL,
	[meta] nvarchar(MAX) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL
)
GO
INSERT Menu ([name], [meta], [hide], [order], [datebegin]) VALUES 
(N'Trang chủ','trang-chu',1,1,'2023-03-03'),
(N'Sản Phẩm','san-pham',1,2,'2023-03-03'),
(N'Tin tức','tin-tuc',1,3,'2023-03-03'),
(N'Hệ thống nhà thuốc','thong-tin-nha-thuoc',1,4,'2023-03-03'),
(N'Liên hệ','lien-he',1,5,'2023-03-03')


GO
CREATE TABLE Banner
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[tittle] nvarchar(50) NULL,
	[content] nvarchar(MAX) NULL,
	[page] nvarchar(50) NULL,
	[img] nvarchar(50) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL
)
GO
INSERT Banner ([tittle], [content], [page], [img], [hide], [order], [datebegin]) VALUES 
('Welcome To Pharma', N'Thuốc Hiệu Quả, Thuốc Mới Mỗi Ngày.','HOME','hero_1.jpg',1,1,'2023/03/04'),
('About Us', N'Tại Pharmacy, mỗi Dược sĩ luôn hết lòng phục vụ, rèn luyện để hoàn thành xuất sắc nhiệm vụ được giao.','ABOUT','hero_1.jpg',1,1,'2023/03/04'),
('Blog', N'Cập nhật những tin tức mới nhất từ Pharmacy','NEWS','hero_1.jpg',1,1,'2023/03/04')


GO
CREATE TABLE QuickLink
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[name] nvarchar(50) NULL,
	[link] nvarchar(MAX) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL
)
GO
INSERT QuickLink ([name], [link], [hide], [order], [datebegin]) VALUES
(N'Dược phẩm','san-pham/duoc-pham',1,1,'2023-03-03'),
(N'Thực phẩm chức năng','san-pham/thuc-pham-chuc-nang',1,2,'2023-03-03'),
(N'Chăm sóc sức khoẻ','san-pham/cham-soc-suc-khoe',1,3,'2023-03-03'),
(N'Chăm sóc cá nhân','san-pham/cham-soc-ca-nhan',1,4,'2023-03-03'),
(N'Thiết bị y tế','san-pham/thiet-bi-y-te',1,5,'2023-03-03'),
(N'Chính sách giao hàng','tin-tuc/chinh-sach-giao-hang',1,6,'2023-03-03'),
(N'Chính sách đổi trả','tin-tuc/chinh-sach-doi-tra',1,7,'2023-03-03'),
(N'Chi nhánh Pharmacy','thong-tin-nha-thuoc#chi-nhanh',1,8,'2023-03-03')


GO
CREATE TABLE ContactInfo
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[content] nvarchar(MAX) NULL,
	[type] nvarchar(50) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL
)
GO
INSERT ContactInfo ([content], [type], [hide], [order], [datebegin]) VALUES 
(N'19 Nguyễn Hữu Thọ, Tân Hưng, Quận 7, Thành phố Hồ Chí Minh','address',1,1,'2023-03-03'),
('1800 1911','phone',1,2,'2023-03-03'),
('pharmacy.contact@gmail.com','email',1,3,'2023-03-03')

-----------------------------------------THÔNG TIN NHÀ THUỐC -------------------------------------

GO
CREATE TABLE CompanyInfo
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[tittle] nvarchar(50) NULL,
	[content] nvarchar(MAX) NULL,
	[img] nvarchar(50) NULL,
	[link] nvarchar(MAX) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL
)
GO
INSERT CompanyInfo ([tittle], [content], [img], [hide], [order], [datebegin]) VALUES 
('How We Started',N'Được thành lập vào năm 2023, Pharmacy là một trong những chuỗi bán lẻ dược phẩm đầu tiên tại Việt Nam. Đến nay, Nhà thuốc sở hữu mạng lưới hơn 200 nhà thuốc đạt GPP trên toàn quốc cùng đội ngũ hơn 2000 trình dược viên uy tín, cung cấp thuốc và sản phẩm chăm sóc sức khỏe hàng đầu với giá cạnh tranh nhất.','bg_1.jpg',1,1,'2023-03-03'),
('We Are Trusted Company',N'Nhà thuốc luôn hướng tới mục tiêu nâng cao chất lượng chăm sóc sức khỏe cho từng khách hàng. Điều này trước đây chỉ nằm trong ý tưởng của DS.An – người sáng lập công ty, một dược sĩ đã làm việc nhiều năm tại Việt Nam. Bằng niềm đam mê và sự sáng tạo của mình, DS.An đã thành lập Nhà thuốc và mang đến những trải nghiệm tốt nhất cho khách hàng.','hero_1.jpg',1,2,'2023-03-03')


GO
CREATE TABLE TeamInfo
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[name] nvarchar(50) NULL,
	[role] nvarchar(50) NULL,
	[description] nvarchar(max) NULL,
	[img] nvarchar(50) NULL,
	[link] nvarchar(max) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL
)
GO
INSERT TeamInfo ([name], [role], [description], [img], [hide], [order], [datebegin]) VALUES 
(N'Lý Tuấn An',N'CEO/CO-FOUNDER',N'Anh sinh năm 2002 tại Thành phố Hồ Chí Minh, Việt Nam và tốt nghiệp ngành công nghệ phần mềm tại Đại học Tôn Đức Thắng năm 2024.','person_2.jpg',1,1,'2023-03-03'),
(N'Prean Mesa',N'CO-FOUNDER',N'Anh sinh năm 2002 tại Campuchia và tốt nghiệp ngành công nghệ phần mềm tại Đại học Tôn Đức Thắng năm 2024.','person_3.jpg',1,2,'2023-03-03'),
(N'Huỳnh Công Chánh',N'CO-FOUNDER',N'Anh sinh năm 2001 tại Trà Vinh, Việt Nam và tốt nghiệp ngành công nghệ phần mềm tại Đại học Tôn Đức Thắng năm 2023.','person_4.jpg',1,3,'2023-03-03')


GO
CREATE TABLE Office
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[location] nvarchar(50) NULL,
	[address] nvarchar(150) NULL,
	[img] nvarchar(50) NULL,
	[link] nvarchar(MAX) NULL,
	[hide] bit NULL,
	[order] int NULL,
	[datebegin] smalldatetime NULL
)
GO
INSERT Office ([location], [address], [img], [link], [hide], [order], [datebegin]) VALUES 
(N'Quận 8',N'175 Cao Xuân Dục, Phường 12, Quận 8, Thành phố Hồ Chí Minh','bg_1.jpg','https://GOo.gl/maps/GVhs5ET7Bm3FXCzB9',1,1,'2023-03-03'),
(N'Quận 7',N'139 Tôn Dật Tiên, Phường Tân Phong, Quận 7, Thành phố Hồ Chí Minh','bg_1.jpg','https://GOo.gl/maps/CuqFiAMUm9CDfHbNA',1,2,'2023-03-03'),
(N'Quận 1',N'22-36 Nguyễn Huệ, Phường Bến Nghé, Quận 1, Thành phố Hồ Chí Minh','bg_1.jpg','https://GOo.gl/maps/L9coFpmgT9JJMGGa6',1,3,'2023-03-03')


----------------------------------------- ACCOUNT -------------------------------------

GO
CREATE TABLE Account
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[email] nvarchar(150) NOT NULL,
	[password] nvarchar(150) NOT NULL,
	[role] int NOT NULL,
	[permission] int NOT NULL
)
GO
INSERT Account ([email], [password], [role], [permission]) VALUES
('admin@gmail.com', '123123', '1', '2'),
('user@gmail.com', '123123', '0', '1')


GO
CREATE TABLE Orders
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[name] nvarchar(100) NOT NULL,
	[phone] nvarchar(20) NOT NULL,
	[address] nvarchar(150) NOT NULL,
	[ward] nvarchar(50) NULL,
	[district] nvarchar(50) NULL,
	[city] nvarchar(50) NULL,
	[datecreate] smalldatetime NULL,
	[total_amount] int NOT NULL,
	[status] int NOT NULL,
	[id_account] int NOT NULL
	CONSTRAINT Orders_idAccount_fk FOREIGN KEY (id_account) REFERENCES Account(id)
)
INSERT ORDERS ([name], [phone], [address], [ward], [district], [city], [datecreate], [total_amount], [status], [id_account]) VALUES
(N'Nguyễn Văn A', '0987654321', N'123 Nguyễn Hữu Thọ', N'Tân Hưng', '7', N'Hồ Chí Minh', '2023-05-01', 440000, 0, 2),
(N'Nguyễn Văn B', '0969696969', N'169/87 Phan Văn Trị', N'3', N'Gò Vấp', N'Hồ Chí Minh', '2023-05-02', 1280000, 0, 2)


GO
CREATE TABLE OrdersDetail
(
	[id] int IDENTITY(1,1) PRIMARY KEY,
	[quantity] int NOT NULL,
	[price] int NOT NULL,
	[amount] int NOT NULL,
	[id_orders] int NOT NULL,
	[id_product] int NOT NULL
	CONSTRAINT OrdersDetail_idOrders_fk FOREIGN KEY (id_orders) REFERENCES Orders(id),
	CONSTRAINT OrdersDetail_idProduct_fk FOREIGN KEY (id_product) REFERENCES Product(id)
)
INSERT OrdersDetail ([id_orders], [id_product], [quantity], [price], [amount]) VALUES
(1, 1, 1, 40000, 40000),
(1, 2, 2, 200000, 400000),
(2, 3, 1, 300000, 300000),
(2, 4, 1, 400000, 400000),
(2, 5, 1, 580000, 580000)