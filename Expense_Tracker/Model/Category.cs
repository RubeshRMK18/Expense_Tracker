using ExpenseTracker.API.Models;

namespace Expense_Tracker.Model
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; } 
        public string Icon { get; set; }  
        public bool IsSystem { get; set; }   

        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Budget> Budgets { get; set; }
    }
}
