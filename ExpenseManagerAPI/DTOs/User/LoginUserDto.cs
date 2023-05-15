using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManagerAPI.DTOs.User;

public class LoginUserDto
{
    [NotNull]
    [Required]
    public string? UserName { get; set; }
    
    [NotNull]
    [Required]
    //[DataType(DataType.Password)]
    public string? Password { get; set; }
}