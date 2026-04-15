using ExpenseTracker.API.Models;
using Expense_Tracker.Model;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<ReimbursementRequest> ReimbursementRequests { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>()
                .Property(u => u.Role).HasConversion<string>();

        
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

      
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

         
            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.Expense)
                .WithOne(e => e.Receipt)
                .HasForeignKey<Receipt>(r => r.ExpenseId);


            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

         
            modelBuilder.Entity<ReimbursementRequest>()
                .HasOne(r => r.Expense)
                .WithOne(e => e.ReimbursementRequest)
                .HasForeignKey<ReimbursementRequest>(r => r.ExpenseId);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Budget>()
                .Property(b => b.LimitAmount).HasColumnType("decimal(18,2)");

    
            modelBuilder.Entity<Expense>()
                .HasIndex(e => new { e.UserId, e.Date });
        }
    }
}