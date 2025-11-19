using Microsoft.EntityFrameworkCore;
using System;

namespace ServerApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRequest> BorrowRequests { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Librarian>().HasData(
                new Librarian
                {
                    ID = 1,
                    Name = "Олена Коваль",
                    Email = "olena.koval@library.com",
                    PasswordHash = "$2a$11$hMgF1UtGTeh2SmFxtodZje0aoyxzCFyUn4wUe3rRwxEzqieYvNaqW"
                },
                new Librarian
                {
                    ID = 2,
                    Name = "Ігор Петренко",
                    Email = "ihor.petrenko@library.com",
                    PasswordHash = "$2a$11$KWJFyA1XhpSB.45m2pVC2.3ODUpnbfUgLyD/cZVr7Rq8RfehL8Nxe"
                }
            );

            modelBuilder.Entity<AdminUser>().HasData(
                new AdminUser
                {
                    ID = 1,
                    Username = "admin",
                    PasswordHash = "$2y$11$u41.KkjrWn5D1cbsAV44o.SfYjKO6YInBfGtheVfAWH6IGC78xNoy",
                    Role = "Administrator"
                }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { ID = 1, Title = "Мистецтво війни", Author = "Сунь-Цзи", Status = "Available" },
                new Book { ID = 2, Title = "Кобзар", Author = "Тарас Шевченко", Status = "Borrowed" },
                new Book { ID = 3, Title = "1984", Author = "Джордж Орвелл", Status = "Available" }
            );

            modelBuilder.Entity<BorrowRequest>().HasData(
                new BorrowRequest
                {
                    ID = 1,
                    LibrarianID = 1,
                    BookID = 2,
                    RequestDate = new DateTime(2025, 11, 1),
                    BorrowerName = "Марія Клименко",
                    Notes = "Request placed during conference week",
                    Status = BorrowRequestStatus.Approved
                },
                new BorrowRequest
                {
                    ID = 2,
                    LibrarianID = 2,
                    BookID = 3,
                    RequestDate = new DateTime(2025, 11, 2),
                    BorrowerName = "Олег Данилюк",
                    Notes = null,
                    Status = BorrowRequestStatus.Pending
                }
            );
        }

    }

    public class Librarian
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public ICollection<BorrowRequest>? BorrowRequests { get; set; }
    }

    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public string Status { get; set; } = "Available";
        public ICollection<BorrowRequest>? BorrowRequests { get; set; }
    }

    public class BorrowRequest
    {
        public int ID { get; set; }
        public int LibrarianID { get; set; }
        public int BookID { get; set; }
        public DateTime RequestDate { get; set; }
        public BorrowRequestStatus Status { get; set; } = BorrowRequestStatus.Pending;
        public string BorrowerName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public Librarian? Librarian { get; set; }
        public Book? Book { get; set; }
    }

    public enum BorrowRequestStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
