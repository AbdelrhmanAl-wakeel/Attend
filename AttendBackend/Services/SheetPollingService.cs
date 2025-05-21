using AttendBackend.Data;
using AttendBackend.Models;
using Microsoft.Extensions.Options;

namespace AttendBackend.Services
{
    public class PollingOptions
    {
        public int PollIntervalMinutes { get; set; }
    }
    public class SheetPollingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly GoogleSheetService _sheetService;
        private readonly ILogger<SheetPollingService> _logger;
        private readonly PollingOptions _options;
        private DateTime _lastChecked = DateTime.MinValue;

        public SheetPollingService(
            IServiceScopeFactory scopeFactory,
            GoogleSheetService sheetService,
            IOptions<PollingOptions> opts,
            ILogger<SheetPollingService> logger)
        {
            _scopeFactory = scopeFactory;
            _sheetService = sheetService;
            _options = opts.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Sheet polling service started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var rows = await _sheetService.GetAllRowsAsync();
                    if (rows.Count > 1)  // skip header
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        // assuming header columns: Name, PhoneNumber, Email (adjust indexes as needed)
                        for (int i = 1; i < rows.Count; i++)
                        {
                            var row = rows[i];
                            var responseDate = row[0]?.ToString() ?? "";
                            var name = row[1]?.ToString() ?? "";
                            var phone = PhoneNumberHelper.Normalize(row[2]?.ToString() ?? "") ;
                            var email = row[3]?.ToString() ?? "";
                            var choice = row[4]?.ToString() ?? "";
                            
                            // skip if phone is empty or already in AttendedUsers
                            if (string.IsNullOrWhiteSpace(phone) ||
                                db.AttendedUsers.Any(u => u.PhoneNumber == phone))
                                continue;

                            db.AttendedUsers.Add(new AttendedUser
                            {
                                ResponseDate= responseDate,
                                PhoneNumber = phone,
                                Name = name,
                                Email = email,
                                Choice = choice,
                                MatchedAt = DateTime.UtcNow
                            });
                        }

                        await db.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation("Sheet polling: processed {Count} rows.", rows.Count - 1);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during sheet polling");
                }

                await Task.Delay(TimeSpan.FromMinutes(_options.PollIntervalMinutes), stoppingToken);
            }
        }
    }
}
