-- ============================================
--   БАЗА ДАНИХ: LibrarySystem
--   Автор: Максим Гончар
--   Опис: Ініціалізація таблиць і тестових даних
--   СУБД: Microsoft SQL Server
-- ============================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LibrarySystem')
BEGIN
    CREATE DATABASE LibrarySystem;
END
GO

USE LibrarySystem;
GO

-- Видалення таблиць, якщо існують
IF OBJECT_ID('dbo.BorrowRequests', 'U') IS NOT NULL DROP TABLE dbo.BorrowRequests;
IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL DROP TABLE dbo.Books;
IF OBJECT_ID('dbo.Librarians', 'U') IS NOT NULL DROP TABLE dbo.Librarians;
IF OBJECT_ID('dbo.Admins', 'U') IS NOT NULL DROP TABLE dbo.Admins;
GO

-- Таблиця Admins
CREATE TABLE Admins (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL DEFAULT 'Administrator'
);
GO

-- Таблиця Librarians
CREATE TABLE Librarians (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL
);
GO

-- Таблиця Books
CREATE TABLE Books (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(150) NOT NULL,
    Status NVARCHAR(20) 
        CHECK (Status IN ('Available', 'Borrowed')) DEFAULT 'Available'
);
GO

-- Таблиця BorrowRequests
CREATE TABLE BorrowRequests (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    LibrarianID INT NOT NULL,
    BookID INT NOT NULL,
    RequestDate DATETIME DEFAULT GETDATE(),
    BorrowerName NVARCHAR(100) NOT NULL,
    Notes NVARCHAR(500) NULL,
    Status NVARCHAR(20)
        CHECK (Status IN ('Pending', 'Approved', 'Rejected')) DEFAULT 'Pending',
    CONSTRAINT FK_BorrowRequests_Librarians FOREIGN KEY (LibrarianID)
        REFERENCES Librarians(ID) ON DELETE CASCADE,
    CONSTRAINT FK_BorrowRequests_Books FOREIGN KEY (BookID)
        REFERENCES Books(ID) ON DELETE CASCADE
);
GO

-- ============================================
--   Додання індексів (ОПТИМІЗАЦІЯ)
-- ============================================

-- Унікальний індекс для пошуку за Email
CREATE UNIQUE INDEX IX_Librarians_Email
    ON Librarians (Email);
GO

-- Індекси на Foreign Keys BorrowRequests
CREATE INDEX IX_BorrowRequests_LibrarianID
    ON BorrowRequests (LibrarianID);
GO

CREATE INDEX IX_BorrowRequests_BookID
    ON BorrowRequests (BookID);
GO

-- Індекс для фільтрації по статусу книжок
CREATE INDEX IX_Books_Status
    ON Books (Status);
GO

-- ============================================
-- Вставка тестових даних
-- ============================================

INSERT INTO Admins (Username, PasswordHash, Role)
VALUES
('admin', '$2b$12$vKxXc57i4wpunkKs8s321ebi2GRJmlRLCtQRuD5BSa5giGXa/MNAm', 'Administrator');

INSERT INTO Librarians (Name, Email, PasswordHash)
VALUES
(N'Олена Коваль', N'olena.koval@library.com', '$2a$11$hMgF1UtGTeh2SmFxtodZje0aoyxzCFyUn4wUe3rRwxEzqieYvNaqW'),
(N'Ігор Петренко', N'ihor.petrenko@library.com', '$2a$11$KWJFyA1XhpSB.45m2pVC2.3ODUpnbfUgLyD/cZVr7Rq8RfehL8Nxe');

INSERT INTO Books (Title, Author, Status)
VALUES
(N'Мистецтво війни', N'Сунь-Цзи', 'Available'),
(N'Кобзар', N'Тарас Шевченко', 'Borrowed'),
(N'1984', N'Джордж Орвелл', 'Available');

INSERT INTO BorrowRequests (LibrarianID, BookID, BorrowerName, Notes, Status)
VALUES
(1, 2, N'Марія Савчук', N'Повернення до 15.12', 'Approved'),
(2, 3, N'Павло Литвин', NULL, 'Pending');
GO

-- Перевірка даних
SELECT * FROM Librarians;
SELECT * FROM Books;
SELECT * FROM BorrowRequests;
GO
