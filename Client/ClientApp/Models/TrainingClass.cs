namespace ClientApp.Models
{
    public class TrainingClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CoachID { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string? CoachName { get; set; }
        public string Description => $"{CoachName ?? "Coach"} • {TimeSlot}";
    }
}
