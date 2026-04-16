using Expense_Tracker.Data;
using Expense_Tracker.Model;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Services
{
    public class BudgetAlertService
    {
        
            private readonly AppDbContext _context;

            public BudgetAlertService(AppDbContext context)
            {
                _context = context;
            }

            public async Task CheckAndCreateAlerts(int userId, int categoryId)
            {
                string month = DateTime.Now.ToString("yyyy-MM");
                int year = DateTime.Now.Year;
                int mon = DateTime.Now.Month;

                var budget = await _context.Budgets
                    .FirstOrDefaultAsync(b =>
                        b.UserId == userId &&
                        b.CategoryId == categoryId &&
                        b.Month == month);

                if (budget == null || budget.BudgetLimit == 0) return;

                var spent = await _context.Expenses
                    .Where(e => e.UserId == userId &&
                                e.CategoryId == categoryId &&
                                e.Date.Year == year &&
                                e.Date.Month == mon)
                    .SumAsync(e => e.Amount);

                decimal percent = spent / budget.BudgetLimit * 100;

                foreach (int threshold in new[] { 80, 100 })
                {
                    if (percent >= threshold)
                    {
                        bool alreadyAlerted = await _context.BudgetAlerts.AnyAsync(a =>
                            a.UserId == userId &&
                            a.CategoryId == categoryId &&
                            a.Month == month &&
                            a.ThresholdPercent == threshold);

                        if (!alreadyAlerted)
                        {
                            _context.BudgetAlerts.Add(new BudgetAlert
                            {
                                UserId = userId,
                                CategoryId = categoryId,
                                Month = month,
                                ThresholdPercent = threshold,
                                AlertedAt = DateTime.Now
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            public async Task<List<BudgetAlert>> GetUserAlerts(int userId)
            {
                return await _context.BudgetAlerts
                    .Where(a => a.UserId == userId)
                    .Include(a => a.Category)
                    .OrderByDescending(a => a.AlertedAt)
                    .ToListAsync();
            }
        }
    }
