namespace ClientApp.Models
{
    public class BorrowRequest
    {
        public string BookTitle { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime RequestedOn { get; set; } = DateTime.UtcNow;
        public string Notes { get; set; } = string.Empty;
    }
}
