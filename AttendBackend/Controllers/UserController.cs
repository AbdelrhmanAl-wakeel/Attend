using AttendBackend.Data;
using AttendBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/User/submit
        [HttpPost("submit")]
        public IActionResult Submit([FromBody] PhoneNumberDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                return BadRequest("Phone number is required.");

            var user = new UserNumber
            {
                PhoneNumber = dto.PhoneNumber,
                SubmittedAt = DateTime.UtcNow
            };

            _context.UserNumbers.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "Number received" });
        }

        // GET: api/User/matches
        [HttpGet("matches")]
        public IActionResult GetMatchedUsers()
        {
            var matched = _context.AttendedUsers.ToList();
            return Ok(matched);
        }

        [HttpGet("GetConfirmedUsers")]
        public IActionResult GetConfirmedUsers()
        {
            var submittedNumbers = _context.UserNumbers
                .Select(u => u.PhoneNumber)
                .ToHashSet();

            var confirmedUsers = _context.AttendedUsers
                .Select(a => new ConfirmedUserDto
                {
                    PhoneNumber = a.PhoneNumber,
                    Name = a.Name,
                    Email = a.Email,
                    IsConfirmed = submittedNumbers.Contains(a.PhoneNumber)
                })
                .ToList();

            return Ok(confirmedUsers);
        }
    }
}
