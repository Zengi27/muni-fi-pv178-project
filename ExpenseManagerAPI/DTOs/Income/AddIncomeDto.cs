namespace ExpenseManagerAPI.DTOs.Income;

public class AddIncomeDto
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}