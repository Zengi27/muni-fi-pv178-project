namespace ExpenseManagerAPI.Services;

public class AuthenticationResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
}