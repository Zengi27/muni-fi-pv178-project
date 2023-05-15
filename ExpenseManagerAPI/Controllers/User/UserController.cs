using ExpenseManagerAPI.DTOs.User;
using ExpenseManagerAPI.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers.User;

[ApiController]
[Authorize]
[Route("user")]
public class UserController : BaseController
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangeUserPasswordDto changeUserPasswordDto)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _userService.ChangePassword(username, changeUserPasswordDto.CurrentPassword, changeUserPasswordDto.NewPassword);

        return HandleServiceResult(serviceResult);
    }

    [HttpPost("change-username")]
    public async Task<IActionResult> ChangeUsername(string newUsername)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _userService.ChangeUsername(username, newUsername);
        
        return HandleServiceResult(serviceResult);
    }
}