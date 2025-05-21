namespace AttendBackend.Models
{
    public class AttendedUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ResponseDate{ get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime MatchedAt { get; set; } = DateTime.UtcNow;
        public string Choice { get; set; } = null!;
    }
}
