namespace ExpenseTracker.API.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }

        // 🔗 Foreign Key
        public int UserId { get; set; }

        // 🔗 Navigation
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
