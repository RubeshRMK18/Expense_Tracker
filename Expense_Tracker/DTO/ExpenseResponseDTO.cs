namespace ExpenseTracker.API.DTOs
{
    public class ExpenseResponseDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
