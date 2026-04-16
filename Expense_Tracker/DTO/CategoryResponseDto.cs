namespace Expense_Tracker.DTO
{
    public class CategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
