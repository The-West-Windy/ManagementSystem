namespace ServerApp.Models.DTOs
{
    public class BorrowRequestDto
    {
        public int LibrarianID { get; set; }
        public int BookID { get; set; }
        public string Status { get; set; }
    }
}
