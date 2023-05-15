using ExpenseManagerAPI.DTOs.Balance;

namespace ExpenseManagerAPI.Services;

public class BalanceService
{
    private readonly AuthService _authService;

    public BalanceService(AuthService authService)
    {
        _authService = authService;
    }
    
    public async Task<ServiceResult<BalanceDto>> CalculateBalance(string username)
    {
        var user = await _authService.GetCurrentUser(username);
        
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