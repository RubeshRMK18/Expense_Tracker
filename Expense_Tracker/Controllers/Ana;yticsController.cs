using Expense_Tracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Ana_yticsController : ControllerBase
    {
        private readonly AnalyticsService _analyticsService;

        public AnalyticsController(AnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAnalytics(int userId)
        {
            var result = await _analyticsService.GetAnalytics(userId);
            return Ok(result);
        }
    }
}
