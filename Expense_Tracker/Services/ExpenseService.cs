using Expense_Tracker.Data;
using Expense_Tracker.Model;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Services
{
    public class ExpenseService
    {
        private readonly AppDbContext _context;
        private readonly FileUploadService _fileUpload;

        public ExpenseService(AppDbContext context, FileUploadService fileUpload)
        {
            _context = context;
            _fileUpload = fileUpload;
        }

        public async Task<ExpenseDto> CreateAsync(int userId, CreateExpenseDto dto, IFormFile receipt)
        {
            var expense = new Expense
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Date = dto.Date,
                PaymentMethod = dto.PaymentMethod,
                Description = dto.Description,
                IsRecurring = dto.IsRecurring,
                CreatedAt = DateTime.UtcNow
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            if (receipt != null)
            {
                var path = await _fileUpload.SaveAsync(receipt, userId);

                _context.Receipts.Add(new Receipt
                {
                    ExpenseId = expense.ExpenseId,
                    FilePath = path,
                    FileType = receipt.ContentType,
                    UploadedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            return await GetByIdAsync(userId, expense.ExpenseId);
        }
        public async Task<object> GetAllAsync(int userId, int page, int pageSize)
        {
            var query = _context.Expenses
                .Where(e => e.UserId == userId)
                .Include(e => e.Category)
                .Include(e => e.Receipt)
                .OrderByDescending(e => e.Date);

            var total = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new ExpenseDto
                {
                    ExpenseId = e.ExpenseId,
                    Amount = e.Amount,
                    Date = e.Date,
                    CategoryName = e.Category.Name,
                    CategoryColor = e.Category.ColorCode,
                    PaymentMethod = e.PaymentMethod,
                    Description = e.Description,
                    IsRecurring = e.IsRecurring,
                    ReceiptPath = e.Receipt != null ? e.Receipt.FilePath : null,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            return new { total, page, pageSize, items };
        }

        public async Task<ExpenseDto> GetByIdAsync(int userId, int expenseId)
        {
            var e = await _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.Receipt)
                .FirstOrDefaultAsync(e => e.ExpenseId == expenseId && e.UserId == userId);

            if (e == null) throw new Exception("Expense not found.");

            return new ExpenseDto
            {
                ExpenseId = e.ExpenseId,
                Amount = e.Amount,
                Date = e.Date,
                CategoryName = e.Category.Name,
                CategoryColor = e.Category.ColorCode,
                PaymentMethod = e.PaymentMethod,
                Description = e.Description,
                IsRecurring = e.IsRecurring,
                ReceiptPath = e.Receipt != null ? e.Receipt.FilePath : null,
                CreatedAt = e.CreatedAt
            };
        }

        public async Task<ExpenseDto> UpdateAsync(int userId, int expenseId, UpdateExpenseDto dto)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.ExpenseId == expenseId && e.UserId == userId);

            if (expense == null) throw new Exception("Expense not found.");

            expense.Amount = dto.Amount;
            expense.Date = dto.Date;
            expense.CategoryId = dto.CategoryId;
            expense.PaymentMethod = dto.PaymentMethod;
            expense.Description = dto.Description;
            expense.IsRecurring = dto.IsRecurring;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(userId, expenseId);
        }

        public async Task DeleteAsync(int userId, int expenseId)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.ExpenseId == expenseId && e.UserId == userId);

            if (expense == null) throw new Exception("Expense not found.");

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }

}
