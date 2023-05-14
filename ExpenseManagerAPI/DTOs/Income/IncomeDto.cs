namespace ExpenseManagerAPI.DTOs.Income;

public class IncomeDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}