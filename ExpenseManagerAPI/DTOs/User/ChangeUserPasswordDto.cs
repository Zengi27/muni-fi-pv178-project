using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManagerAPI.DTOs.User;

public class ChangeUserPasswordDto
{
    [Required]
    [NotNull]
    [DataType(DataType.Password)]
    [DefaultValue("Password987")]
    public string? CurrentPassword { get; set; }

    [Required]
    [NotNull]
    [DataType(DataType.Password)]
    [DefaultValue("Password123")]
    public string? NewPassword { get; set; }
}