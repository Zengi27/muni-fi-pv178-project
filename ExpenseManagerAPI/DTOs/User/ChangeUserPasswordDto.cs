using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseManagerAPI.DTOs.User;

public class ChangeUserPasswordDto
{
    [NotNull]
    [Required]
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    [NotNull]
    [Required]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }
}