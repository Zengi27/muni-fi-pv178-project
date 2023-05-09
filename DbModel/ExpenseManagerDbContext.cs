using DbModel.Entities;
using Microsoft.EntityFrameworkCore;

namespace DbModel;

public class ExpenseManagerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    
    public ExpenseManagerDbContext(DbContextOptions<ExpenseManagerDbContext> options) : base(options)
    {
    }
}