using Microsoft.Extensions.Options;
using RareServer.Config;
using RareServer.Models;

namespace RareServer.Service
{
    public interface ITimeEntryService
    {
        Task<List<EmployeeTimeSummary>> GetTimeEntryAsync();
    }

    public class TimeEntryService : ITimeEntryService
    {
        private readonly ApiSettings _settings;
        private readonly HttpClient _httpClient;

        public TimeEntryService(IOptions<ApiSettings> settings, HttpClient httpClient)
        {
            _settings = settings.Value;
            _httpClient = httpClient;
        }

        public async Task<List<EmployeeTimeSummary>> GetTimeEntryAsync()
        {
            try
            {
                var timeEntries = await _httpClient.GetFromJsonAsync<List<TimeEntry>>(_settings.ApiGet);

                if (timeEntries == null || !timeEntries.Any())
                {
                    return new List<EmployeeTimeSummary>();
                }

                return FilterEntries(timeEntries);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<EmployeeTimeSummary> FilterEntries(List<TimeEntry> timeEntries)
        {
            var employeeSummaries = timeEntries
                .GroupBy(te => string.IsNullOrEmpty(te.EmployeeName) ? "No Name" : te.EmployeeName)
                .Select(g => new EmployeeTimeSummary
                {
                    EmployeeName = g.Key,
                    TotalTimeWorked = Math.Round((decimal)g.Sum(te => (te.EndTimeUtc - te.StarTimeUtc).TotalHours), 0)
                })
                .OrderByDescending(e => e.TotalTimeWorked)
                .ToList();

            return employeeSummaries;
        }
    }
}