using Expense_Tracker.Data;
using Expense_Tracker.DTO;
using Expense_Tracker.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BudgetsController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirst("UserId")!.Value);

        // GET /api/budgets/current
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentBudgets()
        {
            int userId = GetUserId();
            string month = DateTime.Now.ToString("yyyy-MM");
            int year = DateTime.Now.Year;
            int mon = DateTime.Now.Month;

            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId && b.Month == month)
                .Include(b => b.Category)
                .ToListAsync();

            var spending = await _context.Expenses
                .Where(e => e.UserId == userId &&
                            e.Date.Year == year &&
                            e.Date.Month == mon)
                .GroupBy(e => e.CategoryId)
                .Select(g => new { CategoryId = g.Key, Spent = g.Sum(e => e.Amount) })
                .ToListAsync();

            var result = budgets.Select(b =>
            {
                var spent = spending.FirstOrDefault(s => s.CategoryId == b.CategoryId)?.Spent ?? 0;
                return new BudgetStatusDto
                {
                    CategoryId = b.CategoryId,
                    CategoryName = b.Category!.Name,
                    Color = b.Category.ColorCode,
                    Icon = b.Category.Icon,
                    BudgetLimit = b.BudgetLimit,
                    AmountSpent = spent,
                    SpentPercent = b.BudgetLimit > 0
                        ? Math.Round(spent / b.BudgetLimit * 100, 2)
                        : 0
                };
            });

            return Ok(result);
        }

        // PUT /api/budgets
        [HttpPut]
        public async Task<IActionResult> SetBudget([FromBody] BudgetDto dto)
        {
            int userId = GetUserId();
            var existing = await _context.Budgets
                .FirstOrDefaultAsync(b =>
                    b.UserId == userId &&
                    b.CategoryId == dto.CategoryId &&
                    b.Month == dto.Month);

            if (existing != null)
            {
                existing.BudgetLimit = dto.BudgetLimit;
            }
            else
            {
                _context.Budgets.Add(new Budget
                {
                    UserId = userId,
                    CategoryId = dto.CategoryId,
                    Month = dto.Month,
                    BudgetLimit = dto.BudgetLimit
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Budget saved.");
        }
    }
}
