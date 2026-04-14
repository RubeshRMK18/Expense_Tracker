using Expense_Tracker.Data;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Models;

namespace Expense_Tracker.Services
{
    public class ExpenseService
    {
        private readonly AppDbContext _context;

        public ExpenseService(AppDbContext context)
        {
            _context = context;
        }

        public List<ExpenseResponseDTO> GetExpensesByUserId(int userId)
        {
            return _context.Expenses
                .Where(e => e.UserId == userId)
                .Select(e => new ExpenseResponseDTO
                {
                    Id = e.Id,
                    Amount = e.Amount,
                    Category = e.Category,
                    Date = e.Date,
                    Notes = e.Notes,
                })
                .ToList();
        }

        public async Task<string> AddExpense(ExpenseCreateDTO dto)
        {
            var expense = new Expense
            {
                Amount = dto.Amount,
                Category = dto.Category,
                Date = dto.Date,
                Notes = dto.Notes,
                UserId = dto.UserId
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var TodayTotal = _context.Expenses
                .Where(e => e.UserId == dto.UserId && e.Date.Date == DateTime.Today)
                .Sum(e => e.Amount);

            if(TodayTotal > 2000)
            {
                return "Warning: Your total expenses for today have exceeded $2000.";   
            }
            return "Expense added successfully.";
        }

        public string DeleteExpense(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
                return "Expense not found";
            _context.Expenses.Remove(expense);
            _context.SaveChanges();
            return "Expense deleted successfully";
        }
    }
}
