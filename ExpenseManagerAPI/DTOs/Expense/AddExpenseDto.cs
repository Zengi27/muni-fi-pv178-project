namespace ExpenseManagerAPI.DTOs.Expense;

public class AddExpenseDto
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}