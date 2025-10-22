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


IF OBJECT_ID('dbo.BorrowRequests', 'U') IS NOT NULL DROP TABLE dbo.BorrowRequests;
IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL DROP TABLE dbo.Books;
IF OBJECT_ID('dbo.Librarians', 'U') IS NOT NULL DROP TABLE dbo.Librarians;
GO



CREATE TABLE Librarians (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL
);

CREATE TABLE Books (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(150) NOT NULL,
    Status NVARCHAR(20) CHECK (Status IN ('Available', 'Borrowed')) DEFAULT 'Available'
);

CREATE TABLE BorrowRequests (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    LibrarianID INT NOT NULL,
    BookID INT NOT NULL,
    RequestDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'Approved', 'Rejected')) DEFAULT 'Pending',
    CONSTRAINT FK_BorrowRequests_Librarians FOREIGN KEY (LibrarianID)
        REFERENCES Librarians(ID) ON DELETE CASCADE,
    CONSTRAINT FK_BorrowRequests_Books FOREIGN KEY (BookID)
        REFERENCES Books(ID) ON DELETE CASCADE
);
GO



INSERT INTO Librarians (Name, Email, PasswordHash)
VALUES
(N'Олена Коваль', N'olena.koval@library.com', 'hash123'),
(N'Ігор Петренко', N'ihor.petrenko@library.com', 'hash456');

INSERT INTO Books (Title, Author, Status)
VALUES
(N'Мистецтво війни', N'Сунь-Цзи', 'Available'),
(N'Кобзар', N'Тарас Шевченко', 'Borrowed'),
(N'1984', N'Джордж Орвелл', 'Available');

INSERT INTO BorrowRequests (LibrarianID, BookID, Status)
VALUES
(1, 2, 'Approved'),
(2, 3, 'Pending');
GO

SELECT * FROM Librarians;
SELECT * FROM Books;
SELECT * FROM BorrowRequests;
GO
