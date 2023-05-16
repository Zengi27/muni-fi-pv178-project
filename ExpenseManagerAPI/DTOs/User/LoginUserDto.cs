using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManagerAPI.DTOs.User;

public class LoginUserDto
{
    [Required]
    [NotNull]
    [DefaultValue("Novak22")]
    public string? UserName { get; set; }
    
    
    [Required]
    [NotNull]
    [DefaultValue("Password987")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}