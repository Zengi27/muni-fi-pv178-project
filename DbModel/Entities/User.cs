using Microsoft.AspNetCore.Identity;

namespace DbModel.Entities;

public class User : IdentityUser<int>
{
    [PersonalData]
    public string FullName { get; set; }
    [PersonalData]
    public ICollection<Income> Incomes { get; set; }
    [PersonalData]
    public ICollection<Expense> Expenses { get; set; }
}