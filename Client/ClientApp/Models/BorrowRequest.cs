using System.Text.Json.Serialization;

namespace ClientApp.Models
{
    public class BorrowRequest
    {
        public int BookID { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public string? Notes { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BorrowRequestStatus Status { get; set; } = BorrowRequestStatus.Pending;
    }
}
