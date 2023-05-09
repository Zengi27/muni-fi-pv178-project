namespace DbModel.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public ICollection<Income> Incomes { get; set; }
    public ICollection<Expense> Expenses { get; set; }
}