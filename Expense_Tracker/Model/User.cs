using Expense_Tracker.Model;

namespace ExpenseTracker.API.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Budget> Budgets { get; set; }
        public ICollection<Report> Reports { get; set; }
        public ICollection<ReimbursementRequest> ReimbursementRequests { get; set; }
    }
    public enum UserRole { User, Admin }
}



