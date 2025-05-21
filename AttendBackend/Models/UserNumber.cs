namespace AttendBackend.Models
{
    public class UserNumber
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
