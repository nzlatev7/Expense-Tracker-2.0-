using Expense_Tracker_2._0.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_2._0
{
    public class ExpenseTrackerDbContext : DbContext
    {    
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options) : base(options) { }

        //tables
        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(b => b.UserId)
                .IsRequired(false);
        }
        //fluent API???
    }
}
