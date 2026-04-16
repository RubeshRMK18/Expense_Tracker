public class CreateExpenseDto
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    public string PaymentMethod { get; set; }
    public string Description { get; set; }
    public bool IsRecurring { get; set; }


}
