using ExpenseTracker.Models;
using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Model
{
    public class Budget
    {
        [Key]
        public int BudgetId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Month { get; set; } = string.Empty;
        public decimal BudgetLimit { get; set; }

        public User? User { get; set; }
        public Category? Category { get; set; }
    }
}
