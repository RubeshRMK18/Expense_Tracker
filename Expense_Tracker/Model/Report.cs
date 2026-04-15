using ExpenseTracker.API.Models;

namespace Expense_Tracker.Model
{
    public class Report
    {
        public int ReportId { get; set; }
        public int GeneratedBy { get; set; }  
        public ReportType Type { get; set; }
        public string Period { get; set; } 
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public string FilePath { get; set; } 

        public User User { get; set; }
    }

    public enum ReportType { Monthly, Annual, Organisational }
}
}
