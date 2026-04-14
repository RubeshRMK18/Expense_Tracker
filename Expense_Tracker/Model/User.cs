namespace ExpenseTracker.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string HashPassword { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Navigation
        public List<Expense> Expenses { get; set; } = new();
    }
}



