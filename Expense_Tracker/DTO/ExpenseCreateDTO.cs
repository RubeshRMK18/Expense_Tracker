namespace ExpenseTracker.API.DTOs
{
    public class ExpenseCreateDTO
    {
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int UserId { get; set; }
    }
}
