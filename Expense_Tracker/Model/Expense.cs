using Expense_Tracker.Model;

public class Expense
{
    public int ExpenseId { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string PaymentMethod { get; set; }
    public string Description { get; set; }
    public bool IsRecurring { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
    public Category Category { get; set; }
    public Receipt Receipt { get; set; }
    public Guid SplitGroupId { get; internal set; }
}
