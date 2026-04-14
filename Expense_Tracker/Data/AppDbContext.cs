using Expense_Tracker.Model;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }


    }
}
