using System.ComponentModel.DataAnnotations;

namespace ExpenseManagerAPI.DTOs.User;

public class ChangeUserPasswordDto
{
    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}