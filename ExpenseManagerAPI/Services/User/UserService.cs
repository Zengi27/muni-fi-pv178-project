using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagerAPI.Services.User;

public class UserService
{
    private readonly UserManager<DbModel.Entities.User> _userManager;

    public UserService(UserManager<DbModel.Entities.User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<ServiceResult<bool>> ChangePassword(string username, string currentPassword, string newPassword)
    {
        var user = await GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<bool>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!result.Succeeded)
        {
            return ServiceResult<bool>.Failure("Failed to change password", ResultCode.BadRequest);
        }

        return ServiceResult<bool>.Success(true);
    }
    
    public async Task<ServiceResult<bool>> ChangeUsername(string username, string newUsername)
    {
        var user = await GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<bool>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var result = await _userManager.SetUserNameAsync(user, newUsername);

        if (!result.Succeeded)
        {
            return ServiceResult<bool>.Failure("Failed to change username", ResultCode.BadRequest);
        }

        return ServiceResult<bool>.Success(true);
    }
    
    public async Task<DbModel.Entities.User?> GetCurrentUser(string username)
    {
        var user = await _userManager.Users
            .Include(u => u.Expenses)
            .Include(u => u.Incomes)
            .SingleOrDefaultAsync(u => u.UserName == username);

        return user;
    }
}