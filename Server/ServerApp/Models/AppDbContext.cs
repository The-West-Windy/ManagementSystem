using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRequest> BorrowRequests { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BorrowRequest>()
                .Property(br => br.Status)
                .HasConversion<string>();

            modelBuilder.Entity<BorrowRequest>()
                .Property(br => br.BorrowerName)
                .HasMaxLength(100);

            modelBuilder.Entity<BorrowRequest>()
                .Property(br => br.Notes)
                .HasMaxLength(500);

            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    ID = 1,
                    Username = "admin",
                    PasswordHash = "$2b$12$vKxXc57i4wpunkKs8s321ebi2GRJmlRLCtQRuD5BSa5giGXa/MNAm",
                    Role = "Administrator"
                }
            );

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
                    Status = BorrowRequestStatus.Approved,
                    BorrowerName = "Марія Савчук",
                    Notes = "Повернення до 15.12"
                },
                new BorrowRequest
                {
                    ID = 2,
                    LibrarianID = 2,
                    BookID = 3,
                    RequestDate = new DateTime(2025, 11, 2),
                    Status = BorrowRequestStatus.Pending,
                    BorrowerName = "Павло Литвин",
                    Notes = null
                }
            );
        }

    }

    public class Librarian
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<BorrowRequest>? BorrowRequests { get; set; }
    }

    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Status { get; set; } = "Available";
        public ICollection<BorrowRequest>? BorrowRequests { get; set; }
    }

    public class BorrowRequest
    {
        public int ID { get; set; }
        public int LibrarianID { get; set; }
        public int BookID { get; set; }
        public DateTime RequestDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string BorrowerName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Notes { get; set; }

        public BorrowRequestStatus Status { get; set; } = BorrowRequestStatus.Pending;

        public Librarian? Librarian { get; set; }
        public Book? Book { get; set; }
    }
}
