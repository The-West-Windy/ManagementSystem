namespace ClientApp.Models
{
    public class Booking
    {
        public int CoachID { get; set; }
        public int ClassID { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
