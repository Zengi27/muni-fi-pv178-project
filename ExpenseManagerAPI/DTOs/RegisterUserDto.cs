using System.ComponentModel.DataAnnotations;

namespace ExpenseManagerAPI.DTOs;

public class RegisterUserDto
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string FullName { get; set; }

    [Required]
    //[EmailAddress]
    public string Email { get; set; }

    [Required]
    //[DataType(DataType.Password)]
    public string Password { get; set; }
}