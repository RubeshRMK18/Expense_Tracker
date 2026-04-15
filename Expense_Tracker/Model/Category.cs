using ExpenseTracker.API.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string ColorCode { get; set; }
    public string Icon { get; set; }

    public ICollection<Expense> Expenses { get; set; }
}
