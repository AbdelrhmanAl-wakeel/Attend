using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Options;

namespace AttendBackend.Services
{
    public class GoogleSheetOptions
    {
        public string SpreadsheetId { get; set; } = "";
        public string SheetName { get; set; } = "";
    }

    public class GoogleSheetService
    {
        private readonly SheetsService _sheets;
        private readonly GoogleSheetOptions _options;

        public GoogleSheetService(IOptions<GoogleSheetOptions> opts)
        {
            _options = opts.Value;

            GoogleCredential credential;
            using var stream = new FileStream("google-creds.json", FileMode.Open, FileAccess.Read);
            credential = GoogleCredential.FromStream(stream)
                                       .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            _sheets = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "AttendSheetsPoller"
            });
        }

        public async Task<IList<IList<object>>> GetAllRowsAsync()
        {
            var range = $"{_options.SheetName}!A:Z";
            var request = _sheets.Spreadsheets.Values.Get(_options.SpreadsheetId, range);
            var response = await request.ExecuteAsync();
            return response.Values ?? new List<IList<object>>();
        }
    }
}
