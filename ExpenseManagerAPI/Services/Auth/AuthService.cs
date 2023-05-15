using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseManagerAPI.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseManagerAPI.Services.Auth;

public class AuthService
{
    private readonly IConfiguration _config;
    private readonly UserManager<DbModel.Entities.User> _userManager;
    private readonly SignInManager<DbModel.Entities.User> _signInManager;

    public AuthService(IConfiguration config ,UserManager<DbModel.Entities.User> userManager, SignInManager<DbModel.Entities.User> signInManager)
    {
        _config = config;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<AuthenticationResult> LoginAsync(LoginUserDto userDto)
    {
        var result = await _signInManager.PasswordSignInAsync(userDto.UserName, userDto.Password, false, false);

        if (result.Succeeded)
        {
            return new AuthenticationResult
            {
                Success = true,
                Token = await CreateToken(userDto.UserName)
            };
        }

        return new AuthenticationResult
        {
            Success = false,
            ErrorMessage = "Invalid username or password"
        };
    }

    public async Task<AuthenticationResult> RegisterAsync(RegisterUserDto userDto)
    {
        var user = new DbModel.Entities.User
        {
            UserName = userDto.UserName,
            FullName = userDto.FullName,
            Email = userDto.Email
        };

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded)
        {
            return new AuthenticationResult
            {
                Success = true,
                Token = await CreateToken(userDto.UserName)
            };
        }

        return new AuthenticationResult
        {
            Success = false,
            ErrorMessage = result.Errors.FirstOrDefault()?.Description
        };
    }

    private async Task<string> CreateToken(string username)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username)
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(4),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}