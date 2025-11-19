using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models.DTOs
{
    public class BorrowRequestDto
    {
        [Range(1, int.MaxValue)]
        public int BookID { get; set; }

        [Required]
        [StringLength(100)]
        public string BorrowerName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public BorrowRequestStatus Status { get; set; } = BorrowRequestStatus.Pending;
    }
}
