using Microsoft.EntityFrameworkCore;
using AttendBackend.Models;

namespace AttendBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<UserNumber> UserNumbers => Set<UserNumber>();
        public DbSet<AttendedUser> AttendedUsers => Set<AttendedUser>();
    }
}
