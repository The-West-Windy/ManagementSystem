using ServerApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models.DTOs
{
    public class BorrowRequestDto
    {
        [Range(1, int.MaxValue)]
        public int LibrarianID { get; set; }

        [Range(1, int.MaxValue)]
        public int BookID { get; set; }

        [Required]
        [MaxLength(100)]
        public string BorrowerName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Notes { get; set; }

        public BorrowRequestStatus Status { get; set; } = BorrowRequestStatus.Pending;
    }
}
