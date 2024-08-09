using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RareServer.Models;
using RareServer.Service;
using System.Diagnostics;

namespace RareServer.Controllers
{
    public class TimeEntryController : Controller
    {
        private readonly ITimeEntryService _timeEntryService;

        public TimeEntryController(ITimeEntryService timeEntryService)
        {
            _timeEntryService = timeEntryService;
        }

        // GET: TimeEntryController
        public async Task<ActionResult> Index()
        {
            try
            {
                var timeEntries = await _timeEntryService.GetTimeEntryAsync();

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