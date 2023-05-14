using ExpenseManagerAPI.DTOs;
using ExpenseManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto userDto)   
    {
        var result = await _authService.LoginAsync(userDto);

        if (!result.Success)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(new { token = result.Token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto userDto)
    {
        var result = await _authService.RegisterAsync(userDto);

        if (!result.Success)
        {
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(new { token = result.Token });
    }
}