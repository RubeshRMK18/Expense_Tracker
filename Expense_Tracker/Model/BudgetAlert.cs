using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Model
{
    public class BudgetAlert
    {
            [Key]
            public int AlertId { get; set; }
            public int UserId { get; set; }
            public int CategoryId { get; set; }
            public string Month { get; set; } = string.Empty;
            public int ThresholdPercent { get; set; } 
            public DateTime AlertedAt { get; set; } = DateTime.Now;

            public User? User { get; set; }
            public Category? Category { get; set; }
        }
    }
