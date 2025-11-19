using System.Text.Json.Serialization;

namespace ClientApp.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BorrowRequestStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
