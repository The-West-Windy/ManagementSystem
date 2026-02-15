using Microsoft.EntityFrameworkCore;
using System;

namespace ServerApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Coach> Coaches { get; set; }
        public DbSet<TrainingClass> Classes { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coach>().HasData(
                new Coach
                {
                    ID = 1,
                    Name = "Олена Фітнес",
                    Email = "olena.fit@fitgym.com",
                    PasswordHash = "$2a$11$hMgF1UtGTeh2SmFxtodZje0aoyxzCFyUn4wUe3rRwxEzqieYvNaqW"
                },
                new Coach
                {
                    ID = 2,
                    Name = "Ігор Тренер",
                    Email = "ihor.trainer@fitgym.com",
                    PasswordHash = "$2a$11$KWJFyA1XhpSB.45m2pVC2.3ODUpnbfUgLyD/cZVr7Rq8RfehL8Nxe"
                }
            );

            modelBuilder.Entity<TrainingClass>().HasData(
                new TrainingClass { ID = 1, Name = "Ранковий HIIT", CoachID = 1, TimeSlot = "Пн 08:00" },
                new TrainingClass { ID = 2, Name = "Силова підготовка", CoachID = 2, TimeSlot = "Вт 18:00" },
                new TrainingClass { ID = 3, Name = "Функціональний мікс", CoachID = 1, TimeSlot = "Чт 19:30" }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    ID = 1,
                    CoachID = 1,
                    ClassID = 2,
                    ClientName = "Анна",
                    Status = "Confirmed"
                },
                new Booking
                {
                    ID = 2,
                    CoachID = 2,
                    ClassID = 3,
                    ClientName = "Петро",
                    Status = "Pending"
                }
            );
        }

    }

    public class Coach
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public ICollection<Booking>? Bookings { get; set; }
    }

    public class TrainingClass
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public int CoachID { get; set; }
        public string TimeSlot { get; set; } = "";
        public Coach? Coach { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }

    public class Booking
    {
        public int ID { get; set; }
        public int CoachID { get; set; }
        public int ClassID { get; set; }
        public string ClientName { get; set; } = "";
        public string Status { get; set; } = "Pending";
        public Coach? Coach { get; set; }
        public TrainingClass? Class { get; set; }
    }
}
