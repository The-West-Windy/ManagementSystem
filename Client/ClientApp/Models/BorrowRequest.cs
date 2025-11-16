namespace ClientApp.Models
{
    public class BorrowRequest
    {
        public int LibrarianID { get; set; }
        public int BookID { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
