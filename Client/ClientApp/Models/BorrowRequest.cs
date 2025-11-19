namespace ClientApp.Models
{
    public class BorrowRequest
    {
        public int LibrarianID { get; set; }
        public int BookID { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public BorrowRequestStatus Status { get; set; } = BorrowRequestStatus.Pending;
    }
}
