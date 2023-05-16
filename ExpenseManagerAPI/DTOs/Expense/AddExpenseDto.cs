using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagerAPI.DTOs.Expense;

public class AddExpenseDto
{
    [Required]
    [DefaultValue(42)]
    public decimal Amount { get; set; }
    
    [Required]
    [DefaultValue("2023-05-10")]
    public DateTime Date { get; set; }
    
    [Required]
    [DefaultValue("Food")]
    public string? Description { get; set; }
}