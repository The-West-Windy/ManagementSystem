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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Librarian>().HasData(
                new Librarian { ID = 1, Name = "Олена Коваль", Email = "olena.koval@library.com", PasswordHash = "hash123" },
                new Librarian { ID = 2, Name = "Ігор Петренко", Email = "ihor.petrenko@library.com", PasswordHash = "hash456" }
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
                    Status = "Approved"
                },
                new BorrowRequest
                {
                    ID = 2,
                    LibrarianID = 2,
                    BookID = 3,
                    RequestDate = new DateTime(2025, 11, 2),
                    Status = "Pending"
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
        public DateTime RequestDate { get; set; }   // 🔹
        public string Status { get; set; } = "Pending";
        public Librarian? Librarian { get; set; }
        public Book? Book { get; set; }
    }
}
