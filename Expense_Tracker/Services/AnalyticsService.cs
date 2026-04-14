using Expense_Tracker.Data;
using Expense_Tracker.DTO;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Services
{
    public class AnalyticsService
    {

        private readonly AppDbContext _context;

        public AnalyticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AnalyticsDTO> GetAnalytics(int userId)
        {
            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();

            if (!expenses.Any())
            {
                return new AnalyticsDTO
                {
                    DailyAverage = 0,
                    TopCategory = "N/A",
                    TopCategoryAmount = 0,
                    SpendingTrend = "No data",
                    Insight = "No expenses found"
                };
            }

            var total = expenses.Sum(e => e.Amount);
            var minDate = expenses.Min(e => e.Date);
            var maxDate = expenses.Max(e => e.Date);
            var days = (maxDate - minDate).Days + 1;

            var dailyAvg = days > 0 ? total / days : total;

            var topCategory = expenses
                  .GroupBy(e => e.Category)
                   .Select(g => new
         {
          Category = g.Key,
          Total = g.Sum(x => x.Amount)
           })
         .OrderByDescending(x => x.Total)
      .FirstOrDefault();

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var currentMonthTotal = expenses
                .Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear)
                .Sum(e => e.Amount);

            var lastMonth = DateTime.Now.AddMonths(-1);

            var lastMonthTotal = expenses
                .Where(e => e.Date.Month == lastMonth.Month && e.Date.Year == lastMonth.Year)
                .Sum(e => e.Amount);

            string trend;

            if (currentMonthTotal > lastMonthTotal)
                trend = "Increasing ";
            else if (currentMonthTotal < lastMonthTotal)
                trend = "Decreasing ";
            else
                trend = "Stable";

        
            string insight;

            if (dailyAvg > 1000)
                insight = "High spending detected!";
            else if (dailyAvg > 500)
                insight = "Moderate spending pattern";
            else
                insight = "Spending is under control";

            return new AnalyticsDTO
            {
                DailyAverage = dailyAvg,
                TopCategory = topCategory.Category,
                TopCategoryAmount = topCategory.Total,
                SpendingTrend = trend,
                Insight = insight
            };
        }
    }
}
