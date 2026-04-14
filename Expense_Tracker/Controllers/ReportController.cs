using Expense_Tracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("monthly/{userId}")]
        public async Task<IActionResult> GetMonthly(int userId)
        {
            var data = await _reportService.GetMonthlyReport(userId);
            return Ok(data);
        }

        [HttpGet("category/{userId}")]
        public async Task<IActionResult> GetCategory(int userId)
        {
            var data = await _reportService.GetCategoryReport(userId);
            return Ok(data);
        }

        [HttpGet("summary/{userId}")]
        public async Task<IActionResult> GetSummary(int userId)
        {
            var data = await _reportService.GetSummary(userId);
            return Ok(data);
        }
    }
}
