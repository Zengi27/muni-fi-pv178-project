using ExpenseManagerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers;

[ApiController]
[Authorize]
[Route("balance")]
public class BalanceController : ControllerBase
{
    private readonly BalanceService _balanceService;

    public BalanceController(BalanceService balanceService)
    {
        _balanceService = balanceService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        var username = User.Identity.Name;
        var serviceResult = await _balanceService.CalculateBalance(username);

        return HandleServiceResult(serviceResult);
    }
    
    private IActionResult HandleServiceResult<T>(ServiceResult<T> serviceResult)
    {
        switch (serviceResult.ResultCode)
        {
            case ResultCode.Ok:
                return Ok(serviceResult.Data);
            case ResultCode.NoContent:
                return NoContent();
            case ResultCode.NotFound:
                return NotFound(serviceResult.ErrorMessage);
            case ResultCode.Unauthorized:
                return Unauthorized(serviceResult.ErrorMessage);
            default:
                return StatusCode(500);
        }
    }
}