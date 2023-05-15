using ExpenseManagerAPI.DTOs.Income;
using ExpenseManagerAPI.Services;
using ExpenseManagerAPI.Services.Income;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers.Income;

[ApiController]
[Authorize]
[Route("incomes")]
public class IncomeController : BaseController
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
    
    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        var username = User.Identity.Name;
        var serviceResult = await _incomeService.GetUserIncomes(username);

        return HandleServiceResult(serviceResult);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var username = User.Identity.Name;
        var serviceResult = await _incomeService.DeleteIncome(username, id);

        return HandleServiceResult(serviceResult);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateExpense(IncomeDto incomeDto)
    {
        var username = User.Identity.Name;
        var serviceResult = await _incomeService.UpdateIncome(username, incomeDto);

        return HandleServiceResult(serviceResult);
    }
    
    [HttpGet("total")]
    public async Task<IActionResult> GetTotalIncome()
    {
        var username = User.Identity.Name;
        var serviceResult = await _incomeService.GetTotalIncome(username);

        return HandleServiceResult(serviceResult);
    }
}