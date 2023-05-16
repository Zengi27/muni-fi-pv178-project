using System.ComponentModel;

namespace ExpenseManagerAPI.DTOs.Income;

public class IncomeDto
{
    [DefaultValue(2)]
    public int Id { get; set; }
    
    [DefaultValue(100)]
    public decimal Amount { get; set; }
    
    [DefaultValue("2023-05-02")]
    public DateTime Date { get; set; }
    
    [DefaultValue("Salary")]
    public string? Description { get; set; }
}