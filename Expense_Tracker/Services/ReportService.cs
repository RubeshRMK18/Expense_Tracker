using Expense_Tracker.Data;
using Expense_Tracker.DTO;

namespace Expense_Tracker.Services
{
    public class ReportService
    {

        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        // 📊 Monthly Report
        public async Task<List<ReportResponseDTO>> GetMonthlyReport(int userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new ReportResponseDTO
                {
                    Label = $"{g.Key.Month}/{g.Key.Year}",
                    Total = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Label)
                .ToListAsync();
        }

        // 📊 Category Report
        public async Task<List<ReportResponseDTO>> GetCategoryReport(int userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .GroupBy(e => e.Category)
                .Select(g => new ReportResponseDTO
                {
                    Label = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .ToListAsync();
        }

        // ⚠️ Summary + Insight
        public async Task<SummaryDTO> GetSummary(int userId)
        {
            var expenses = await _context.Expenses
                .Where(e => e.UserId == userId)
                .ToListAsync();

            if (!expenses.Any())
            {
                return new SummaryDTO
                {
                    TotalExpense = 0,
                    DailyAverage = 0,
                    Insight = "No expenses found"
                };
            }

            var total = expenses.Sum(e => e.Amount);

            var minDate = expenses.Min(e => e.Date);
            var maxDate = expenses.Max(e => e.Date);

            var totalDays = (maxDate - minDate).Days + 1;

            var dailyAvg = totalDays > 0 ? total / totalDays : total;

          
            string insight;

            if (dailyAvg > 1000)
                insight = "High spending pattern detected";
            else if (dailyAvg > 500)
                insight = "Moderate spending";
            else
                insight = "Spending is under control";

            return new SummaryDTO
            {
                TotalExpense = total,
                DailyAverage = dailyAvg,
                Insight = insight
            };
        }
    }
}