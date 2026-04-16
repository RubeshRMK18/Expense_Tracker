namespace Expense_Tracker.DTO
{
    public class BudgetStatusDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public decimal BudgetLimit { get; set; }
        public decimal AmountSpent { get; set; }
        public decimal SpentPercent { get; set; }
    }
}
