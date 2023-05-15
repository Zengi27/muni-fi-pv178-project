using ExpenseManagerAPI.DTOs.Balance;
using ExpenseManagerAPI.Services.User;

namespace ExpenseManagerAPI.Services.Balance;

public class BalanceService
{
    private readonly UserService _userService;

    public BalanceService(UserService userService)
    {
        _userService = userService;
    }
    
    public async Task<ServiceResult<BalanceDto>> CalculateBalance(string username)
    {
        var user = await _userService.GetCurrentUser(username);
        
        if (user == null)
        {
            return ServiceResult<BalanceDto>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var totalIncome = user.Incomes
            .Sum(i => i.Amount);

        var totalExpense = user.Expenses
            .Sum(e => e.Amount);
        
        var balanceDto = new BalanceDto()
        {
            Balance = totalIncome - totalExpense
        };

        return ServiceResult<BalanceDto>.Success(balanceDto);
    }
}