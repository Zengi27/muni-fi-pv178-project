using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManagerAPI.DTOs.User;

public class RegisterUserDto
{
    [Required]
    [NotNull]
    [DefaultValue("Novak22")]
    public string? UserName { get; set; }

    [Required]
    [NotNull]
    [DefaultValue("Jan Novak")]
    public string? FullName { get; set; }

    [Required]
    [NotNull]
    [EmailAddress]
    [DefaultValue("novak@gmail.com")]
    public string? Email { get; set; }
    
    [Required]
    [NotNull]
    [DataType(DataType.Password)]
    [DefaultValue("Password987")]
    public string? Password { get; set; }
}