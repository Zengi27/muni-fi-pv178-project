using System.ComponentModel.DataAnnotations;

namespace ExpenseManagerAPI.DTOs;

public class LoginUserDto
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    //[DataType(DataType.Password)]
    public string Password { get; set; }
}