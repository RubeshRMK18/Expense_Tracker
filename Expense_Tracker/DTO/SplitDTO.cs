namespace Expense_Tracker.DTO
{
    public class SplitDTO
    {
        public class SplitExpenseDto
        {
            public decimal Total { get; set; }
            public string Description { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public List<SplitItemDto> Splits { get; set; } = new();
        }

        public class SplitItemDto
        {
            public int CategoryId { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
