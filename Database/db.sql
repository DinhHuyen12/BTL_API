CREATE DATABASE QuanLyThuVien;
use QuanLyThuVien
CREATE TABLE Books 
(
    BookId INT PRIMARY KEY IDENTITY(1,1),
    ISBN VARCHAR(20) UNIQUE NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    Category NVARCHAR(100),
    PublicationYear INT
);

-- Bookshelves
CREATE TABLE Bookshelves 
(
    BookshelfId INT PRIMARY KEY IDENTITY(1,1),
    ShelfCode VARCHAR(20) UNIQUE NOT NULL,
    Location NVARCHAR(100)
);
GO

-- Book copies
CREATE TABLE BookCopies 
(
    BookCopyId INT PRIMARY KEY IDENTITY(1,1),
    BookId INT NOT NULL,
    BookshelfId INT,
    Status NVARCHAR(20) NOT NULL DEFAULT N'Available',
    FOREIGN KEY (BookID) REFERENCES Books(BookID),
    FOREIGN KEY (BookshelfId) REFERENCES Bookshelves(BookshelfId),
    CONSTRAINT chk_Status CHECK (Status IN (N'Available', N'Borrowed', N'Reserved', N'Lost'))
);
GO

-- Readers
CREATE TABLE Readers 
(
    ReaderId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    CardId VARCHAR(20) UNIQUE NOT NULL,
    ExpirationDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT N'Active',
    CONSTRAINT chk_ReaderStatus CHECK (Status IN (N'Active', N'Inactive', N'Blocked'))
);
GO

-- Book loans
CREATE TABLE BookLoans 
(
    LoanId INT PRIMARY KEY IDENTITY(1,1),
    ReaderId INT NOT NULL,
    BookCopyId INT NOT NULL,
    BorrowDate DATE NOT NULL,
    DueDate DATE NOT NULL,
    ReturnDate DATE NULL,
    FOREIGN KEY (ReaderId) REFERENCES Readers(ReaderId),
    FOREIGN KEY (BookCopyId) REFERENCES BookCopies(BookCopyId),
    CONSTRAINT chk_DueDate CHECK (DueDate >= BorrowDate)
);
GO

-- Reservations
CREATE TABLE Reservations 
(
    ReservationId INT PRIMARY KEY IDENTITY(1,1),
    ReaderId INT NOT NULL,
    BookId INT NOT NULL,
    ReservationDate DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT N'Pending',
    FOREIGN KEY (ReaderId) REFERENCES Readers(ReaderId),
    FOREIGN KEY (BookId) REFERENCES Books(BookId),
    CONSTRAINT chk_ReservationStatus CHECK (Status IN (N'Pending', N'Collected', N'Cancelled'))
);
GO

-- Fines
CREATE TABLE Fines 
(
    FineId INT PRIMARY KEY IDENTITY(1,1),
    LoanId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    IsPaid BIT DEFAULT 0,
    FOREIGN KEY (LoanID) REFERENCES BookLoans(LoanID)
);
GO

-- Fine payments
CREATE TABLE FinePayments 
(
    FinePaymentId INT PRIMARY KEY IDENTITY(1,1),
    FineId INT NOT NULL,
    PaymentDate DATE NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (FineId) REFERENCES Fines(FineId)
);
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    FullName NVARCHAR(100),
    Email VARCHAR(100) UNIQUE,
    Status NVARCHAR(20) NOT NULL DEFAULT N'Active',
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT chk_user_status CHECK (Status IN (N'Active', N'Inactive', N'Locked'))
);
GO
ALTER TABLE Users
ADD RoleId INT NOT NULL DEFAULT 1;  -- default = 1 (user bình thường)

ALTER TABLE Users
ADD CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId);

CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) UNIQUE NOT NULL,
    Description NVARCHAR(200)
);

insert into Roles(RoleName)
VALUES('Admin'),('Librarian')

insert into Functions(FunctionName)
values(N'Quản lý người dùng'),(N'Quản lý thông tin sách')


CREATE TABLE Functions (
    FunctionId INT PRIMARY KEY IDENTITY(1,1),
    FunctionName NVARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE RolePermissions (
    RoleId INT NOT NULL,
    FunctionId INT NOT NULL,
	CanRead int,
	CanWrite INT,
	CanDelete INT,
	CanUpdate int,
    PRIMARY KEY (RoleId, FunctionId),
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
    FOREIGN KEY (FunctionId) REFERENCES Functions(FunctionId)
);

ALTER TABLE Users
ADD RoleId INT NOT NULL DEFAULT 1;  -- default = 1 (user bình thường)



-- 1. Thêm Sách
INSERT INTO Books (ISBN, Title, Author, Category, PublicationYear) VALUES
('978604210001', N'Lập trình C# cơ bản', N'Nguyễn Văn A', N'Công nghệ thông tin', 2020),
('978604210002', N'Cơ sở dữ liệu', N'Trần Thị B', N'Công nghệ thông tin', 2019),
('978604210003', N'Kinh tế vi mô', N'Lê Văn C', N'Kinh tế', 2018);

-- 2. Thêm Kệ sách
INSERT INTO Bookshelves (ShelfCode, Location) VALUES
('K001', N'Tầng 1 - CNTT'),
('K002', N'Tầng 2 - Kinh tế');

-- 3. Thêm Bạn đọc
INSERT INTO Readers (FullName, CardId, ExpirationDate, Status) VALUES
(N'Nguyễn Minh Hoàng', 'BD004', '2026-12-31', N'Active'),
(N'Lê Thị Thu', 'BD005', '2025-12-31', N'Active'),
(N'Trần Văn Nam', 'BD006', '2025-06-30', N'Active');

-- 4. Thêm Bản sao sách
INSERT INTO BookCopies (BookId, BookshelfId, Status) VALUES
(1, 1, N'Available'),
(1, 1, N'Available'),
(2, 1, N'Available'),
(3, 2, N'Available');

-- 5. Thêm Mượn trả
INSERT INTO BookLoans (ReaderId, BookCopyId, BorrowDate, DueDate, ReturnDate) VALUES
(1, 1, '2025-08-01', '2025-08-15', '2025-08-10'),  -- đã trả
(2, 2, '2025-08-05', '2025-08-20', NULL),          -- chưa trả
(3, 3, '2025-07-20', '2025-07-30', '2025-08-05');  -- trả trễ

-- 6. Thêm Đặt chỗ
INSERT INTO Reservations (ReaderId, BookId, ReservationDate, Status) VALUES
(1, 2, '2025-08-25', N'Pending'),
(2, 3, '2025-08-26', N'Collected');

-- 7. Thêm Phạt
INSERT INTO Fines (LoanId, Amount, IsPaid) VALUES
(3, 50000, 0),  -- chưa nộp
(2, 20000, 1);  -- đã nộp

-- 8. Thêm Thu phạt
INSERT INTO FinePayments (FineId, PaymentDate, Amount) VALUES
(2, '2025-08-26', 20000);





