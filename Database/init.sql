-- ============================================
--   БАЗА ДАНИХ: TrainingSystem
--   Опис: Ініціалізація таблиць і тестових даних
--   СУБД: Microsoft SQL Server
-- ============================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TrainingSystem')
BEGIN
    CREATE DATABASE TrainingSystem;
END
GO

USE TrainingSystem;
GO

-- Видалення таблиць, якщо існують
IF OBJECT_ID('dbo.Bookings', 'U') IS NOT NULL DROP TABLE dbo.Bookings;
IF OBJECT_ID('dbo.Classes', 'U') IS NOT NULL DROP TABLE dbo.Classes;
IF OBJECT_ID('dbo.Coaches', 'U') IS NOT NULL DROP TABLE dbo.Coaches;
GO

-- Таблиця Coaches
CREATE TABLE Coaches (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL
);
GO

-- Таблиця Classes
CREATE TABLE Classes (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    CoachID INT NOT NULL,
    TimeSlot NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Classes_Coaches FOREIGN KEY (CoachID)
        REFERENCES Coaches(ID) ON DELETE CASCADE
);
GO

-- Таблиця Bookings
CREATE TABLE Bookings (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    CoachID INT NOT NULL,
    ClassID INT NOT NULL,
    ClientName NVARCHAR(150) NOT NULL,
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'Confirmed', 'Cancelled')) DEFAULT 'Pending',
    CONSTRAINT FK_Bookings_Coaches FOREIGN KEY (CoachID)
        REFERENCES Coaches(ID) ON DELETE CASCADE,
    CONSTRAINT FK_Bookings_Classes FOREIGN KEY (ClassID)
        REFERENCES Classes(ID) ON DELETE CASCADE
);
GO

-- ============================================
--   Додання індексів (ОПТИМІЗАЦІЯ)
-- ============================================

CREATE UNIQUE INDEX IX_Coaches_Email
    ON Coaches (Email);
GO

CREATE INDEX IX_Classes_CoachID
    ON Classes (CoachID);
GO

CREATE INDEX IX_Bookings_CoachID
    ON Bookings (CoachID);
GO

CREATE INDEX IX_Bookings_ClassID
    ON Bookings (ClassID);
GO

-- ============================================
-- Вставка тестових даних
-- ============================================

INSERT INTO Coaches (Name, Email, PasswordHash)
VALUES
(N'Олена Фітнес', N'olena.fit@fitgym.com', '$2a$11$hMgF1UtGTeh2SmFxtodZje0aoyxzCFyUn4wUe3rRwxEzqieYvNaqW'),
(N'Ігор Тренер', N'ihor.trainer@fitgym.com', '$2a$11$KWJFyA1XhpSB.45m2pVC2.3ODUpnbfUgLyD/cZVr7Rq8RfehL8Nxe');

INSERT INTO Classes (Name, CoachID, TimeSlot)
VALUES
(N'Ранковий HIIT', 1, N'Пн 08:00'),
(N'Силова підготовка', 2, N'Вт 18:00'),
(N'Функціональний мікс', 1, N'Чт 19:30');

INSERT INTO Bookings (CoachID, ClassID, ClientName, Status)
VALUES
(1, 2, N'Анна', 'Confirmed'),
(2, 3, N'Петро', 'Pending');
GO

-- Перевірка даних
SELECT * FROM Coaches;
SELECT * FROM Classes;
SELECT * FROM Bookings;
GO
