namespace ExpenseManagerAPI.DTOs.Expense;

public class ExpenseDto
{ 
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}