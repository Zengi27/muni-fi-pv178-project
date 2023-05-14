using ExpenseManagerAPI.DTOs.Income;
using ExpenseManagerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers;

[ApiController]
[Authorize]
[Route("incomes")]
public class IncomeController : ControllerBase
{
    private readonly IncomeService _incomeService;

    public IncomeController(IncomeService incomeService)
    {
        _incomeService = incomeService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddIncome(AddIncomeDto addIncomeDto)
    {
        var username = User.Identity.Name;
        var serviceResult = await _incomeService.AddIncomeForUser(username, addIncomeDto);

        switch (serviceResult.ResultCode)
        {
            case ResultCode.Ok:
                return CreatedAtAction(nameof(GetIncomeById), new { id = serviceResult.Data.Id }, serviceResult.Data);
            case ResultCode.Unauthorized:
                return Unauthorized(serviceResult.ErrorMessage);
            default:
                return StatusCode(500);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetIncomeById(int id)
    {
        var username = User.Identity.Name;
        var serviceResult = await _incomeService.GetUserIncomeById(username, id);
        
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