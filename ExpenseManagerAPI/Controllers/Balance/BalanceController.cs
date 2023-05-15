using ExpenseManagerAPI.Services;
using ExpenseManagerAPI.Services.Balance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers.Balance;

[ApiController]
[Authorize]
[Route("balance")]
public class BalanceController : BaseController
{
    private readonly BalanceService _balanceService;

    public BalanceController(BalanceService balanceService)
    {
        _balanceService = balanceService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _balanceService.CalculateBalance(username);

        return HandleServiceResult(serviceResult);
    }
}