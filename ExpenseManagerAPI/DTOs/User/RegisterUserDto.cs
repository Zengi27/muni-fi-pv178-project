using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManagerAPI.DTOs.User;

public class RegisterUserDto
{
    [NotNull]
    [Required]
    public string? UserName { get; set; }

    [NotNull]
    [Required]
    public string? FullName { get; set; }

    [NotNull]
    [Required]
    //[EmailAddress]
    public string? Email { get; set; }
    
    [NotNull]
    [Required]
    //[DataType(DataType.Password)]
    public string? Password { get; set; }
}