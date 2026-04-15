using ExpenseTracker.API.Models;

namespace Expense_Tracker.Model
{
    public class ReimbursementRequest
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public int ExpenseId { get; set; }
        public ReimbursementStatus Status { get; set; } = ReimbursementStatus.Pending;
        public int? ReviewedBy { get; set; }  // Admin UserId
        public DateTime? ReviewedAt { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Expense Expense { get; set; }
    }

    public enum ReimbursementStatus { Pending, Approved, Rejected }
}
