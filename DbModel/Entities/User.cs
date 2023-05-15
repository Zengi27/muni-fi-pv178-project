using Microsoft.AspNetCore.Identity;

namespace DbModel.Entities;

public class User : IdentityUser<int>
{
    [PersonalData]
    public string? FullName { get; set; }

    [PersonalData] 
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    
    [PersonalData]
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}