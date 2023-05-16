using System.ComponentModel;

namespace ExpenseManagerAPI.DTOs.Expense;

public class ExpenseDto
{ 
    [DefaultValue(2)]
    public int Id { get; set; }
    
    [DefaultValue(42)]
    public decimal Amount { get; set; }
    
    [DefaultValue("2023-05-10")]
    public DateTime Date { get; set; }
    
    [DefaultValue("Food")]
    public string? Description { get; set; }
}