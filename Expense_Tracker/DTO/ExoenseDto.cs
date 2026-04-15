public class ExpenseDto
{
    public int ExpenseId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string CategoryName { get; set; }
    public string CategoryColor { get; set; }
    public string PaymentMethod { get; set; }
    public string Description { get; set; }
    public bool IsRecurring { get; set; }
    public string ReceiptPath { get; set; }
    public DateTime CreatedAt { get; set; }
}
