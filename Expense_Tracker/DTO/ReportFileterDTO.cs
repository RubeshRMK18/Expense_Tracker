namespace Expense_Tracker.DTO
{
    public class ReportFileterDTO
    {
        public int UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Category { get; set; }
    }
}
