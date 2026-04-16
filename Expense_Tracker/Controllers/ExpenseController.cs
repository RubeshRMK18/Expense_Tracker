using Expense_Tracker.Data;
using Expense_Tracker.DTO;
using Expense_Tracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Expense_Tracker.DTO.SplitDTO;

namespace Expense_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpenseService _expenseService;
        private readonly AppDbContext _context;
        private readonly BudgetAlertService _budgetAlertService;

        public ExpensesController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromForm] CreateExpenseDto dto, IFormFile receipt = null)
        {
            var result = await _expenseService.CreateAsync(GetUserId(), dto, receipt);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 20)
        {
            var result = await _expenseService.GetAllAsync(GetUserId(), page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _expenseService.GetByIdAsync(GetUserId(), id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateExpenseDto dto)
        {
            try
            {
                var result = await _expenseService.UpdateAsync(GetUserId(), id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _expenseService.DeleteAsync(GetUserId(), id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("split")]
        public async Task<IActionResult> SplitExpense([FromBody] SplitExpenseDto dto)
        {
            int userId = GetUserId();

            // Validate splits sum to total
            decimal splitsTotal = dto.Splits.Sum(s => s.Amount);
            if (Math.Round(splitsTotal, 2) != Math.Round(dto.Total, 2))
                return BadRequest("Split amounts must sum to the total.");

            Guid splitGroupId = Guid.NewGuid();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var split in dto.Splits)
                {
                    _context.Expenses.Add(new Expense
                    {
                        UserId = userId,
                        Amount = split.Amount,
                        CategoryId = split.CategoryId,
                        Description = dto.Description,
                        Date = dto.Date,
                        SplitGroupId = splitGroupId
                    });
                }

                await _context.SaveChangesAsync();

                // Trigger budget alerts for each split category
                foreach (var split in dto.Splits)
                    await _budgetAlertService.CheckAndCreateAlerts(userId, split.CategoryId);

                await transaction.CommitAsync();
                return Ok("Split expense added successfully.");
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Failed to save split expense.");
            }
        }

        // GET /api/expenses/summary/by-category — UC5
        [HttpGet("summary/by-category")]
        public async Task<IActionResult> SummaryByCategory(
            [FromQuery] int month, [FromQuery] int year)
        {
            int userId = GetUserId();

            var rawData = await _context.Expenses
                .Where(e => e.UserId == userId &&
                            e.Date.Month == month &&
                            e.Date.Year == year)
                .GroupBy(e => new { e.CategoryId, e.Category!.Name, e.Category.ColorCode, e.Category.Icon })
                .Select(g => new
                {
                    g.Key.Name,
                    g.Key.ColorCode,
                    g.Key.Icon,
                    Total = g.Sum(e => e.Amount)
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();

            decimal grandTotal = rawData.Sum(x => x.Total);

            var result = rawData.Select(x => new CatergorySummaryDto
            {
                CategoryName = x.Name,
                Color = x.ColorCode,
                Icon = x.Icon,
                TotalAmount = x.Total,
                Percentage = grandTotal > 0
                    ? Math.Round(x.Total / grandTotal * 100, 2)
                    : 0
            });

            return Ok(result);
        }
    }
}
