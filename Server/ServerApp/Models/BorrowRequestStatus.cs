using System.Text.Json.Serialization;

namespace ServerApp.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BorrowRequestStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
