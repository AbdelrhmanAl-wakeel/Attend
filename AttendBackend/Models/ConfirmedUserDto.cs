namespace AttendBackend.Models
{
    public class ConfirmedUserDto
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
