namespace ExpenseManagerAPI.DTOs.Expense;

public class ExpenseReportDto
{
    public int Year { get; set; }
    public int? Month { get; set; }
    public decimal TotalAmount { get; set; }
    public IEnumerable<ExpenseDto>? Expenses { get; set; }
}