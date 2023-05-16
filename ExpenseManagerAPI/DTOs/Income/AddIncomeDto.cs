using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseManagerAPI.DTOs.Income;

public class AddIncomeDto
{
    [Required]
    [DefaultValue(100)]
    public decimal Amount { get; set; }

    [Required]
    [DefaultValue("2023-05-02")]
    public DateTime Date { get; set; }
    
    [Required]
    [DefaultValue("Salary")]
    public string? Description { get; set; }
}