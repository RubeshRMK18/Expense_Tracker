namespace Expense_Tracker.DTO
{
    public class BudgetDto
    {
        public int CategoryId { get; set; }
        public string Month { get; set; } = string.Empty;
        public decimal BudgetLimit { get; set; }
    }
}
