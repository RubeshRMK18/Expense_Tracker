public class Receipt
{
    public int ReceiptId { get; set; }
    public int ExpenseId { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Expense Expense { get; set; }
}
