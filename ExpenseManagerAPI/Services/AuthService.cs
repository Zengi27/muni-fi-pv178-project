using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DbModel;
using DbModel.Entities;
using ExpenseManagerAPI.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseManagerAPI.Services;

public class AuthService
{
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthService(IConfiguration config ,UserManager<User> userManager, SignInManager<User> signInManager)
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
        var user = new User
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
    
    public async Task<User?> GetCurrentUser(string username)
    {
        var user = await _userManager.Users
            .Include(u => u.Expenses)
            .Include(u => u.Incomes)
            .SingleOrDefaultAsync(u => u.UserName == username);

        return user;
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