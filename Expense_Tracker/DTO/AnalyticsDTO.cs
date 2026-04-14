namespace Expense_Tracker.DTO
{
    public class AnalyticsDTO
    {
        public decimal DailyAverage { get; set; }

        public string TopCategory { get; set; }

        public decimal TopCategoryAmount { get; set; }

        public string SpendingTrend { get; set; }

        public string Insight { get; set; }
    }
}
