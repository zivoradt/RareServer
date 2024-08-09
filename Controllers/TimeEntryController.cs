using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RareServer.Models;
using RareServer.Service;
using RareServer.Service.ChartServices;
using System.Diagnostics;

namespace RareServer.Controllers
{
    public class TimeEntryController : Controller
    {
        private readonly ITimeEntryService _timeEntryService;

        private readonly IChartService _chartService;

        public TimeEntryController(ITimeEntryService timeEntryService, IChartService chartService)
        {
            _timeEntryService = timeEntryService;
            _chartService = chartService;
        }

        // GET: TimeEntryController
        public async Task<ActionResult> Index()
        {
            try
            {
                var timeEntries = await _timeEntryService.GetTimeEntryAsync();

                var chartData = _chartService.GeneratePieChart(timeEntries);

                ViewBag.ChartData = chartData;

                return View(timeEntries);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { errorMessage = ex.Message });
            }
        }

        public IActionResult Error(string errorMessage)
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = errorMessage ?? "An unexpected error occurred."
            };

            return View(model);
        }
    }
}