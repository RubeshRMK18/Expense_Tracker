using ExpenseTracker.API.Models;

namespace Expense_Tracker.Model
{
    public class Budget
    {
        public int BudgetId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal LimitAmount { get; set; }
        public BudgetPeriod Period { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Category Category { get; set; }
    }
}
public enum BudgetPeriod { Monthly, Quarterly, Yearly }
